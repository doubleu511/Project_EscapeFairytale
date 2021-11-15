using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinocio_Pot : SelectableObject
{
    public override void OnClicked()
    {
        UIManager.CanvasGroup_DefaultShow(UIManager.instance.pinocio_tree, true);
        UIManager.ChangeToSubCamera(new Vector3(0,0,0), transform.rotation, false);
        UIManager.instance.currentShowObject = this;
    }
}