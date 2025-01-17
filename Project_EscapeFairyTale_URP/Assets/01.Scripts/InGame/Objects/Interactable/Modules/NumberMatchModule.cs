using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NumberMatchModule : Module
{
    // 숫자가 같은지 확인하여 맞다면 정답 이벤트를 호출한다.

    public Counter[] numbers;
    public int[] answers;
    public UnityEvent correctEvent;
    public UnityEvent loadEvent;

    private bool isTrigger = false;

    public override void Start()
    {
        base.Start();
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

        eventFlow = "unlock";
        correctEvent.Invoke();
        isTrigger = true;
    }

    public override void Load()
    {
        eventFlow = GameManager.saveDic[saveKey];
        if (eventFlow.Equals("unlock"))
        {
            loadEvent.Invoke();

            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i].index = answers[i];
                numbers[i].Refresh();
            }

            isTrigger = true;
        }
    }
}
