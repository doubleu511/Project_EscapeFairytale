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

        //DataLoad();
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
        string a = JsonUtility.ToJson(new Serialization<string, int>(saveDic));
        print(a);
    }

    [ContextMenu("Load")]
    void DataLoad()
    {
        string b = "{\"keys\":[\"pickable_milk5\",\"pickable_milk0\",\"pickable_milk3\",\"pickable_milk1\",\"pickable_milk2\",\"pickable_milk4\",\"pickable_cupcake0\",\"\",\"pickable_cupcake5\",\"pickable_book3\",\"pickable_cupcake3\",\"pickable_cupcake1\",\"pickable_book5\",\"pickable_cupcake4\",\"pickable_book2\",\"pickable_book0\",\"pickable_book1\",\"pickable_cupcake2\",\"pickable_book4\"],\"values\":[1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0]}";
        saveDic = JsonUtility.FromJson<Serialization<string, int>>(b).ToDictionary();
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

    [ContextMenu("saveTest")]
    public void SaveLog()
    {
        string a = JsonUtility.ToJson(new Serialization<string, int>(saveDic));
        print(a);
    }
}
