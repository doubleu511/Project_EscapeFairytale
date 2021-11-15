using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HeartBox : SelectableObject, ISaveAble
{
    public GameObject heartBox_cover;
    public Vector3 endPos;

    [Header("Save")]
    public string saveKey;
    private string _eventFlow = "lock";
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
    } // lock이면 잠긴것, unlock이면 풀린것 

    private bool isTrigger = false;

    protected void Start()
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

    public override void OnClicked()
    {
        if (isTrigger) return;

        const int HEART_ID = 4;

        if(GameManager.Instance.selectedItemId == HEART_ID)
        {
            GameManager.Instance.inventoryManager.DecreaseTab(GameManager.Instance.selectedTab.tabId);
            UnLock(false);
        }
    }

    public void UnLock(bool instant)
    {
        isTrigger = true;
        if (!instant)
        {
            heartBox_cover.transform.DOMove(endPos, 1).SetRelative();
            GameManager.PlaySFX(GameManager.Instance.audioBox.object_drawer_open);
            eventFlow = "unlock";
        }
        else
        {
            heartBox_cover.transform.DOMove(endPos, 0).SetRelative();
        }
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
        if (eventFlow.Equals("unlock"))
        {
            UnLock(true);
        }
    }
}
