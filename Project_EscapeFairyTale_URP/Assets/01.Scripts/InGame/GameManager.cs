using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

     public bool isGameOver = false;

    [Header("Management")]
    public InventoryManager inventoryManager;
    public SpriteBox spriteBox;
    public AudioBox audioBox;

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

    [Header("SelectedColorAnim")]
    public Color color_select = Color.red;
    public Color color_anim1 = Color.red;
    public Color color_anim2 = Color.red;

    public static Dictionary<string, int> saveDic = new Dictionary<string, int>();

    private void Awake()
    {
        if(Instance)
        {
            Debug.LogError("다수의 게임매니저 실행중");
        }

        Instance = this;

        DataLoad();
    }

    void Start()
    {
        color_select = color_anim1;
        ColorChange(false);

        MouseEvent.MouseLock(false);
        UIManager.ChangeToSubCamera(Vector3.one, Quaternion.Euler(0,0,0));
    }

    [ContextMenu("Save")]
    void DataSave()
    {
        // 이거 말고도, 플레이어 인벤토리와 플레이어 위치를 저장해야한다.
        string json = JsonUtility.ToJson(new Serialization<string, int>(saveDic));
        SecurityPlayerPrefs.SetString("object-save", json);
        SecurityPlayerPrefs.SetString("playerPos-save", $"{player.transform.position.x} {player.transform.position.y} {player.transform.position.z}");
        SecurityPlayerPrefs.SetString("playerInventory-save",
            $"{inventoryManager.tabs[0].itemId} {inventoryManager.tabs[1].itemId} {inventoryManager.tabs[2].itemId}" +
            $" {inventoryManager.tabs[3].itemId} {inventoryManager.tabs[4].itemId} {inventoryManager.tabs[5].itemId}");
        SecurityPlayerPrefs.SetString("playerInventoryCount-save",
            $"{inventoryManager.tabs[0].itemCount} {inventoryManager.tabs[1].itemCount} {inventoryManager.tabs[2].itemCount}" +
            $" {inventoryManager.tabs[3].itemCount} {inventoryManager.tabs[4].itemCount} {inventoryManager.tabs[5].itemCount}");
        print("Save Complete");
    }

    [ContextMenu("Load")]
    void DataLoad()
    {
        string json = SecurityPlayerPrefs.GetString("object-save", "{}");
        saveDic = JsonUtility.FromJson<Serialization<string, int>>(json).ToDictionary();

        string playerPos = SecurityPlayerPrefs.GetString("playerPos-save", "1.27 0.102 4.44");
        string[] poses = playerPos.Split(' ');
        player.transform.position = new Vector3(float.Parse(poses[0]), float.Parse(poses[1]), float.Parse(poses[2]));

        string playerItem = SecurityPlayerPrefs.GetString("playerInventory-save", "-1 -1 -1 -1 -1 -1");
        string[] items = playerItem.Split(' ');
        for(int i = 0;i<items.Length;i++)
        {
            inventoryManager.tabs[i].itemId = int.Parse(items[i]);
        }

        string playerItemCount = SecurityPlayerPrefs.GetString("playerInventoryCount-save", "-1 -1 -1 -1 -1 -1");
        string[] itemCounts = playerItemCount.Split(' ');
        for (int i = 0; i < items.Length; i++)
        {
            inventoryManager.tabs[i].itemCount = int.Parse(itemCounts[i]);
        }

        print("Load Complete");
    }

    [ContextMenu("Reset")]
    void DataReset()
    {
        SecurityPlayerPrefs.SetString("object-save", "{}");
        SecurityPlayerPrefs.SetString("playerPos-save", "1.27 0.102 4.44");
        SecurityPlayerPrefs.SetString("playerInventory-save", "-1 -1 -1 -1 -1 -1");
        SecurityPlayerPrefs.SetString("playerInventoryCount-save", "-1 -1 -1 -1 -1 -1");
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

    public static void PlaySFX(AudioSource source, AudioClip clip, float volume = 1)
    {
        source.PlayOneShot(clip, volume);
    }

    public static void PlaySFX(AudioClip clip, float volume = 1)
    {
        Instance.defaultSFXSource.PlayOneShot(clip, volume);
    }

    [ContextMenu("DataLog")]
    public void SaveLog()
    {
        string a = JsonUtility.ToJson(new Serialization<string, int>(saveDic));
        print(a);
    }
}
