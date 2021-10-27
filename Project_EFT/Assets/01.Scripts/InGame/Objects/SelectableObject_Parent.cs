using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject_Parent : SelectableObject
{
    public List<SelectableObject> selectableObjects; // �θ� �������ش�.
    public bool pickable = false;
    public int itemId = 0;

    protected virtual void Start()
    {
        if (selectableObjects.Count > 0)
        {
            for (int i = 0; i < selectableObjects.Count; i++)
            {
                selectableObjects[i].parentObj = this;
            }
        }
    }

    /// <summary>
    /// �ڽ��� �ƿ������� ���� ��쿡��, �׸��� �θ� ������Ʈ������ ȣ���ؾ��մϴ�.
    /// </summary>
    public void OnParentDisable()
    {
        ignoreRaycast = true;
        if (selectableObjects.Count > 0)
        {
            for (int i = 0; i < selectableObjects.Count; i++)
            {
                selectableObjects[i].parentObj = null;
                selectableObjects[i].OnDisHighlighted();
            }
        }
        OnDisHighlighted();
    }

    /// <summary>
    /// �ڽ��� �ƿ������� ���� ��쿡��, �׸��� �θ� ������Ʈ������ ȣ���ؾ��մϴ�.
    /// </summary>
    public void OnParentEnable()
    {
        ignoreRaycast = false;
        if (selectableObjects.Count > 0)
        {
            for (int i = 0; i < selectableObjects.Count; i++)
            {
                selectableObjects[i].parentObj = this;
            }
        }
    }

    public override void OnClicked()
    {
        if (pickable)
        {
            if (GameManager.Instance.inventoryManager.TryGetRemainingTab(itemId, out TabScript tab))
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
                // �κ��丮 ����
                GameManager.Instance.inventoryManager.TIP_FullInventory();
            }
        }
    }
}
