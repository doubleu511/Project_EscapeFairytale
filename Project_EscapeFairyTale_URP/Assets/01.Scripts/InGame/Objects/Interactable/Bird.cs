using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : SelectableObject
{
    // 새 - 해피 엔딩을 보고 난 이후 하게될 미니게임 오브젝트

    public override void OnClicked()
    {
        base.OnDisHighlighted();
        UIManager.instance.birdRemain++;
        GameManager.PlaySFX(GameManager.Instance.audioBox.object_pickup);
        UIManager.Tip_RBAppear(GameManager.Instance.spriteBox.UI_Bird, $"새를 획득하였습니다.\n<color=\"#33d6ff\"><b>{7 - UIManager.instance.birdRemain}</b></color>개 남았습니다!", 0.5f, 3, 2);
        gameObject.SetActive(false);
    }
}
