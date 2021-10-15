using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : PickableObject
{
    public override void OnClicked()
    {
        gameObject.SetActive(false);
        // 인벤토리에 추가
        UIManager.Tip_RBAppear(null, "책을 획득하였습니다.\n<b>인벤토리</b>에서 책을 읽을 수 있습니다.", 0.5f, 3, 2);
    }
}
