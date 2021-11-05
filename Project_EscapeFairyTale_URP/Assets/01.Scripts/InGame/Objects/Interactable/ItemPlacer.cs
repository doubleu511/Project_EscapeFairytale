using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacer : SelectableObject, ISaveAble
{
    private const string placeText = "클릭하여 선택한 아이템을 놓습니다.";
    private const string cantPlaceText = "아이템이 꽉 찼습니다.";

    public int[] placeAbleIds;
    public Transform[] placeAblePoses;

    [Header("Save")]
    public string saveKey;

    private string eventFlow = "";
     // 이 놈은 placeAblePoses 만큼 스페이스 된 아이디를 불러와서 스폰시킨다.

    public bool[] placeAbles;
    public int[] placedId;

    protected override void Awake()
    {
        base.Awake();
        placeAbles = new bool[placeAblePoses.Length];
        placedId = new int[placeAblePoses.Length];

        for(int i = 0; i<placeAblePoses.Length;i++)
        {
            placeAbles[i] = true;
            placedId[i] = -1;
        }
    }

    private void Start()
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

    public override void OnHighlighted(string text)
    {
        for (int i = 0; i < placeAbleIds.Length; i++)
        {
            if (GameManager.Instance.selectedItemId == placeAbleIds[i])
            {
                base.OnHighlighted(placeText);
                return;
            }
        }
    }

    public override void OnClicked()
    {
        for (int i = 0; i < placeAbleIds.Length; i++)
        {
            if (GameManager.Instance.selectedItemId == placeAbleIds[i])
            {
                if(!PosSetTry())
                {
                    UIManager.instance.cursorBtTipText.text = cantPlaceText;
                }
            }
        }
    }

    private bool PosSetTry()
    {
        for(int i = 0; i< placeAbles.Length;i++)
        {
            if(placeAbles[i])
            {
                PickableObject obj = Instantiate(GameManager.Instance.itemData.infos[GameManager.Instance.selectedItemId].itemPrefab, placeAblePoses[i].position, transform.rotation)
                    .GetComponent<PickableObject>().Drop().GetComponent<PickableObject>();
                placedId[i] = GameManager.Instance.selectedItemId;
                Destroy(obj.GetComponent<Rigidbody>());
                placeAbles[i] = false;
                obj.itemPlacer = this;
                obj.itemPlaceIndex = i;
                TempSave();
                GameManager.Instance.inventoryManager.DecreaseTab(GameManager.Instance.selectedTab.tabId);
                return true;
            }
        }
        return false;
    }

    public void TempSave()
    {
        if (saveKey != "")
        {
            eventFlow = "";

            for (int i = 0; i < placeAblePoses.Length; i++)
            {
                eventFlow += placedId[i].ToString();

                if(i < placeAblePoses.Length -1)
                {
                    eventFlow += ' ';
                }
            }

            GameManager.saveDic[saveKey] = eventFlow;
        }
    }

    public void Load()
    {
        eventFlow = GameManager.saveDic[saveKey];
        string[] data = eventFlow.Split(' ');
        for (int i = 0; i < data.Length; i++)
        {
            int id = int.Parse(data[i]);

            placeAbles[i] = id == -1;
            placedId[i] = id;

            if(!placeAbles[i])
            {
                PickableObject obj = Instantiate(GameManager.Instance.itemData.infos[placedId[i]].itemPrefab, placeAblePoses[i].position, transform.rotation)
                .GetComponent<PickableObject>().Drop().GetComponent<PickableObject>();
                Destroy(obj.GetComponent<Rigidbody>());
                obj.itemPlacer = this;
                obj.itemPlaceIndex = i;
            }
        }
    }
}