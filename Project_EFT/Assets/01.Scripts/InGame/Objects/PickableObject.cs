using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : SelectableObject
{
    public int itemId;

    public override void OnClicked()
    {
        if(GameManager.Instance.inventoryManager.TryGetRemainingTab(itemId, out TabScript tab))
        {
            if (tab.itemId != -1)
            {
                tab.itemCount++;
            }
            else
            {
                tab.itemId = itemId;
                tab.itemCount = 1;
            }
            base.OnDisHighlighted();
            GameManager.Instance.inventoryManager.SelectedItemRefresh();
            gameObject.SetActive(false);
            GameManager.Instance.inventoryManager.TIP_ItemGotTipAppear(GameManager.Instance.itemData.infos[itemId].itemSprite);
        }
        else
        {
            // 인벤토리 꽉참
            GameManager.Instance.inventoryManager.TIP_FullInventory();
        }
    }
}
