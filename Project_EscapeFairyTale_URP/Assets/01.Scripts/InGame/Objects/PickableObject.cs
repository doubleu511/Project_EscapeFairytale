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

    protected void Start()
    {
        if (saveKey != "")
        {
            if (!GameManager.saveDic.ContainsKey(saveKey))
            {
                GameManager.saveDic.Add(saveKey, eventFlow);
            }
            else
            {
                Load();
            }
        }
    }

    public GameObject Drop()
    {
        _eventFlow = "true";
        TempSave();
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * 2 * transform.localScale.x, ForceMode.Impulse);

        return this.gameObject;
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
        if (!bool.Parse(eventFlow))
        {
            gameObject.SetActive(false);
            Invoke("DestroyObj", 0.5f);
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
            GameManager.Instance.inventoryManager.SelectedItemRefresh();
            eventFlow = "false";
            if (itemPlacer != null)
            {
                itemPlacer.placeAbles[itemPlaceIndex] = true;
            }
            gameObject.SetActive(false);
            Invoke("DestroyObj", 0.5f);
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

    private void DestroyObj()
    {
        Destroy(this.gameObject);
    }
}
