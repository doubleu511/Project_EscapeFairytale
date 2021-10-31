using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject_Parent : SelectableObject
{
    public List<SelectableObject> selectableObjects; // 부모만 설정해준다.

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

    /// <summary>
    /// 로직 또는 퍼즐이 끝났을때, 퍼즐 parent를 비활성화 시키기 위함입니다.
    /// </summary>
    public void IgnoreCamRayCast()
    {
        GetComponent<Collider>().enabled = false;
        foreach(SelectableObject item in selectableObjects)
        {
            item.GetComponent<Collider>().enabled = false;
        }
    }

    public override void OnClicked()
    {
        if (isSubCameraMove)
        {
            UIManager.ChangeToSubCamera(movePos, Quaternion.Euler(moveRot.x, moveRot.y, moveRot.z));
            UIManager.instance.currentShowObject = this;
        }
    }
}
