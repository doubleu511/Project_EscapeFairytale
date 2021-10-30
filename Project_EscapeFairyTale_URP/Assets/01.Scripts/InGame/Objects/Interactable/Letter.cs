using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : SelectableObject
{
    public Sprite letterSprite;

    [TextArea(5, 20)]
    public string letterTxt;

    public override void OnDisHighlighted()
    {
        base.OnDisHighlighted();
        UIManager.LetterUIClose();
    }

    public override void OnClicked()
    {
        if (letterTxt == "")
        {
            UIManager.LetterUI(letterSprite);
        }
        else
        {
            UIManager.LetterUI(letterSprite, letterTxt);
        }
    }
}
