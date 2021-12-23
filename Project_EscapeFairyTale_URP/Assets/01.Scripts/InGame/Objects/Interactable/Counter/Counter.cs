using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    // 자신이 인덱스를 임의의 방법으로 더하거나/빼서 저장하고, 모듈에 이를 저장시킨다.

    [SerializeField] protected int maxIndex;
    [HideInInspector] public int index = 0;
    [HideInInspector] public Module parentModule;
    public Text countText;

    public virtual void Add()
    {
        if (++index > maxIndex) index = 0;

        if (countText != null) countText.text = index.ToString();
        parentModule.AnswerCheck();
    }

    public virtual void Remove()
    {
        if (--index < 0) index = maxIndex;

        if (countText != null) countText.text = index.ToString();
        parentModule.AnswerCheck();
    }

    public virtual void Trigger(int buttonId)
    {
        parentModule.AnswerCheck();
    }

    public void Refresh()
    {
        if (countText != null) countText.text = index.ToString();
    }
}
