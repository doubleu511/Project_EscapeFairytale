using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : PickableObject, ISaveAble
{
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
        UIManager.Tip_RBAppear(GameManager.Instance.spriteBox.UI_Book_Covers[itemId], "å�� ȹ���Ͽ����ϴ�.\n<color=\"#33d6ff\"><b>BŰ</b></color>�� ������ å�� ���� �� �ֽ��ϴ�.", 0.5f, 3, 2);
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
