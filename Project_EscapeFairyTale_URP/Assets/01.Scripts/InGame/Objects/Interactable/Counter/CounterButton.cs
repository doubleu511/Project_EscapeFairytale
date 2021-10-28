using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CounterButtonType
{
    ADD,
    REMOVE,
    TRIGGER
}

public class CounterButton : SelectableObject
{
    public Counter parentCounter;
    public CounterButtonType buttonType;
    public int buttonId;

    public override void OnClicked()
    {
        switch(buttonType)
        {
            case CounterButtonType.ADD:
                parentCounter.Add();
                break;
            case CounterButtonType.REMOVE:
                parentCounter.Remove();
                break;
            case CounterButtonType.TRIGGER:
                parentCounter.Trigger(buttonId);
                break;
        }
    }
}
