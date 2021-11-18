using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class Cinderella_Bell : MonoBehaviour
{
    public Cinderella_Clock_minute minute;
    public AudioSource bgmPianoSource;
    public AudioSource bgmCorrectSource;
    public Window[] windows;

    public Button inputButton;
    public InputField spellInput;
    public string answer;

    [System.Serializable]
    public struct Window
    {
        public Transform left;
        public Transform right;
    };

    private Coroutine bellCoroutine;

    private int count = 0;

    private void Start()
    {
        inputButton.onClick.AddListener(() =>
        {
            UIManager.AliceClockInput(false);

            string spell = spellInput.text;
            spell.Replace(" ", "");

            if(spell.Equals(answer) && count == 3)
            {
                // 정답
                StopCoroutine(bellCoroutine);
                minute.SetClockValue(-1, -1);
                DOTween.To(() => GameManager.Instance.clock_color_select,
                value => GameManager.Instance.clock_color_select = value, new Color(0, 1, 1), 2);
                bgmPianoSource.DOFade(0, 0.25f);
                bgmCorrectSource.volume = 0;
                bgmCorrectSource.Play();
                bgmCorrectSource.DOFade(1, 1f);
                bgmCorrectSource.DOPitch(1.5f, 7f).SetEase(Ease.InExpo);

                UIManager.instance.subCamera.transform.DOShakePosition(10, 0.02f, 10, 90, false, true);

                GameManager.Instance.urpSettings.TryGet<Vignette>(out Vignette vignette);
                DOTween.To(() => vignette.color.value, value => vignette.color.value = value, new Color(0, 1, 1), 7).SetEase(Ease.InExpo).OnComplete(() =>
                {
                    DOTween.To(() => vignette.color.value, value => vignette.color.value = value, Color.black, 3);
                    DOTween.To(() => vignette.intensity.value, value => vignette.intensity.value = value, 1, 7).OnComplete(() =>
                    {
                        DOTween.To(() => UIManager.instance.blackScreenCanvasGroup.alpha,
                            value => UIManager.instance.blackScreenCanvasGroup.alpha = value, 1, 2).OnComplete(() =>
                            {
                                vignette.intensity.value = 0.272f;
                                DOTween.To(() => UIManager.instance.blackScreenCanvasGroup.alpha,
                                value => UIManager.instance.blackScreenCanvasGroup.alpha = value, 0, 2).OnComplete(() =>
                                {
                                    Cinderella_Clock_TrueButton.HappyEnding();
                                    UIManager.instance.subCamera_Back.interactable = true;
                                    UIManager.instance.subCamera_Back.image.DOFade(1, 1);
                                });
                            });
                    });
                    // 화면 까맣게 한후


                });
            }
            else
            {
                StopCoroutine(bellCoroutine);
                bgmPianoSource.DOPitch(0, 1f);
                count = 0;
                Cinderella_Clock_TrueButton.isCleared = false;
                for (int i = 0; i < windows.Length; i++)
                {
                    if (i != 2)
                    {
                        windows[i].left.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
                        windows[i].right.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
                    }
                    else
                    {
                        windows[i].left.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
                    }
                }
            }
        });
    }

    public void BellStart()
    {
        bgmPianoSource.pitch = 1;
        GameManager.PlaySFX(bgmPianoSource, GameManager.Instance.audioBox.object_clock_ambient, SoundType.BGM);
        bellCoroutine = StartCoroutine(Bell());
    }

    IEnumerator Bell()
    {
        yield return new WaitForSeconds(6.25f);
        while (count < 5)
        {
            GameManager.PlaySFX(GameManager.Instance.audioBox.object_clock_bell);

            if (count != 2)
            {
                windows[count].left.transform.DOLocalRotate(new Vector3(0, 0, 120), 1).SetRelative();
                windows[count].right.transform.DOLocalRotate(new Vector3(0, 0, -120), 1).SetRelative();
            }
            else
            {
                windows[count].left.transform.DOLocalRotate(new Vector3(0, 0, -95), 1).SetRelative();
            }

            yield return new WaitForSeconds(3);
            count++;
        }

        count = 0;
        Cinderella_Clock_TrueButton.isCleared = false;
        for (int i = 0; i < windows.Length; i++)
        {
            if (i != 2)
            {
                windows[i].left.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
                windows[i].right.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
            }
            else
            {
                windows[i].left.transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
            }
        }
    }
}
