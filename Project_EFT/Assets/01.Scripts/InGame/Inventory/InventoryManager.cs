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

    [Space(10)]
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
        group.DOKill();
        if (!open)
        {
            group.DOFade(0,time).OnComplete(() =>
            {
                group.blocksRaycasts = false;
                group.interactable = false;
            });
        }
        else
        {
            group.blocksRaycasts = true;
            group.interactable = true;
            group.DOFade(1, time);
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

        if (!GameManager.Instance.player.isSubCam)
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

    public bool TryGetRemainingTab(int currentItemId, out TabScript tab)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            if(tabs[i].itemId == currentItemId)
            {
                tab = tabs[i];
                return true;
            }
        }

        for (int i = 0; i < tabs.Length; i++)
        {
            if (tabs[i].itemId == -1)
            {
                tab = tabs[i];
                return true;
            }
        }

        tab = null;
        return false;
    }

    public void DecreaseTab(int tabIndex)
    {
        tabs[tabIndex].itemCount--;

        if (tabs[tabIndex].itemCount <= 0)
        {
            tabs[tabIndex].itemId = -1;
            if (tabIndex == GameManager.Instance.selectedTab.tabId)
            {
                GameManager.Instance.selectedItemId = -1;
                GameManager.Instance.selectedTab = null;
            }
        }

        GameManager.Instance.inventoryManager.SelectedItemRefresh();
        foreach (TabScript item in GameManager.Instance.inventoryManager.tabs)
        {
            item.Refresh();
        }
    }

    public void TIP_FullInventory()
    {
        UIManager.Tip_RBAppear(null, "인벤토리가 가득 찼습니다!", 0.5f, 3, 2);
    }

    public void TIP_ItemGotTipAppear(Sprite sprite)
    {
        UIManager.Tip_RBAppear(sprite, "아이템을 획득하였습니다.", 0.5f, 3, 2);
    }
}
