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

        if (Input.GetKeyDown(KeyCode.E))
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.player.playerState == PlayerState.OPEN_INVENTORY)
            {
                GameManager.Instance.inventoryManager.InventoryClose();
                GameManager.Instance.inventoryManager.ItemDetailClose();
            }
        }

        if (Input.GetButtonDown("Fire1")) // ���콺 ��Ŭ��
        {
            if (GameManager.Instance.player.playerState == PlayerState.NORMAL)
            {
                if (PlayerAction.currentObj != null)
                {
                    PlayerAction.currentObj.GetComponent<SelectableObject>().OnClicked();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F)) // ������ ���
        {
            if (GameManager.Instance.itemUseCallback[GameManager.Instance.selectedItemId] != null)
            {
                GameManager.Instance.itemUseCallback[GameManager.Instance.selectedItemId].Invoke();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q)) // ������ ������
        {
            if (GameManager.Instance.selectedItemId != -1)
            {
                int itemId = GameManager.Instance.selectedItemId;
                GameManager.Instance.inventoryManager.SetNullTab(GameManager.Instance.selectedTab.tabId);

                Rigidbody rb = Instantiate(GameManager.Instance.itemData.infos[itemId].itemPrefab, transform.position, transform.rotation).GetComponent<Rigidbody>();
                rb.AddForce(Camera.main.transform.forward * 4, ForceMode.Impulse);
            }
        }
    }
}