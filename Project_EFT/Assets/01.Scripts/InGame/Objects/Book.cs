using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : PickableObject
{
    public override void OnClicked()
    {
        gameObject.SetActive(false);
        UIManager.Tip_RBAppear(null, "책을 획득하였습니다.\n<color=\"#33d6ff\"><b>B키</b></color>를 눌러서 책을 읽을 수 있습니다.", 0.5f, 3, 2);
        GameManager.PlaySFX(GameManager.Instance.audioBox.object_book_pickup);
        UIManager.instance.bookUIs[itemId].NoteEnabled = true;
    }
}
