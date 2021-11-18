using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cinderella_Clock_TrueButton : SelectableObject
{
    private static Cinderella_Clock_TrueButton instance;
    public static bool cin_bad = false;

    public Cinderella_Clock clock;
    public Cinderella_Clock_minute minute;

    public Outline clockCircle_outline;
    public ParticleSystem clearParticle;
    public Vector2 time_answer;
    public Vector2 disSpell_answer;

    public Vector3 clearPos;
    public Cinderella_Bell bell;

    [Header("시계 조작 이후")]
    public GameObject glass_segment;
    public GameObject glass_shoes;
    public GameObject glass_key;
    public GameObject iron_key;
    public Door lastDoor;

    private bool _isCleared = false;
    public static bool isCleared { get { return instance._isCleared; }
        set
        {
            instance._isCleared = value;
            if(!value)
            {
                DOTween.To(() => instance.clockCircle_outline.outlineWidth,
                value => instance.clockCircle_outline.outlineWidth = value, 0, 1);
                UIManager.instance.subCamera.transform.DOMove(instance.clock.teleportPos, 1);
                instance.minute.SetClockValue(11, 0);
                instance.clockCircle_outline.enabled = false;
                UIManager.instance.subCamera_Back.interactable = true;
                UIManager.instance.subCamera_Back.image.DOFade(1, 1);
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }

    private void Start()
    {
        GetComponent<Outline>().enabled = true;
        GameManager.Instance.clock_color_select = Color.white;
    }

    public override void OnClicked()
    {
        if (isCleared) return;
        if (!minute.trueClock) return;

        if (Cinderella_Clock.hour == time_answer.x && Cinderella_Clock.minute == time_answer.y) // 정답
        {
            isCleared = true;
            minute.ignoreRaycast_inSubCam = true;
            UIManager.instance.subCamera_Back.interactable = false;
            UIManager.instance.subCamera_Back.image.DOFade(0, 1);
            GameManager.PlaySFX(GameManager.Instance.audioBox.object_clock_powerOn);

            Sequence seq = DOTween.Sequence();

            GameManager.Instance.clock_color_select = GameManager.Instance.color_anim1;
            clockCircle_outline.enabled = true;
            clockCircle_outline.outlineWidth = 5;
            clockCircle_outline.colorType = Outline.HighLightColor.ClockColor;

            seq.Append(DOTween.To(() => GameManager.Instance.clock_color_select,
                value => GameManager.Instance.clock_color_select = value, Color.white, 2));
            seq.AppendCallback(() =>
            {
                clockCircle_outline.outlineWidth = 30;
                clearParticle.Play();
                GameManager.PlaySFX(GameManager.Instance.audioBox.object_clock_bomb);
            });
            seq.Join(DOTween.To(() => clockCircle_outline.outlineWidth,
                value => clockCircle_outline.outlineWidth = value, 5, 1));
            seq.AppendCallback(() => bell.BellStart());
            seq.AppendInterval(2);
            seq.Append(UIManager.instance.subCamera.transform.DOMove(clearPos, 1));
            seq.AppendCallback(() =>
            {
                UIManager.AliceClockInput(true);
            });
        }
        else if (Cinderella_Clock.hour == disSpell_answer.x && Cinderella_Clock.minute == disSpell_answer.y) // 정답 - 주문 해제
        {
            isCleared = true;
            minute.ignoreRaycast_inSubCam = true;
            UIManager.instance.subCamera_Back.interactable = false;
            UIManager.instance.subCamera_Back.image.DOFade(0, 1);
            GameManager.PlaySFX(GameManager.Instance.audioBox.object_clock_powerOn);

            Sequence seq = DOTween.Sequence();

            GameManager.Instance.clock_color_select = GameManager.Instance.color_anim1;
            clockCircle_outline.enabled = true;
            clockCircle_outline.outlineWidth = 5;
            clockCircle_outline.colorType = Outline.HighLightColor.ClockColor;

            seq.Append(DOTween.To(() => GameManager.Instance.clock_color_select,
                value => GameManager.Instance.clock_color_select = value, Color.gray, 2));
            seq.AppendCallback(() =>
            {
                clockCircle_outline.outlineWidth = 30;
                clearParticle.Play();
                GameManager.PlaySFX(GameManager.Instance.audioBox.object_clock_bigbell);
            });
            seq.Join(DOTween.To(() => clockCircle_outline.outlineWidth,
                value => clockCircle_outline.outlineWidth = value, 5, 1));
            seq.AppendCallback(() =>
            {
                GameManager.PlaySFX(GameManager.Instance.audioBox.object_clock_glassbreak);
                UIManager.instance.subCamera_Back.interactable = true;
                UIManager.instance.subCamera_Back.image.DOFade(1, 1);
                BadEnding();
            });
        }
    }

    public static void BadEnding()
    {
        instance.glass_segment.SetActive(false);
        instance.iron_key.SetActive(true);
        instance.lastDoor.requireItemId = 9;
        cin_bad = true;
    }

    public static void HappyEnding()
    {
        instance.glass_segment.SetActive(false);
        instance.glass_shoes.SetActive(true);
        instance.glass_key.SetActive(true);
        instance.lastDoor.requireItemId = 10;
        cin_bad = false;
    }
}
