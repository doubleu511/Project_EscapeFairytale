using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : SelectableObject
{
    public int itemId;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnHighlighted(string text)
    {
        base.OnHighlighted(text);
        UIManager.instance.cursorBtTipText.text = text;
    }

    public override void OnDisHighlighted()
    {
        base.OnDisHighlighted();
        UIManager.instance.cursorBtTipText.text = "";
    }

    public override void OnClicked()
    {
        if(GameManager.Instance.inventoryManager.TryGetNullTab(out TabScript tab))
        {
            tab.itemId = itemId;
            gameObject.SetActive(false);
            GameManager.Instance.inventoryManager.TIP_ItemGotTipAppear(GameManager.Instance.itemData.infos[itemId].itemSprite);
        }
        else
        {
            // �κ��丮 ����
            GameManager.Instance.inventoryManager.TIP_FullInventory();
        }
    }
}
