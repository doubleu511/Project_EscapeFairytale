using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinderella_Clock : SelectableObject_Parent
{
    public static int hour;
    public static int minute;
    public GameObject minute_needle;

    public override void OnClicked()
    {
        UIManager.ChangeToSubCamera(new Vector3(-5.324f, 2.156f, 9.178f), Quaternion.Euler(0, 0, 0));
        OnParentDisable();
        UIManager.instance.currentShowObject = this;
    }

    public void OnNeedlePlace()
    {
        if(minute_needle != null)
        {
            minute_needle.SetActive(true);
        }
    }
}
