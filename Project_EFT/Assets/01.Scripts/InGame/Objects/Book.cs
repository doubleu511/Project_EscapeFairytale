using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : PickableObject
{
    public override void OnClicked()
    {
        gameObject.SetActive(false);
        // �κ��丮�� �߰�
        UIManager.Tip_RBAppear(null, "å�� ȹ���Ͽ����ϴ�.\n<b>�κ��丮</b>���� å�� ���� �� �ֽ��ϴ�.", 0.5f, 3, 2);
    }
}
