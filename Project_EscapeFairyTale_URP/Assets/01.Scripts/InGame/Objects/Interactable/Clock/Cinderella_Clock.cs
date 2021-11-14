using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinderella_Clock : SelectableObject
{
    public static int hour;
    public static int minute;
    public GameObject minute_needle;
    public Vector3 teleportPos;
    public Vector3 teleportRot;

    public override void OnClicked()
    {
        UIManager.ChangeToSubCamera(new Vector3(teleportPos.x, teleportPos.y, teleportPos.z), Quaternion.Euler(teleportRot.x, teleportRot.y, teleportRot.z));
        UIManager.instance.currentShowObject = this;
    }

    public void OnNeedlePlace()
    {
        if(minute_needle != null)
        {
            GameManager.PlaySFX(GameManager.Instance.audioBox.object_clock_assembly);
            minute_needle.SetActive(true);
        }
    }
}
