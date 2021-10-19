using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : SelectableObject
{
    public Sprite letterSprite;

    [TextArea(5, 10)]
    public string letterTxt;

    public override void OnDisHighlighted()
    {
        base.OnDisHighlighted();
        UIManager.instance.letterCanvasGroup.alpha = 0;
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
