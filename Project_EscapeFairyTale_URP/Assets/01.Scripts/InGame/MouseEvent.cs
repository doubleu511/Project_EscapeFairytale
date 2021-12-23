using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEvent
{
    public static void MouseLock(bool value) // static 정적 함수를 사용하여 쉽게 호출한다.
    { // 마우스를 숨겨서 고정시키거나 보이게 한다.
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
