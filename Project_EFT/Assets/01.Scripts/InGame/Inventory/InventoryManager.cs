using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryManager : MonoBehaviour
{
    [Header("Panels")]
    public CanvasGroup inventoryPanel;
    public CanvasGroup itemDetailPanel;

    [Header("ItemDetail")]
    public Text itemName;
    public Text itemLore;
    public Image itemImg;
    public Button backBtn;
    public Button into3DBtn;

    public Image selectedImg;
    public TabScript[] tabs;

    [System.NonSerialized]
    public int tabClickCount = 0;

    private void Start()
    {
        backBtn.onClick.AddListener(() =>
        {
            ItemDetailClose();
        });
    }

    public void Popup(CanvasGroup group, bool open, float time = 0.5f)
    {
        if (!open)
        {
            DOTween.To(() => group.alpha, value => group.alpha = value, 0, time).OnComplete(() =>
            {
                group.blocksRaycasts = false;
                group.interactable = false;
            });
        }
        else
        {
            group.blocksRaycasts = true;
            group.interactable = true;
            DOTween.To(() => group.alpha, value => group.alpha = value, 1, time);
        }
    }

    public void InventoryOpen()
    {
        foreach(TabScript item in tabs)
        {
            item.Refresh();
        }

        Popup(inventoryPanel, true);
        MouseEvent.MouseLock(false);
        GameManager.Instance.player.playerState = PlayerState.OPEN_INVENTORY;
    }

    public void InventoryClose()
    {
        Popup(inventoryPanel, false);
        MouseEvent.MouseLock(true);
        GameManager.Instance.player.playerState = PlayerState.NORMAL;
    }

    public void ItemDetailOpen()
    {
        itemName.text = GameManager.Instance.itemData.infos[GameManager.Instance.selectedItemId].itemName;
        itemLore.text = GameManager.Instance.itemData.infos[GameManager.Instance.selectedItemId].itemLore;
        itemImg.sprite = GameManager.Instance.itemData.infos[GameManager.Instance.selectedItemId].itemSprite;

        Popup(itemDetailPanel, true);
    }

    public void ItemDetailClose()
    {
        Popup(itemDetailPanel, false);
    }

    public void SelectedItemRefresh()
    {
        if (GameManager.Instance.selectedItemId != -1)
        {
            selectedImg.sprite = GameManager.Instance.itemData.infos[GameManager.Instance.selectedItemId].itemSprite;
        }
        else
        {
            selectedImg.sprite = null;
            GameManager.Instance.selectedItemUI.SetActive(false);
        }
    }
}
