using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEvent
{
    public static void MouseLock(bool value) // static ���� �Լ��� ����Ͽ� ���� ȣ���Ѵ�.
    { // ���콺�� ���ܼ� ������Ű�ų� ���̰� �Ѵ�.
        if (value)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
