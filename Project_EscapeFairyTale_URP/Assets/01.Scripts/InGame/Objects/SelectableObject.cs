using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class SelectableObject : MonoBehaviour
{
    [TextArea]
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

    // 이건 PlayerAction에서 충돌한 오브젝트를 해당 스크립트를 GetComponent 해준뒤 있다면 실행해줄 함수들이다.
    // 또한 상속하여 덮어씌울 수 있다.

    public virtual void OnHighlighted(string text) // 마우스가 해당 오브젝트에 있다면?
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

    public virtual void OnDisHighlighted() // 마우스가 해당 오브젝트를 더이상 보지 않을때
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

    public virtual void OnClicked() // 마우스가 해당 오브젝트를 보고 클릭했을때 (상속 전용)
    {
        if (parentObj != null)
        {
            parentObj.OnClicked();
        }
    }
}
