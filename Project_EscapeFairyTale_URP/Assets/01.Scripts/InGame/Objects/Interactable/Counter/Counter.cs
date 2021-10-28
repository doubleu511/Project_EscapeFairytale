using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] protected int maxIndex;
    [HideInInspector] public int index = 0;

    public virtual void Add()
    {
        if (++index > maxIndex) index = 0;
    }

    public virtual void Remove()
    {
        if (--index < 0) index = maxIndex;
    }

    public virtual void Trigger(int buttonId)
    {

    }
}
