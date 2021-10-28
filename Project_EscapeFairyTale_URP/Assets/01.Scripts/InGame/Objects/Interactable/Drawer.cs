using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Drawer : SelectableObject_Parent
{
    private const string lockTxt = "잠겨있는 것 같습니다.";
    private const string unlockTxt = "클릭하여 서랍을 여닫습니다.";

    private Vector3 defaultPos;
    private Vector3 openPos;
    private bool isOpen = false;

    public bool isLocked = true;

    [Tooltip("isLocked가 true일때만 작성한다. 잠금을 해제할때 필요한 아이템 아이디. -1이라면 다른 이벤트를 통하여 해제해야한다.")]
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
