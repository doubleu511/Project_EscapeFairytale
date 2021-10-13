using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item_SizeChange : MonoBehaviour
{
    public int sizeValueRaw = 0;
    public float currentSize = 1;
    private float[] sizeScaleValues = new float[3] { 0.1f, 1f, 5f };

    public void OnUseBig()
    {
        if (sizeValueRaw < 1)
        {
            Debug.Log("커질꺼에요");
            sizeValueRaw++;
            currentSize = sizeScaleValues[sizeValueRaw + 1];
            UIManager.Tip_SizeChange(sizeValueRaw + 1);
            GameManager.Instance.player.transform.DOScale(currentSize, 2);
            GameManager.Instance.inventoryManager.SetNullTab(GameManager.Instance.selectedTab.tabId);
        }
        else
        {
            UIManager.Tip_RBAppear(null, "먹어도 효과가 없을 것 같습니다..", 0.5f, 3, 2);
        }
    }

    public void OnUseSmall()
    {
        if (sizeValueRaw > -1)
        {
            Debug.Log("작아질꺼에요");
            sizeValueRaw--;
            currentSize = sizeScaleValues[sizeValueRaw + 1];
            UIManager.Tip_SizeChange(sizeValueRaw + 1);
            GameManager.Instance.player.transform.DOScale(currentSize, 2);
            GameManager.Instance.inventoryManager.SetNullTab(GameManager.Instance.selectedTab.tabId);
        }
        else
        {
            UIManager.Tip_RBAppear(null, "먹어도 효과가 없을 것 같습니다..", 0.5f, 3, 2);
        }
    }
}
