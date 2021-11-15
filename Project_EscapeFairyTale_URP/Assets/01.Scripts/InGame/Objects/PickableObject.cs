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
    private string _eventFlow = "true";
    public string eventFlow { get { return _eventFlow; }
        set {
            if (_eventFlow != value)
            {
                _eventFlow = value;
                TempSave();
            }
        }
    } // true이면 오브젝트가 켜진것, false이면 꺼진것 

    private Coroutine dropWait;

    protected void Start()
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

    public GameObject ObjectOn(Vector3 pos, Quaternion rotation)
    {
        this.gameObject.transform.position = pos;
        this.gameObject.transform.rotation = rotation;
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
                itemPlacer.placeAbles[itemPlaceIndex] = true;
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
            }

        }
        else
        {
            // 인벤토리 꽉참
            GameManager.Instance.inventoryManager.TIP_FullInventory();
        }
    }
}
