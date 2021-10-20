using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(cakeslice.Outline))]
public class SelectableObject : MonoBehaviour
{
    public string selectText;
    cakeslice.Outline outline;

    [SerializeField]
    private List<SelectableObject> selectableObjects; // 부모만 설정해준다.

    protected virtual void Awake()
    {
        outline = GetComponent<cakeslice.Outline>();
    }

    private void Start()
    {
        if(selectableObjects.Count > 0)
        {
            for(int i = 0; i<selectableObjects.Count;i++)
            {
                selectableObjects[i].selectableObjects = selectableObjects;
            }
        }
    }

    public virtual void OnHighlighted(string text)
    {
        if (selectableObjects.Count > 0)
        {
            foreach (SelectableObject item in selectableObjects)
            {
                item.outline.eraseRenderer = false;
                cakeslice.OutlineEffect.Instance?.AddOutline(item.outline);
            }
        }
        else
        {
            outline.eraseRenderer = false;
            cakeslice.OutlineEffect.Instance?.AddOutline(outline);
        }
        UIManager.instance.cursorBtTipText.text = text;
    }

    public virtual void OnDisHighlighted()
    {
        if (selectableObjects.Count > 0)
        {
            foreach (SelectableObject item in selectableObjects)
            {
                item.outline.eraseRenderer = true;
                cakeslice.OutlineEffect.Instance?.RemoveOutline(item.outline);
            }
        }
        else
        {
            outline.eraseRenderer = true;
            cakeslice.OutlineEffect.Instance?.RemoveOutline(outline);
        }
        UIManager.instance.cursorBtTipText.text = "";
    }

    public virtual void OnClicked()
    {

    }
}
