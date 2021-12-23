using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : PickableObject, ISaveAble
{
    // 책은 아이템 아이디를 사용하되 책 고유의 아이디를 사용한다.

    protected override void Start()
    {
        if (!saveKey.Equals(""))
        {
            if (!GameManager.saveDic.ContainsKey(saveKey))
            {
                GameManager.saveDic.Add(saveKey, eventFlow);
            }
            else
            {
                Load();
            }
        }
    }

    public override void OnClicked()
    {
        base.OnDisHighlighted();
        eventFlow = "false";
        gameObject.SetActive(false);
        UIManager.Tip_RBAppear(GameManager.Instance.spriteBox.UI_Book_Covers[itemId], "책을 획득하였습니다.\n<color=\"#33d6ff\"><b>B키</b></color>를 눌러서 책을 읽을 수 있습니다.", 0.5f, 3, 2);
        GameManager.PlaySFX(GameManager.Instance.audioBox.object_book_pickup);
        UIManager.instance.bookUIs[itemId].NoteEnabled = true;
    }

    public override void TempSave()
    {
        if (saveKey != "")
        {
            GameManager.saveDic[saveKey] = eventFlow;
        }
    }

    public override void Load()
    {
        eventFlow = GameManager.saveDic[saveKey];
        if (eventFlow.Equals("false"))
        {
            if (dropWait != null)
            {
                StopCoroutine(dropWait);
            }
            gameObject.SetActive(false);
            UIManager.instance.bookUIs[itemId].NoteEnabled = true;
        }
    }
}
