using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    void Update()
    {
        InputInGame();
    }

    void InputInGame()
    {
        // 인풋 - 플레이어 스테이트에 따라서 적용될 인풋을 다르게 설정하여 조절한다.


        if (Input.GetKeyDown(KeyCode.R)) // 재시작
        {
            if (GameManager.Instance.isGameOver)
            {
                DOTween.KillAll();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        if (GameManager.Instance.player.playerState == PlayerState.DEAD) return;
        if (GameManager.Instance.player.playerState == PlayerState.WAKING_UP) return;
        if (GameManager.Instance.player.playerState == PlayerState.ENDING) return;

        if (Input.GetKeyDown(KeyCode.E)) // 인벤토리
        {
            if (GameManager.Instance.player.playerState != PlayerState.OPEN_BOOK)
            {
                if (GameManager.Instance.player.playerState == PlayerState.NORMAL)
                {
                    GameManager.Instance.inventoryManager.InventoryOpen();
                }
                else if (GameManager.Instance.player.playerState == PlayerState.OPEN_INVENTORY)
                {
                    GameManager.Instance.inventoryManager.InventoryClose();
                    GameManager.Instance.inventoryManager.ItemDetailClose();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // 일시 정지 또는 취소
        {
            if (GameManager.Instance.player.playerState == PlayerState.OPEN_INVENTORY)
            {
                GameManager.Instance.inventoryManager.InventoryClose();
                GameManager.Instance.inventoryManager.ItemDetailClose();
            }
            else if (GameManager.Instance.player.playerState == PlayerState.OPEN_BOOK)
            {
                GameManager.Instance.player.playerState = PlayerState.NORMAL;
                UIManager.BookDefaultUI(false);
                UIManager.BookDetailClose();

                if (!GameManager.Instance.player.isSubCam)
                    MouseEvent.MouseLock(true);
            }
            else if (GameManager.Instance.player.playerState == PlayerState.NORMAL)
            {
                UIManager.PauseUI(true);
            }
            else if (GameManager.Instance.player.playerState == PlayerState.PAUSED)
            {
                UIManager.PauseUI(false);
            }
        }

        if (Input.GetButtonDown("Fire1")) // 마우스 왼클릭
        {
            if (GameManager.Instance.player.playerState == PlayerState.NORMAL)
            {
                if (PlayerAction.currentObj != null)
                {
                    PlayerAction.currentObj.GetComponent<SelectableObject>().OnClicked();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F)) // 아이템 사용
        {
            if (GameManager.Instance.selectedItemId != -1)
            {
                if (GameManager.Instance.itemUseCallback[GameManager.Instance.selectedItemId] != null)
                {
                    GameManager.Instance.itemUseCallback[GameManager.Instance.selectedItemId].Invoke();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q)) // 아이템 버리기
        {
            if (!GameManager.Instance.player.isSubCam)
            {
                if (GameManager.Instance.selectedItemId != -1)
                {
                    int itemId = GameManager.Instance.selectedItemId;
                    GameManager.Instance.inventoryManager.DecreaseTab(GameManager.Instance.selectedTab.tabId);

                    float scale = Item_SizeChange.dropItemSizeScale[Item_SizeChange.sizeValueRaw + 1];
                    GameObject obj = GameManager.Instance.FindDisabledObject(itemId).ObjectOn(Camera.main.transform.position, transform.rotation, new Vector3(scale, scale, scale));
                    obj.GetComponent<PickableObject>().Drop();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.B)) // 책 UI
        {
            if (GameManager.Instance.player.playerState != PlayerState.OPEN_INVENTORY)
            {
                if(GameManager.Instance.player.playerState == PlayerState.NORMAL)
                {
                    GameManager.Instance.player.playerState = PlayerState.OPEN_BOOK;
                    MouseEvent.MouseLock(false);
                    UIManager.BookDefaultUI(true);
                }
                else if (GameManager.Instance.player.playerState == PlayerState.OPEN_BOOK)
                {
                    GameManager.Instance.player.playerState = PlayerState.NORMAL;

                    if (!GameManager.Instance.player.isSubCam)
                        MouseEvent.MouseLock(true);
                    UIManager.BookDefaultUI(false);
                    UIManager.BookDetailClose();
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Space)) // 튜토리얼 스킵
        {
            if(UIManager.instance.isTutorialPanelAppear)
            {
                UIManager.TutorialPanel("");
            }
        }
    }
}
