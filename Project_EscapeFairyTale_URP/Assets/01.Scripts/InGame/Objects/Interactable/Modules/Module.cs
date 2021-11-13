using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour, ISaveAble
{
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
    } // unlock이면 언락, lock이면 락

    public virtual void Start()
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

    public virtual void TempSave()
    {
        if (saveKey != "")
        {
            GameManager.saveDic[saveKey] = eventFlow;
        }
    }

    public virtual void Load()
    {

    }

    public virtual void AnswerCheck()
    {
        
    }
}
