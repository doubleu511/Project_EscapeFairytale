using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : SelectableObject
{
    private Rigidbody rb;

    public int itemId;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    public override void OnHighlighted()
    {
        base.OnHighlighted();
        UIManager.instance.cursorBtTipText.text = "클릭하여 아이템을 획득할 수 있습니다.";
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
            // 인벤토리 꽉참
            GameManager.Instance.inventoryManager.TIP_FullInventory();
        }
    }
}
