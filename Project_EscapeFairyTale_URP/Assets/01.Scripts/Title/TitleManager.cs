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

    public Transform[] camerarandomPos;

    public CanvasGroup detailPanel;
    public GameObject[] mainPanels;
    public GameObject[] moreDetailPanels;
    public VolumeProfile urpSettings;

    [Header("AudioSource")]
    public AudioSource defaultSFXSource;
    public AudioBox audioBox;

    private bool isOpened = false;
    private Tween urpVignetteTween;
    private Tween urpVignetteTween2;
    private AudioSource[] allSource;

    [Header("Option")]
    public Dropdown graphics_WindowMode;
    public Dropdown graphics_Resolution;
    public Slider sounds_bgmSlider;
    public Slider sounds_sfxSlider;
    public Text sounds_bgmText;
    public Text sounds_sfxText;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }

        Camera.main.transform.position = camerarandomPos[Random.Range(0, camerarandomPos.Length)].transform.position;
        allSource = FindObjectsOfType<AudioSource>();
    }

    private void Start()
    {
        AudioListener.volume = 1;

        // 설정 드롭다운
        graphics_WindowMode.onValueChanged.AddListener(value => SettingManager.ScreenMode(value));
        graphics_Resolution.onValueChanged.AddListener(value => SettingManager.Resolution(value));
        sounds_bgmSlider.onValueChanged.AddListener(value => SettingManager.BGMVolume(value));
        sounds_bgmSlider.onValueChanged.AddListener(value => sounds_bgmText.text = Mathf.RoundToInt(value * 100).ToString());
        sounds_sfxSlider.onValueChanged.AddListener(value => SettingManager.SFXVolume(value));
        sounds_sfxSlider.onValueChanged.AddListener(value => sounds_sfxText.text = Mathf.RoundToInt(value * 100).ToString());

        int fullScreenValue = SecurityPlayerPrefs.GetInt(SettingManager.FULL_SCREEN, 0);
        int resolutionValue = SecurityPlayerPrefs.GetInt(SettingManager.RESOLUTION, 0);
        float bgmValue = SecurityPlayerPrefs.GetFloat(SettingManager.BGMVOLUME, 1);
        float sfxValue = SecurityPlayerPrefs.GetFloat(SettingManager.SFXVOLUME, 1);

        graphics_WindowMode.value = fullScreenValue;
        graphics_Resolution.value = resolutionValue;
        sounds_bgmSlider.value = bgmValue;
        sounds_sfxSlider.value = sfxValue;

        SettingManager.bgmVolume = bgmValue;
        SettingManager.sfxVolume = sfxValue;
    }

    public void MainButtonEvent(int index)
    {
        PlaySFX(audioBox.ui_menu_select, 0.5f);

        if(isOpened)
        {
            if (mainPanels[index].activeInHierarchy) return;

            detailPanel.DOKill();
            detailPanel.DOFade(0, 1f).OnComplete(() =>
            {
                MainTab(index);
                detailPanel.DOFade(1, 0.5f);
                detailPanel.interactable = true;
                detailPanel.blocksRaycasts = true;
            });
            detailPanel.interactable = false;
            detailPanel.blocksRaycasts = false;
        }
        else
        {
            MainTab(index);
            detailPanel.DOKill();
            detailPanel.DOFade(1, 0.5f);
            detailPanel.interactable = true;
            detailPanel.blocksRaycasts = true;
        }

        isOpened = true;
        URPVignetteTween(new Color(37f / 255, 13f / 255, 12f / 255), 0.2f, 3);
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

    public static void PlaySFX(AudioClip clip, float volume = 1)
    {
        Instance.defaultSFXSource.PlayOneShot(clip, volume * SettingManager.sfxVolume);
    }

    public static void PlaySFX(AudioSource source, AudioClip clip, SoundType type, float volume = 1)
    {
        if (type == SoundType.BGM)
        {
            source.clip = clip;
            source.volume = volume * SettingManager.bgmVolume;
            source.Play();
        }
        else
        {
            source.PlayOneShot(clip, volume * SettingManager.sfxVolume);
        }
    }

    public void SoundSourceInit()
    {
        foreach (AudioSource item in allSource)
        {
            if (item.outputAudioMixerGroup != null)
            {
                if (item.outputAudioMixerGroup.name == "BGM")
                {
                    item.DOComplete();
                    item.volume = SettingManager.bgmVolume;
                }
            }
        }
    }
}
