using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
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
}
