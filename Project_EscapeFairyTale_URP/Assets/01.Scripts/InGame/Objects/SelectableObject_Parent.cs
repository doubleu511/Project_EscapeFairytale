using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject_Parent : SelectableObject
{
    public List<SelectableObject> selectableObjects; // �θ� �������ش�.

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

    /// <summary>
    /// ���� �Ǵ� ������ ��������, ���� parent�� ��Ȱ��ȭ ��Ű�� �����Դϴ�.
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
