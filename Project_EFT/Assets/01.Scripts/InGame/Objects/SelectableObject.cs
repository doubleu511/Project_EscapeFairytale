using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(cakeslice.Outline))]
public class SelectableObject : MonoBehaviour
{
    public string selectText;
    cakeslice.Outline outline;

    protected virtual void Awake()
    {
        outline = GetComponent<cakeslice.Outline>();
    }

    public virtual void OnHighlighted(string text)
    {
        outline.eraseRenderer = false;
        cakeslice.OutlineEffect.Instance?.AddOutline(outline);
        UIManager.instance.cursorBtTipText.text = text;
    }

    public virtual void OnDisHighlighted()
    {
        outline.eraseRenderer = true;
        cakeslice.OutlineEffect.Instance?.RemoveOutline(outline);
        UIManager.instance.cursorBtTipText.text = "";
    }

    public virtual void OnClicked()
    {

    }
}
