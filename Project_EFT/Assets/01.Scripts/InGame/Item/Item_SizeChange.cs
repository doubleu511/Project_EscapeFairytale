using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item_SizeChange : MonoBehaviour
{
    public static int sizeValueRaw = 0;
    public float currentSize = 1;
    private float[] sizeScaleValues = new float[3] { 0.033f, 1f, 5f };

    public void OnUseBig()
    {
        if (GameManager.Instance.player.isSubCam) return;

        if (sizeValueRaw < 1)
        {
            sizeValueRaw++;
            currentSize = sizeScaleValues[sizeValueRaw + 1];
            UIManager.Tip_SizeChange(sizeValueRaw + 1);

            GameManager.Instance.player.transform.DOScale(currentSize, 2).SetUpdate(UpdateType.Fixed);
            GameManager.Instance.inventoryManager.SetNullTab(GameManager.Instance.selectedTab.tabId);
        }
        else
        {
            UIManager.Tip_RBAppear(null, "먹어도 효과가 없을 것 같습니다..", 0.5f, 3, 2);
        }
    }

    public void OnUseSmall()
    {
        if (GameManager.Instance.player.isSubCam) return;

        if (sizeValueRaw > -1)
        {
            sizeValueRaw--;
            currentSize = sizeScaleValues[sizeValueRaw + 1];
            UIManager.Tip_SizeChange(sizeValueRaw + 1);

            GameManager.Instance.player.transform.DOScale(currentSize, 2).SetUpdate(UpdateType.Fixed);
            GameManager.Instance.inventoryManager.SetNullTab(GameManager.Instance.selectedTab.tabId);
        }
        else
        {
            UIManager.Tip_RBAppear(null, "먹어도 효과가 없을 것 같습니다..", 0.5f, 3, 2);
        }
    }
}
