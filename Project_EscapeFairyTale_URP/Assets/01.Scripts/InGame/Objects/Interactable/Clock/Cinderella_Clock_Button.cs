using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cinderella_Clock_Button : SelectableObject
{
    private static Cinderella_Clock_Button instance;

    public Cinderella_Clock_minute minute;

    public Outline clockCircle_outline;
    public ParticleSystem clearParticle;
    public Vector2 answer;

    private bool _isCleared = false;
    public static bool isCleared { get { return instance._isCleared; }
        set
        {
            instance._isCleared = value;
            if(!value)
            {
                DOTween.To(() => instance.clockCircle_outline.outlineWidth,
                value => instance.clockCircle_outline.outlineWidth = value, 0, 1);
                instance.minute.SetClockValue(11, 0);
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }

    public override void OnClicked()
    {
        if (isCleared) return;

        if (Cinderella_Clock.hour == answer.x && Cinderella_Clock.minute == answer.y) // 정답
        {
            Debug.Log("클리어");
            isCleared = true;
            minute.ignoreRaycast_inSubCam = true;

            Sequence seq = DOTween.Sequence();

            GameManager.Instance.clock_color_select = GameManager.Instance.color_anim1;
            clockCircle_outline.enabled = true;
            clockCircle_outline.colorType = Outline.HighLightColor.ClockColor;

            seq.Append(DOTween.To(() => GameManager.Instance.clock_color_select,
                value => GameManager.Instance.clock_color_select = value, Color.white, 2));
            seq.AppendCallback(() =>
            {
                clockCircle_outline.outlineWidth = 30;
                clearParticle.Play();
            });
            seq.Join(DOTween.To(() => clockCircle_outline.outlineWidth,
                value => clockCircle_outline.outlineWidth = value, 5, 1));
        }
    }
}
