using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [Header("RightBottomTip")]
    [Header("Tips")]
    [Space(10)]
    public CanvasGroup tip_rb;
    public Image tip_rb_Img;
    public Text tip_rb_Txt;

    [Header("CursorBottomTip")]
    public Text cursorBtTipText;

    [Header("SizeChangeTip")]
    public CanvasGroup tip_size;
    public RectTransform tip_size_arrow;
    public RectTransform[] tip_size_signs;

    private void Awake()
    {
        if(!instance)
        instance = this;
    }

    public static void Tip_RBAppear(Sprite sprite, string text, float appearTime, float waitTime, float disappearTime)
    {
        instance.tip_rb.DOComplete();
        instance.tip_rb.alpha = 0;
        instance.tip_rb.DOFade(1, appearTime);
        instance.tip_rb_Img.sprite = sprite;
        instance.tip_rb_Txt.text = text;

        instance.tip_rb.DOFade(0, disappearTime).SetDelay(waitTime);
    }

    public static void Tip_SizeChange(int index)
    {
        instance.tip_size.DOComplete();
        instance.tip_size.alpha = 0;
        instance.tip_size.DOFade(1, 0.1f);
        instance.tip_size_arrow.DOMoveX(instance.tip_size_signs[index].transform.position.x, 1.5f).SetEase(Ease.OutBounce);

        instance.tip_size.DOFade(0, 0.5f).SetDelay(3f);
    }
}
