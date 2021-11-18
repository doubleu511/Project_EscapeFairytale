using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alice_Ladder : MonoBehaviour, ISaveAble
{
    public static int ladderLevel = 0;
    public GameObject[] ladderSegments;

    [Header("Save")]
    public string saveKey;
    private string _eventFlow = "notexist";
    public string eventFlow
    {
        get { return _eventFlow; }
        set
        {
            if (_eventFlow != value)
            {
                _eventFlow = value;
                TempSave();
            }
        }
    } // exist이면 있는것, notexist면 없는것 

    private void Start()
    {
        if (!saveKey.Equals(""))
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

    public void Ladder_Build()
    {
        ladderLevel++;
        ladderSegments[ladderLevel - 1].SetActive(true);
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
        if (eventFlow.Equals("exist"))
        {
            for (int i = 0; i < ladderSegments.Length; i++)
            {
                ladderLevel = 3;
                ladderSegments[i].SetActive(true);
            }
        }
    }
}
