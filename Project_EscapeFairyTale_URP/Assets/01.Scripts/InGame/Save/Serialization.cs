using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Serialization<TKey, TValue> : ISerializationCallbackReceiver // 이 인터페이스가 구현되면 JsonUtility에 넣을 수 있는 것 같다.
{
    // 제네릭으로 형식을 받아서 리스트로 만들어서 직렬화 해준다.

    [SerializeField]
    List<TKey> keys;
    [SerializeField]
    List<TValue> values;

    Dictionary<TKey, TValue> target;
    public Dictionary<TKey, TValue> ToDictionary() { return target; }

    public Serialization(Dictionary<TKey, TValue> target)
    {
        this.target = target;
    }

    public void OnBeforeSerialize()
    {
        keys = new List<TKey>(target.Keys);
        values = new List<TValue>(target.Values);
    }

    public void OnAfterDeserialize()
    {
        var count = Math.Min(keys.Count, values.Count);
        target = new Dictionary<TKey, TValue>(count);
        for (var i = 0; i < count; ++i)
        {
            target.Add(keys[i], values[i]);
        }
    }
}
