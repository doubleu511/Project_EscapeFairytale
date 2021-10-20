using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject_Parent : SelectableObject
{
    public List<SelectableObject> selectableObjects; // �θ� �������ش�.

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

    }
}
