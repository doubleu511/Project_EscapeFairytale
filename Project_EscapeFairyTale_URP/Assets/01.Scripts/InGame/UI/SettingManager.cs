using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager
{
    public static readonly string FULL_SCREEN = "option.fullScreen";
    public static readonly string RESOLUTION = "option.resolution";
    public static readonly string BGMVOLUME = "option.bgm";
    public static readonly string SFXVOLUME = "option.sfx";

    public static float bgmVolume = 1;
    public static float sfxVolume = 1;

    public static void ScreenMode(int value)
    {
        Screen.fullScreen = (value == 0) ? true : false;
        SecurityPlayerPrefs.SetInt(FULL_SCREEN, value);
        Debug.Log($"FullScreen {((value == 0) ? true : false)}");
    }

    public static void Resolution(int value)
    {
        switch(value)
        {
            case 0:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
            case 1:
                Screen.SetResolution(1600, 900, Screen.fullScreen);
                break;
            case 2:
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;
            case 3:
                Screen.SetResolution(960, 540, Screen.fullScreen);
                break;
        }

        SecurityPlayerPrefs.SetInt(RESOLUTION, value);
    }

    public static void BGMVolume(float value)
    {
        SecurityPlayerPrefs.SetFloat(BGMVOLUME, value);
        bgmVolume = value;

        if(GameManager.Instance)
        {
            GameManager.Instance.SoundSourceInit();
        }
        else if (TitleManager.Instance)
        {
            TitleManager.Instance.SoundSourceInit();
        }
    }

    public static void SFXVolume(float value)
    {
        SecurityPlayerPrefs.SetFloat(SFXVOLUME, value);
        sfxVolume = value;
    }
}
