using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ModelDetailPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        canvasGroup.DOKill();
        canvasGroup.DOFade(1, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canvasGroup.DOKill();
        canvasGroup.DOFade(0, 0.5f);
    }
}
