using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinderella_Bell : SelectableObject
{
    private int count = 0;

    public override void OnClicked()
    {
        //SFX Àç»ý

        if(Cinderella_Clock_Button.isCleared)
        {
            count++;
        }
    }
}
