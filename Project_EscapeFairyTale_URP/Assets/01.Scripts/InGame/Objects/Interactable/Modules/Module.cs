using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour, ISaveAble
{
    // 퍼즐 모듈의 최상위 클래스
    // 모든 하위 클래스에서 쓰일 최소한의 변수/함수 입력


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
