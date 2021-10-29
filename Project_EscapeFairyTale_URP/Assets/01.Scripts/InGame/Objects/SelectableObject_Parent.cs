using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject_Parent : SelectableObject
{
    public List<SelectableObject> selectableObjects; // 부모만 설정해준다.
    public bool pickable = false;
    public int itemId = 0;

    [HideInInspector] public bool isSubCameraMove;
    [HideInInspector] public Vector3 movePos;
    [HideInInspector] public Vector3 moveRot;

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
    /// 자식이 아웃라인이 있을 경우에만, 그리고 부모 오브젝트에서만 호출해야합니다.
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
    /// 자식이 아웃라인이 있을 경우에만, 그리고 부모 오브젝트에서만 호출해야합니다.
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
                Invoke("DestroyObj", 0.5f);
                GameManager.Instance.inventoryManager.TIP_ItemGotTipAppear(GameManager.Instance.itemData.infos[itemId].itemSprite);
            }
            else
            {
                // 인벤토리 꽉참
                GameManager.Instance.inventoryManager.TIP_FullInventory();
            }
        }
        else if (isSubCameraMove)
        {
            UIManager.ChangeToSubCamera(movePos, Quaternion.Euler(moveRot.x, moveRot.y, moveRot.z));
        }
    }

    private void DestroyObj()
    {
        Destroy(this.gameObject);
    }
}
