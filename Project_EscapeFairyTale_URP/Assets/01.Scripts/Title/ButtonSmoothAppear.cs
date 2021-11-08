using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonSmoothAppear : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image myImg;
    private Color defaultColor;

    public bool isClickable = false;
    public Color gotoClickColor;
    public Color gotoColor;
    public float duration;

    private void Awake()
    {
        myImg = GetComponent<Image>();
    }

    private void Start()
    {
        defaultColor = myImg.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        myImg.DOColor(gotoColor, duration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myImg.DOColor(defaultColor, duration);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isClickable)
        {
            myImg.DOKill();
            myImg.DOColor(gotoClickColor, duration);
        }
    }
}
