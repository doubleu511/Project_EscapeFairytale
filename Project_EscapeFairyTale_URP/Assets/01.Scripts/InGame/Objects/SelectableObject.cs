using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class SelectableObject : MonoBehaviour
{
    public string selectText;
    public bool ignoreRaycast = false;
    public bool ignoreRaycast_inSubCam = false;
    public bool highlightedIndependent = false;
    Outline outline;

    [System.NonSerialized]
    public SelectableObject_Parent parentObj = null;

    protected virtual void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public virtual void OnHighlighted(string text)
    {
        if (parentObj != null && !highlightedIndependent)
        {
            foreach (SelectableObject item in parentObj.selectableObjects)
            {
                if (item.gameObject.activeInHierarchy)
                {
                    if (item.outline.colorType == Outline.HighLightColor.Default)
                    {
                        item.outline.enabled = true;
                    }
                }
            }
        }
        else
        {
            if (gameObject.activeInHierarchy)
            {
                if (outline.colorType == Outline.HighLightColor.Default)
                {
                    outline.enabled = true;
                }
            }
        }
        UIManager.instance.cursorBtTipText.text = text;
    }

    public virtual void OnDisHighlighted()
    {
        if (parentObj != null && !highlightedIndependent)
        {
            foreach (SelectableObject item in parentObj.selectableObjects)
            {
                if (item.gameObject.activeInHierarchy)
                {
                    if (item.outline.colorType == Outline.HighLightColor.Default)
                    {
                        item.outline.enabled = false;
                    }
                }
            }
        }
        else
        {
            if (gameObject.activeInHierarchy)
            {
                if (outline.colorType == Outline.HighLightColor.Default)
                {
                    outline.enabled = false;
                }
            }
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
