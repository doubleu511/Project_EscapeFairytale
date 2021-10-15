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
        outline.enabled = true;
    }

    public virtual void OnDisHighlighted()
    {
        outline.enabled = false;
    }

    public virtual void OnClicked()
    {

    }
}
