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
        UIManager.ChangeToSubCamera(new Vector3(-0.401f, 2.035f, 7.12f), Quaternion.Euler(0, 336.248962f, 0));
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
