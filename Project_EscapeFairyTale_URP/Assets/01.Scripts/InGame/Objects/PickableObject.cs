using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : SelectableObject, ISaveAble
{
    public int itemId;

    [HideInInspector]
    public ItemPlacer itemPlacer;
    [HideInInspector]
    public int itemPlaceIndex;

    [Header("Save")]
    public string saveKey;
    protected string _eventFlow = "true";
    public string eventFlow { get { return _eventFlow; }
        set {
            if (_eventFlow != value)
            {
                _eventFlow = value;
                TempSave();
            }
        }
    } // true이면 오브젝트가 켜진것, false이면 꺼진것 

    protected Coroutine dropWait;

    protected virtual void Start()
    {
        if (!saveKey.Equals(""))
        {
            if (!GameManager.saveDic.ContainsKey(saveKey))
            {
                _eventFlow = $"{transform.position.x} {transform.position.y} {transform.position.z}/" +
                    $"{transform.eulerAngles.x} {transform.eulerAngles.y} {transform.eulerAngles.z}/" +
                    $"{transform.localScale.x} {transform.localScale.y} {transform.localScale.z}";
                GameManager.saveDic.Add(saveKey, eventFlow);
            }
            else
            {
                Load();
            }
        }
    }

    public GameObject ObjectOn(Vector3 pos, Quaternion rotation, Vector3 localScale)
    {
        this.gameObject.transform.position = pos;
        this.gameObject.transform.rotation = rotation;
        this.gameObject.transform.localScale = localScale;
        _eventFlow = $"{transform.position.x} {transform.position.y} {transform.position.z}/" +
            $"{transform.eulerAngles.x} {transform.eulerAngles.y} {transform.eulerAngles.z}/" +
            $"{transform.localScale.x} {transform.localScale.y} {transform.localScale.z}";
        TempSave();
        gameObject.SetActive(true);

        dropWait = StartCoroutine(Wait());

        return this.gameObject;
    }

    public void Drop()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(Camera.main.transform.forward * 2 * GameManager.Instance.player.transform.localScale.x, ForceMode.Impulse);
        }
        else
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.AddForce(Camera.main.transform.forward * 2 * GameManager.Instance.player.transform.localScale.x, ForceMode.Impulse);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        _eventFlow = $"{transform.position.x} {transform.position.y} {transform.position.z}/" +
        $"{transform.eulerAngles.x} {transform.eulerAngles.y} {transform.eulerAngles.z}/" +
        $"{transform.localScale.x} {transform.localScale.y} {transform.localScale.z}";
        TempSave();
    }

    public virtual void TempSave()
    {
        if (saveKey != "")
        {
            GameManager.saveDic[saveKey] = eventFlow;
        }
    }

    public virtual void Load()
    {
        eventFlow = GameManager.saveDic[saveKey];
        if (eventFlow.Equals("false"))
        {
            if (dropWait != null)
            {
                StopCoroutine(dropWait);
            }
            gameObject.SetActive(false);
        }
        else
        {
            string[] transforms = _eventFlow.Split('/');
            string[] poses = transforms[0].Split(' ');
            string[] rotations = transforms[1].Split(' ');
            string[] scales = transforms[2].Split(' ');

            transform.position = new Vector3(float.Parse(poses[0]), float.Parse(poses[1]), float.Parse(poses[2]));
            transform.eulerAngles = new Vector3(float.Parse(rotations[0]), float.Parse(rotations[1]), float.Parse(rotations[2]));
            transform.localScale = new Vector3(float.Parse(scales[0]), float.Parse(scales[1]), float.Parse(scales[2]));
        }
    }

    public override void OnClicked()
    {
        if(GameManager.Instance.inventoryManager.TryGetRemainingTab(itemId, out TabScript tab))
        {
            if (tab.itemId != -1)
            {
                tab.itemCount++;
            }
            else
            {
                tab.itemId = itemId;
                tab.itemCount = 1;
            }
            base.OnDisHighlighted();
            GameManager.PlaySFX(GameManager.Instance.audioBox.object_pickup);
            GameManager.Instance.inventoryManager.SelectedItemRefresh();
            eventFlow = "false";
            if (itemPlacer != null)
            {
                const int MUFFIN_ID = 2;
                const int MILK_ID = 3;

                if (itemId == MUFFIN_ID)
                {
                    GameManager.Instance.muffinCountLeft++;
                }
                else if (itemId == MILK_ID)
                {
                    GameManager.Instance.milkCountLeft++;
                }
                itemPlacer.placeAbles[itemPlaceIndex] = true;
                itemPlacer = null;
            }
            if (dropWait != null)
            {
                StopCoroutine(dropWait);
            }
            gameObject.SetActive(false);
            GameManager.Instance.inventoryManager.TIP_ItemGotTipAppear(GameManager.Instance.itemData.infos[itemId].itemSprite);

            if(GameManager.Instance.isTutorial)
            {
                if(!GameManager.Instance.isEatenItem)
                {
                    GameManager.Instance.isEatenItem = true;
                    UIManager.TutorialPanel("획득한 아이템은 \"E\" 키로 인벤토리를 열어 확인할 수 있습니다.");
                }

                if (!GameManager.Instance.isUsedItem)
                {
                    if (GameManager.Instance.inventoryManager.TryMuffinAndMilkRemain())
                    {
                        GameManager.Instance.isUsedItem = true;
                        UIManager.TutorialPanel("일부 아이템은 \"F\"키를 눌러서 상호작용이 가능합니다.");
                        UIManager.TutorialPanel("<size=65>인벤토리를 열어서 아이템을 클릭하여 선택하고, 특정 상황에서 F키를 눌러 사용할 수 있습니다.</size>");
                    }
                }

                if(!GameManager.Instance.isInventoryFull)
                {
                    if (!GameManager.Instance.inventoryManager.TryGetRemainingTab(-1, out TabScript tab2))
                    {
                        GameManager.Instance.isInventoryFull = true;
                        UIManager.TutorialPanel("인벤토리가 꽉차면 더 이상 아이템을 먹을 수 없습니다.");
                        UIManager.TutorialPanel("아이템을 선택하고 \"Q\"키를 눌러서 잠시 버려둘 수 있습니다.");
                    }
                }
            }

        }
        else
        {
            // 인벤토리 꽉참
            GameManager.Instance.inventoryManager.TIP_FullInventory();
        }
    }
}
