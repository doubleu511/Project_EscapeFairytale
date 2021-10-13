using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(cakeslice.Outline))]
public class SelectableObject : MonoBehaviour
{
    cakeslice.Outline outline;

    protected virtual void Awake()
    {
        outline = GetComponent<cakeslice.Outline>();
    }

    public virtual void OnHighlighted()
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
