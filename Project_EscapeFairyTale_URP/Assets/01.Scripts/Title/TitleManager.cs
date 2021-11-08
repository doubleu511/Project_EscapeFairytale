using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{
    public static TitleManager Instance;

    public CanvasGroup detailPanel;
    public GameObject[] mainPanels;
    public GameObject[] moreDetailPanels;
    public VolumeProfile urpSettings;

    private bool isOpened = false;
    private Tween urpVignetteTween;
    private Tween urpVignetteTween2;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
    }

    public void MainButtonEvent(int index)
    {
        if(isOpened)
        {
            detailPanel.DOFade(0, 0.25f).OnComplete(() =>
            {
                MainTab(index);
                detailPanel.DOFade(1, 0.4f);
            });
        }
        else
        {
            MainTab(index);
            detailPanel.DOFade(1, 0.4f);
        }

        isOpened = true;
        URPVignetteTween(new Color(37f / 255, 13f / 255, 12f / 255), 0.2f, 3);
        // 사운드
    }

    public void MainTab(int index)
    {
        switch (index)
        {
            case 3:
                {
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false; //play모드를 false로.
                    #else
                    Application.Quit();
                    #endif

                    return;
                }
            default:
                {
                    foreach(GameObject item in mainPanels)
                    {
                        item.SetActive(false);
                    }
                    mainPanels[index].SetActive(true);
                }
                break;
        }
    }

    public void URPVignetteTween(Color gotoColor, float intro, float outro)
    {
        urpSettings.TryGet<Vignette>(out Vignette vignette);
        urpVignetteTween.Kill();
        urpVignetteTween2.Kill();

        urpVignetteTween = DOTween.To(() => vignette.color.value, value => vignette.color.value = value, gotoColor, intro).OnComplete(() =>
             {
                 urpVignetteTween2 = DOTween.To(() => vignette.color.value, value => vignette.color.value = value, Color.black, outro);
             });
    }
}
