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
        if (TitleManager.Instance)
        {
            TitleManager.PlaySFX(TitleManager.Instance.audioBox.ui_tapSound);
        }
        else if(GameManager.Instance)
        {
            GameManager.PlaySFX(GameManager.Instance.audioBox.ui_tapSound);
        }
        myImg.DOColor(gotoColor, duration).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myImg.DOColor(defaultColor, duration).SetUpdate(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isClickable)
        {
            myImg.DOKill();
            myImg.DOColor(gotoClickColor, duration).SetUpdate(true);
        }
    }
}
