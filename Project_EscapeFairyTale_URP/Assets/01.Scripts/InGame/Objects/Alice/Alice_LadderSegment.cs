using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alice_LadderSegment : PickableObject, ISaveAble
{
    public Alice_Ladder ladder;

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

        ladder.Ladder_Build();
        int remainWood = 3 - Alice_Ladder.ladderLevel;
        if (remainWood > 0)
        {
            UIManager.Tip_RBAppear(GameManager.Instance.spriteBox.UI_WoodSegment, $"���������� ȹ���Ͽ����ϴ�.\n<color=\"#eb4034\"><b>{remainWood}��</b></color>�� �� ��Ƽ� ��ٸ��� �ϼ��ϼ���!", 0.5f, 3, 2);
        }
        else
        {
            UIManager.Tip_RBAppear(GameManager.Instance.spriteBox.UI_WoodSegment, $"���������� ��� ȹ���Ͽ����ϴ�.\n<color=\"#eb4034\"><b>��ٸ�</b></color>�� �ϼ��Ǿ����ϴ�.", 0.5f, 3, 2);
        }
        GameManager.PlaySFX(GameManager.Instance.audioBox.object_pickup);
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
        }
    }
}