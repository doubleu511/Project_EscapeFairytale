using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(cakeslice.Outline))]
public class SelectableObject : MonoBehaviour
{
    public string selectText;
    public bool ignoreRaycast = false;
    public bool ignoreRaycast_inSubCam = false;
    cakeslice.Outline outline;

    [System.NonSerialized]
    public SelectableObject_Parent parentObj = null;

    protected virtual void Awake()
    {
        outline = GetComponent<cakeslice.Outline>();
    }

    public virtual void OnHighlighted(string text)
    {
        if (parentObj != null)
        {
            foreach (SelectableObject item in parentObj.selectableObjects)
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
        if (parentObj != null)
        {
            foreach (SelectableObject item in parentObj.selectableObjects)
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
        if (parentObj != null)
        {
            parentObj.OnClicked();
        }
    }
}
