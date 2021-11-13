using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.IO;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isGameOver = false;


    public readonly static Vector3 playerDefaultPos = new Vector3(-6.443f, 0.14f, 5.8f);

    [Header("Management")]
    public InventoryManager inventoryManager;
    public SpriteBox spriteBox;
    public AudioBox audioBox;
    public VolumeProfile urpSettings;

    [Header("Player")]
    public PlayerController player;

    [Header("Inventory")]
    public ItemData itemData;
    public int selectedItemId;
    public TabScript selectedTab;
    public GameObject selectedItemUI;

    [Header("ItemEffects")]
    public UnityEvent[] itemUseCallback;

    [Header("AudioSource")]
    public AudioSource defaultSFXSource;
    private AudioSource[] allSource;

    [Header("SelectedColorAnim")]
    public Color clock_color_select;

    public Color color_select = Color.red;
    public Color color_anim1 = Color.red;
    public Color color_anim2 = Color.red;

    public static Dictionary<string, string> saveDic = new Dictionary<string, string>();

    [HideInInspector] public bool isTutorial = false;
    //TutorialBoolean
    [HideInInspector] public bool isEatenItem = false;
    [HideInInspector] public bool isUsedItem = false;
    [HideInInspector] public bool isSmalled = false;

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError("다수의 게임매니저 실행중");
        }

        Instance = this;
        allSource = FindObjectsOfType<AudioSource>();

        DataLoad();
    }

    void Start()
    {
        color_select = color_anim1;
        ColorChange(false);
    }

    [ContextMenu("Save")]
    public void DataSave()
    {
        // 이거 말고도, 플레이어 인벤토리와 플레이어 위치를 저장해야한다.
        string json = JsonUtility.ToJson(new Serialization<string, string>(saveDic));
        SecurityPlayerPrefs.SetString("object-save", json);
        SecurityPlayerPrefs.SetString("playerPos-save", $"{player.transform.position.x} {player.transform.position.y} {player.transform.position.z}");
        SecurityPlayerPrefs.SetString("playerInventory-save",
            $"{inventoryManager.tabs[0].itemId} {inventoryManager.tabs[1].itemId} {inventoryManager.tabs[2].itemId}" +
            $" {inventoryManager.tabs[3].itemId} {inventoryManager.tabs[4].itemId} {inventoryManager.tabs[5].itemId}");
        SecurityPlayerPrefs.SetString("playerInventoryCount-save",
            $"{inventoryManager.tabs[0].itemCount} {inventoryManager.tabs[1].itemCount} {inventoryManager.tabs[2].itemCount}" +
            $" {inventoryManager.tabs[3].itemCount} {inventoryManager.tabs[4].itemCount} {inventoryManager.tabs[5].itemCount}");
        SecurityPlayerPrefs.SetBool("saved-file-exists", true);
        SecurityPlayerPrefs.SetInt("playerSize-save", Item_SizeChange.sizeValueRaw);
        SecurityPlayerPrefs.SetString("saved-dateTime", DateTime.Now.ToString());
        Screenshot.TakeScreenshot();
        print("Save Complete");
    }

    [ContextMenu("Load")]
    public void DataLoad()
    {
        string json = SecurityPlayerPrefs.GetString("object-save", "{}");
        saveDic = JsonUtility.FromJson<Serialization<string, string>>(json).ToDictionary();

        string playerPos = SecurityPlayerPrefs.GetString("playerPos-save", $"{playerDefaultPos.x} {playerDefaultPos.y} {playerDefaultPos.z}");
        string[] poses = playerPos.Split(' ');
        player.transform.position = new Vector3(float.Parse(poses[0]), float.Parse(poses[1]), float.Parse(poses[2]));

        string playerItem = SecurityPlayerPrefs.GetString("playerInventory-save", "-1 -1 -1 -1 -1 -1");
        string[] items = playerItem.Split(' ');
        for (int i = 0; i < items.Length; i++)
        {
            inventoryManager.tabs[i].itemId = int.Parse(items[i]);
        }

        string playerItemCount = SecurityPlayerPrefs.GetString("playerInventoryCount-save", "-1 -1 -1 -1 -1 -1");
        string[] itemCounts = playerItemCount.Split(' ');
        for (int i = 0; i < items.Length; i++)
        {
            inventoryManager.tabs[i].itemCount = int.Parse(itemCounts[i]);
        }

        bool fileExists = SecurityPlayerPrefs.GetBool("saved-file-exists", false);
        if (!fileExists)
        {
            // 튜토리얼
            urpSettings.TryGet<Vignette>(out Vignette vignette);
            vignette.intensity.value = 1;
            DOTween.To(() => vignette.intensity.value, value => vignette.intensity.value = value, 0.272f, 2).SetDelay(1);

            player.GetComponent<Animator>().Play("Player_Wakeup");
            player.playerState = PlayerState.WAKING_UP;
            isTutorial = true;
        }
        else
        {
            UIManager.InGameAppear(true);
        }

        int playerSize = SecurityPlayerPrefs.GetInt("playerSize-save", 0);
        Item_SizeChange.SetSizeInstant(playerSize);

        print("Load Complete");
    }

    [ContextMenu("Reset")]
    public void debug_Reset()
    {
        DataReset();
    }

    public static void DataReset()
    {
        SecurityPlayerPrefs.SetString("object-save", "{}");

        SecurityPlayerPrefs.SetString("playerPos-save", $"{playerDefaultPos.x} {playerDefaultPos.y} {playerDefaultPos.z}");

        SecurityPlayerPrefs.SetString("playerInventory-save", "-1 -1 -1 -1 -1 -1");
        SecurityPlayerPrefs.SetString("playerInventoryCount-save", "-1 -1 -1 -1 -1 -1");
        SecurityPlayerPrefs.SetBool("saved-file-exists", false);

        string path = Application.dataPath;
        path = Path.Combine(path, "../sc/saved.png");
        if (File.Exists(path))
        {
            File.Delete(path);
        }


        SecurityPlayerPrefs.SetString("saved-dateTime", "");
        SecurityPlayerPrefs.SetInt("playerSize-save", 0);
        print("Reset Complete");
    }

    private void ColorChange(bool start)
    {
        if (start)
        {
            DOTween.To(() => color_select, value => color_select = value, color_anim1, 1).OnComplete(() => ColorChange(false));
        }
        else
        {
            DOTween.To(() => color_select, value => color_select = value, color_anim2, 1).OnComplete(() => ColorChange(true));
        }
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

    [ContextMenu("DataLog")]
    public void SaveLog()
    {
        string a = JsonUtility.ToJson(new Serialization<string, string>(saveDic));
        print(a);
    }
}
