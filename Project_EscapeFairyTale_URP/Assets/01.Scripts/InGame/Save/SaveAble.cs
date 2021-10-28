using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 이제 앞으로, 저장해야할 흐름이 있다면 eventFlow를 더해주고, 훗날 0에서 바로 그 eventFlow로 바뀔때 또는 낮춰질 때
 * 적용되어야할 오브젝트의 상태를 바로바로 불러올수 있게 해줘야한다. (바뀔때 새로고침해서 switch로
 * 해당 Flow와 맞는 상태로 계속 바꿔주는게 최선일듯 싶다)
*/
public class SaveAble : MonoBehaviour
{
    public string saveKey;
    public int eventFlow = 0;

    public virtual void Save()
    {
        SecurityPlayerPrefs.SetInt(saveKey, eventFlow);
    }

    public virtual void Load()
    {
        eventFlow = SecurityPlayerPrefs.GetInt(saveKey, 0);
    }
}
