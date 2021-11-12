using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    void Update()
    {
        InputInGame();
    }

    void InputInGame()
    {
        if (GameManager.Instance.player.playerState == PlayerState.DEAD) return;
        if (GameManager.Instance.player.playerState == PlayerState.WAKING_UP) return;

        if (Input.GetKeyDown(KeyCode.E))
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

        if (Input.GetKeyDown(KeyCode.Escape))
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

                    GameObject obj = Instantiate(GameManager.Instance.itemData.infos[itemId].itemPrefab, Camera.main.transform.position, transform.rotation).GetComponent<PickableObject>().Drop();
                    float scale = Item_SizeChange.dropItemSizeScale[Item_SizeChange.sizeValueRaw + 1];
                    obj.transform.localScale = new Vector3(scale, scale, scale);
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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(UIManager.instance.isTutorialPanelAppear)
            {
                UIManager.TutorialPanel("");
            }
        }
    }
}
