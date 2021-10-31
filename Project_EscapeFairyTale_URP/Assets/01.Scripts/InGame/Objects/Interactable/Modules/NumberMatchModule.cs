using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NumberMatchModule : Module
{
    public Counter[] numbers;
    public int[] answers;
    public UnityEvent correctEvent;

    private bool isTrigger = false;

    private void Start()
    {
        foreach(Counter item in numbers)
        {
            item.parentModule = this;
        }
    }

    public override void AnswerCheck()
    {
        if (isTrigger) return;

        for(int i = 0; i< numbers.Length; i++)
        {
            if (numbers[i].index != answers[i]) return;
        }

        correctEvent.Invoke();
        isTrigger = true;
    }
}
