using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Drawer : SelectableObject_Parent
{
    private const string lockTxt = "����ִ� �� �����ϴ�.";
    private const string unlockTxt = "Ŭ���Ͽ� ������ ���ݽ��ϴ�.";

    private Vector3 defaultPos;
    private Vector3 openPos;
    private bool isOpen = false;

    public bool isLocked = true;

    [Tooltip("isLocked�� true�϶��� �ۼ��Ѵ�. ����� �����Ҷ� �ʿ��� ������ ���̵�. -1�̶�� �ٸ� �̺�Ʈ�� ���Ͽ� �����ؾ��Ѵ�.")]
    public int requireItemId = 0;
    public bool isItemBroke = false;


    protected override void Start()
    {
        base.Start();
        defaultPos = transform.localPosition;
        openPos = transform.localPosition + new Vector3(0, -0.15f, 0);

        selectText = isLocked ? lockTxt : unlockTxt;
    }

    public override void OnClicked()
    {
        if(!isLocked)
        {
            if (requireItemId != -1)
            {
                if (GameManager.Instance.selectedItemId == requireItemId)
                {
                    Unlock();
                    if (isItemBroke)
                    {
                        GameManager.Instance.inventoryManager.DecreaseTab(GameManager.Instance.selectedTab.tabId);
                    }
                }
            }

            if (isOpen)
            {
                transform.DOLocalMove(defaultPos, 1);
                isOpen = false;
            }
            else
            {
                transform.DOLocalMove(openPos, 1);
                isOpen = true;
            }
        }
    }

    public void Unlock()
    {
        isLocked = false;
        selectText = unlockTxt;
    }
}
