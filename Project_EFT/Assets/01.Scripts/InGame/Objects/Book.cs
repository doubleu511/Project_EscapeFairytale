using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : PickableObject
{
    public override void OnClicked()
    {
        gameObject.SetActive(false);
        UIManager.Tip_RBAppear(null, "å�� ȹ���Ͽ����ϴ�.\n<color=\"#33d6ff\"><b>BŰ</b></color>�� ������ å�� ���� �� �ֽ��ϴ�.", 0.5f, 3, 2);
        GameManager.PlaySFX(GameManager.Instance.audioBox.object_book_pickup);
        UIManager.instance.bookUIs[itemId].NoteEnabled = true;
    }
}
