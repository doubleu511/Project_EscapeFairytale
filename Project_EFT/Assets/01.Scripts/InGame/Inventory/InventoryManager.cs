using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryManager : MonoBehaviour
{
    public CanvasGroup inventoryPanel;
    public Image selectedImg;
    public TabScript[] tabs;

    public void Open()
    {
        foreach(TabScript item in tabs)
        {
            item.Refresh();
        }

        DOTween.To(() => inventoryPanel.alpha, value => inventoryPanel.alpha = value, 1, 0.75f);
        inventoryPanel.blocksRaycasts = true;
        inventoryPanel.interactable = true;
        MouseEvent.MouseLock(false);
        GameManager.Instance.player.playerState = PlayerState.OPEN_INVENTORY;
    }

    public void Close()
    {
        DOTween.To(() => inventoryPanel.alpha, value => inventoryPanel.alpha = value, 0, 0.75f);
        inventoryPanel.blocksRaycasts = false;
        inventoryPanel.interactable = false;
        MouseEvent.MouseLock(true);
        GameManager.Instance.player.playerState = PlayerState.NORMAL;
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
