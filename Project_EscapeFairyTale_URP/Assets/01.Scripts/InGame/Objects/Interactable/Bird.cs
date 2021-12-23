using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : SelectableObject
{
    // �� - ���� ������ ���� �� ���� �ϰԵ� �̴ϰ��� ������Ʈ

    public override void OnClicked()
    {
        base.OnDisHighlighted();
        UIManager.instance.birdRemain++;
        GameManager.PlaySFX(GameManager.Instance.audioBox.object_pickup);
        UIManager.Tip_RBAppear(GameManager.Instance.spriteBox.UI_Bird, $"���� ȹ���Ͽ����ϴ�.\n<color=\"#33d6ff\"><b>{7 - UIManager.instance.birdRemain}</b></color>�� ���ҽ��ϴ�!", 0.5f, 3, 2);
        gameObject.SetActive(false);
    }
}
