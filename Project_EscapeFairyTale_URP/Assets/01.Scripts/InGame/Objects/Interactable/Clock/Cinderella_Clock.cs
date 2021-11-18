using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinderella_Clock : SelectableObject, ISaveAble
{
    public static int hour;
    public static int minute;
    public GameObject minute_needle;
    public Vector3 teleportPos;
    public Vector3 teleportRot;

    public bool isHappyEnding = false;

    [Header("Save")]
    public string saveKey;
    private string _eventFlow = "nothave";
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
    } // have이면 있는것, nothave면 없는것 

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

    public void Load()
    {
        eventFlow = GameManager.saveDic[saveKey];
        if (eventFlow.Equals("have"))
        {
            OnNeedlePlace(true);
        }
    }

    public override void OnClicked()
    {
        UIManager.ChangeToSubCamera(new Vector3(teleportPos.x, teleportPos.y, teleportPos.z), Quaternion.Euler(teleportRot.x, teleportRot.y, teleportRot.z));
        UIManager.instance.currentShowObject = this;
    }

    public void OnNeedlePlace(bool instant)
    {
        if(minute_needle != null)
        {
            if(!instant) GameManager.PlaySFX(GameManager.Instance.audioBox.object_clock_assembly);
            minute_needle.SetActive(true);
        }
    }

    public void TempSave()
    {
        if (saveKey != "")
        {
            GameManager.saveDic[saveKey] = eventFlow;
        }
    }
}
