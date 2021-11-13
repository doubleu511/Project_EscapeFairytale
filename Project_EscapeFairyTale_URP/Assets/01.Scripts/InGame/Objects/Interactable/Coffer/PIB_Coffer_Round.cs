using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public enum PIB_Coffer_Color
{
    YELLOW,
    RED,
    AQUA,
    PINK,
    ORANGE,
    LIME,
    GRAY,
    PURPLE,
    NULL
}

public class PIB_Coffer_Round : SelectableObject, ISaveAble
{
    private bool isHighlighted = false; // true�� ���콺�� ��ħ�� ���Ŵ�.
    private bool dragStart = false;

    private Vector3 beforePos;
    private Vector3 nowPos;
    private Vector3 centerPos = new Vector3(960, 540);

    private int beforeAngle = 0;

    public Color uiDefaultColor;

    public PIB_Coffer_Color prevColor = PIB_Coffer_Color.NULL;
    public PIB_Coffer_Color currentColor = PIB_Coffer_Color.YELLOW;

    public PIB_Coffer_Color[] input = new PIB_Coffer_Color[4];
    public PIB_Coffer_Color[] answer = new PIB_Coffer_Color[4];
    private int index = 0;

    public PIB_Coffer_Handle handle;

    public Color[] highlightColor;
    public Image[] highlightTips = new Image[4];

    public Transform round_dummy;

    private AudioSource tick_audioSource;

    [Header("Save")]
    public string saveKey;
    private string _eventFlow = "lock";
    public string eventFlow
    {
        get { return _eventFlow; }
        set
        {
            if (_eventFlow != value)
            {
                _eventFlow = value;
                TempSave();
            }
        }
    } // 1�̸� ���, 0�̸� ��

    protected override void Awake()
    {
        base.Awake();
        tick_audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (!saveKey.Equals(""))
        {
            if (!GameManager.saveDic.ContainsKey(saveKey))
            {
                GameManager.saveDic.Add(saveKey, eventFlow);
            }
            else
            {
                Load();
            }
        }
    }

    public override void OnHighlighted(string text)
    {
        base.OnHighlighted(text);
        isHighlighted = true;
    }

    public override void OnDisHighlighted()
    {
        base.OnDisHighlighted();
        isHighlighted = false;
    }

    public override void OnClicked()
    {
        centerPos = UIManager.instance.subCamera.WorldToScreenPoint(transform.position);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isHighlighted)
            {
                dragStart = true;
                beforePos = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragStart = false;
        }

        if (dragStart)
        {
            nowPos = Input.mousePosition;

            Vector3 currentDir = beforePos - centerPos;
            Vector3 mouseDir = nowPos - centerPos;

            float angle = Vector2.SignedAngle(mouseDir, currentDir);

            angle *= 0.5f;

            round_dummy.transform.localRotation *= Quaternion.Euler(0, angle, 0);
            beforePos = nowPos;

            // ���� �� ������ �������� �ϱ�.
            int intAngle = Mathf.FloorToInt(-round_dummy.localRotation.eulerAngles.y + 360);

            int angleRemainder = intAngle % 5;

            intAngle = (intAngle / 5) * 5 + (angleRemainder > 2 ? 5 : 0);

            transform.localRotation = Quaternion.Euler(0, intAngle, 0);

            if (beforeAngle != intAngle)
            {
                if (!tick_audioSource.isPlaying)
                {
                    GameManager.PlaySFX(tick_audioSource, GameManager.Instance.audioBox.object_coffer_tick, SoundType.SFX);
                }

                switch (intAngle)
                {
                    case 360:
                        OverrideColor(PIB_Coffer_Color.YELLOW);
                        break;
                    case 45:
                        OverrideColor(PIB_Coffer_Color.RED);
                        break;
                    case 90:
                        OverrideColor(PIB_Coffer_Color.AQUA);
                        break;
                    case 135:
                        OverrideColor(PIB_Coffer_Color.PINK);
                        break;
                    case 180:
                        OverrideColor(PIB_Coffer_Color.ORANGE);
                        break;
                    case 225:
                        OverrideColor(PIB_Coffer_Color.LIME);
                        break;
                    case 270:
                        OverrideColor(PIB_Coffer_Color.GRAY);
                        break;
                    case 315:
                        OverrideColor(PIB_Coffer_Color.PURPLE);
                        break;
                }

            }

            beforeAngle = intAngle;
        }
    }

    void OverrideColor(PIB_Coffer_Color color)
    {
        if (currentColor != color)
        {
            if(prevColor == color)
            {
                if (index < 4)
                {
                    ColorSelect(currentColor);
                }
            }
            prevColor = currentColor;
            currentColor = color;
        }
    }

    void ColorSelect(PIB_Coffer_Color color)
    {
        input[index] = color;
        GameManager.PlaySFX(handle.coffer_audioSource, GameManager.Instance.audioBox.object_coffer_light_on, SoundType.SFX);
        highlightTips[index].DOColor(highlightColor[(int)color], 1).OnComplete(() =>
        {
            if(index == 4)
            {
                AnswerCheck();
            }
        });
        index++;
    }

    void AnswerCheck()
    {
        for(int i = 0; i<input.Length;i++)
        {
            if (input[i] != answer[i])
            {
                GameManager.PlaySFX(handle.coffer_audioSource, GameManager.Instance.audioBox.object_coffer_wrong, SoundType.SFX);

                Sequence seq = DOTween.Sequence();
                foreach (Image item in highlightTips)
                {
                    seq.Insert(0, item.DOColor(uiDefaultColor, 0.25f));
                }

                foreach (Image item in highlightTips)
                {
                    seq.Insert(0.25f, item.DOColor(Color.red, 0.25f).SetLoops(4, LoopType.Yoyo).OnComplete(() =>
                     {
                         prevColor = PIB_Coffer_Color.NULL;
                         item.DOColor(uiDefaultColor, 0.25f);
                         index = 0;
                     }));
                }
                return;
            }
        }

        GameManager.PlaySFX(handle.coffer_audioSource, GameManager.Instance.audioBox.object_coffer_correct, SoundType.SFX);
        foreach (Image item in highlightTips)
        {
            item.DOColor(new Color(0.5f, 1, 0), 0.5f).OnComplete(() =>
            {
                eventFlow = "unlock";
                handle.UnLock(false);
            });
        }
    }

    public void TempSave()
    {
        if (saveKey != "")
        {
            GameManager.saveDic[saveKey] = eventFlow;
        }
    }

    public void Load()
    {
        eventFlow = GameManager.saveDic[saveKey];
        if (eventFlow.Equals("unlock"))
        {
            for (int i = 0; i < highlightTips.Length; i++)
            {
                highlightTips[i].color = new Color(0.5f, 1, 0);
            }

            handle.UnLock(true);
        }
    }
}
