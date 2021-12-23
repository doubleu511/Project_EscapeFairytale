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
    public Transform foodGenerate;
    private List<Transform> foodGeneratePoses = new List<Transform>();
    public int muffinCountLeft = 6;
    public int milkCountLeft = 6;

    [Header("Inventory")]
    public ItemData itemData;
    public int selectedItemId;
    public TabScript selectedTab;
    public GameObject selectedItemUI;
    public List<PickableObject> pickableObjectList = new List<PickableObject>(); // 처음부터 꺼진 애들은 미리 넣어준다.

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
    [HideInInspector] public bool isInventoryFull = false;

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError("다수의 게임매니저 실행중");
        }

        Instance = this; // 싱글톤
        allSource = FindObjectsOfType<AudioSource>(); // 모든 사운드 소스를 찾는다.

        PickableObject[] pickableObjects = FindObjectsOfType<PickableObject>(); // 주울 수 있는 것들을 모두 담아준다.
        foreach(PickableObject item in pickableObjects)
        {
            pickableObjectList.Add(item);
        }

        DataLoad(); // 데이터 로드
    }

    void Start()
    {
        color_select = color_anim1; // 선택 가능 오브젝트 아웃라인 색깔 설정 -> 애니메이션 후 오브젝트에서 참조하게
        foodGenerate.GetComponentsInChildren<Transform>(foodGeneratePoses); // 랜덤 생성될 음식위치들 저장
        foodGeneratePoses.RemoveAt(0); // 부모는 지운다

        ColorChange(false);
    }

    public static void Save()
    {
        Instance.StartCoroutine(Instance.SaveCoroutine()); // 세이브 코루틴
    }

    IEnumerator SaveCoroutine() // 세이브 링 출현 후 세이브
    {
        UIManager.CanvasGroup_DefaultShow(UIManager.instance.saveRing, true, true);
        yield return new WaitForSeconds(1);
        DataSave();
        UIManager.CanvasGroup_DefaultShow(UIManager.instance.saveRing, false, true);
    }

    [ContextMenu("Save")]
    public void DataSave() // 필요한 데이터를 저장하고, 로드할때 string을 형식에 맞게 변환하여 로드한다.
    {
        // 딕셔너리를 직렬화하여 json으로 저장한다.
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
        SecurityPlayerPrefs.SetString("saved-food-remain", $"{muffinCountLeft} {milkCountLeft}");
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

        string foodRemain = SecurityPlayerPrefs.GetString("saved-food-remain", "6 6");
        string[] foods = foodRemain.Split(' ');
        muffinCountLeft = int.Parse(foods[0]);
        milkCountLeft = int.Parse(foods[1]);

        print("Load Complete");
    }

    [ContextMenu("Reset")]
    public void debug_Reset() // 디버그 - 데이터 리셋
    {
        DataReset();
    }

    public static void DataReset() // 디버그 - 데이터 리셋
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
        SecurityPlayerPrefs.SetString("saved-food-remain", "6 6");
        print("Reset Complete");
    }

    private void ColorChange(bool start) // 선택 가능 오브젝트 아웃라인 색깔 애니메이션
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

    public static void PlaySFX(AudioClip clip, float volume = 1) // 효과음 재생
    {
        Instance.defaultSFXSource.PlayOneShot(clip, volume * SettingManager.sfxVolume);
    }

    public static void PlaySFX(AudioSource source, AudioClip clip, SoundType type, float volume = 1) // 효과음 소스를 넣은 후 조절
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

    public void SoundSourceInit() // 효과음 초기화
    {
        foreach (AudioSource item in allSource)
        {
            if (item.outputAudioMixerGroup != null)
            {
                if (item.outputAudioMixerGroup.name == "BGM") // BGM이라면 볼륨을 설정된 수치로 바꿔준다.
                {
                    item.DOComplete();
                    item.volume = SettingManager.bgmVolume;
                }
            }
        }
    }

    [ContextMenu("DataLog")]
    public void SaveLog() // 디버그 - 오브젝트 저장 데이터 로그 출력
    {
        string a = JsonUtility.ToJson(new Serialization<string, string>(saveDic));
        print(a);
    }

    public PickableObject FindDisabledObject(int itemId) // 지금 꺼져있는 오브젝트를 리스트에서 찾아서 리턴해준다.
    {
        for(int i = 0; i<pickableObjectList.Count;i++)
        {
            if(pickableObjectList[i].itemId == itemId)
            {
                if (!pickableObjectList[i].gameObject.activeSelf)
                {
                    Book book = pickableObjectList[i].GetComponent<Book>(); // 근데 책이라면, 아이템 아이디를 다른 방식으로 썼으므로 넘어간다.
                    if (book != null) continue;

                    return pickableObjectList[i];
                }
            }
        }

        return null;
    }

    public void CreateMuffinTest() // 만약 맵에 남아있는 머핀 수가 1개 이하라면, 머핀을 새로 만드는 함수
    {
        const int MUFFIN_ID = 2;

        muffinCountLeft--;

        if(muffinCountLeft <= 0)
        {
            muffinCountLeft = 1;
            // 만든다.
            int randomIdx = UnityEngine.Random.Range(0, foodGeneratePoses.Count);

            PickableObject muffin = FindDisabledObject(MUFFIN_ID);
            if(muffin != null)
            {
                float randomRangeX = 0.07f;
                float randomRangeZ = 0.07f;
                Vector3 pos = foodGeneratePoses[randomIdx].position;

                float randomX = pos.x + UnityEngine.Random.Range(-randomRangeX, randomRangeX);
                float randomZ = pos.z + UnityEngine.Random.Range(-randomRangeZ, randomRangeZ);

                Vector3 randomPos = new Vector3(randomX, pos.y, randomZ);

                muffin.ObjectOn(randomPos, foodGeneratePoses[randomIdx].rotation, foodGeneratePoses[randomIdx].localScale);
            }
        }
    }

    public void CreateMilkTest() // 만약 맵에 남아있는 우유 수가 1개 이하라면, 우유를 새로 만드는 함수
    {
        const int MILK_ID = 3;

        milkCountLeft--;

        if (milkCountLeft <= 0)
        {
            milkCountLeft = 1;
            // 만든다.
            int randomIdx = UnityEngine.Random.Range(0, foodGeneratePoses.Count);

            PickableObject milk = FindDisabledObject(MILK_ID);
            if (milk != null)
            {
                float randomRangeX = 0.07f;
                float randomRangeZ = 0.07f;
                Vector3 pos = foodGeneratePoses[randomIdx].position;

                float randomX = pos.x + UnityEngine.Random.Range(-randomRangeX, randomRangeX);
                float randomZ = pos.z + UnityEngine.Random.Range(-randomRangeZ, randomRangeZ);

                Vector3 randomPos = new Vector3(randomX, pos.y, randomZ);

                milk.ObjectOn(randomPos, foodGeneratePoses[randomIdx].rotation, foodGeneratePoses[randomIdx].localScale);
            }
        }
    }
}
