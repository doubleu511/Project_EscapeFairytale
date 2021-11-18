using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item_SizeChange : MonoBehaviour
{
    public static int sizeValueRaw = 0;
    public static float currentSize = 1;
    public static int currentFOV = 70;

    private static float[] sizeScaleValues = new float[3] { 0.033f, 1f, 5f };
    private static int[] FOVValues = new int[3] { 50, 70, 70 };
    private static float[] stepOffsets = new float[3] { 0.05f, 0.25f, 0.25f };
    public static float[] dropItemSizeScale = new float[3] { 0.1f, 1f, 5f };

    public SelectableObject pot;

    public void OnUseBig()
    {
        if (GameManager.Instance.player.isSubCam)
        {
            if (UIManager.instance.currentShowObject == pot)
            {
                Pinocio_Tree.PourMilk();
                return;
            }
            else
            {
                return;
            }
        }

        if (sizeValueRaw < 1 && ((sizeValueRaw < 0 && !BigSizeEnter.CantEatMilkPlace) || BigSizeEnter.isMilkUsePossible))
        {
            sizeValueRaw++;
            currentSize = sizeScaleValues[sizeValueRaw + 1];
            currentFOV = FOVValues[sizeValueRaw + 1];
            GameManager.Instance.player.GetComponent<CharacterController>().stepOffset = stepOffsets[sizeValueRaw + 1];
            UIManager.Tip_SizeChange(sizeValueRaw + 1);

            GameManager.Instance.player.transform.DOScale(currentSize, 2).SetUpdate(UpdateType.Fixed);
            Camera.main.DOFieldOfView(currentFOV, 2).SetUpdate(UpdateType.Fixed);
            GameManager.Instance.inventoryManager.DecreaseTab(GameManager.Instance.selectedTab.tabId);

            GameManager.PlaySFX(GameManager.Instance.audioBox.player_eatMilk);
            GameManager.Instance.CreateMilkTest();
        }
        else
        {
            UIManager.Tip_RBAppear(GameManager.Instance.spriteBox.UI_CantEat_Milk, "�Ծ ȿ���� ���� �� �����ϴ�..", 0.5f, 3, 2);
        }
    }

    public void OnUseSmall()
    {
        if (GameManager.Instance.player.isSubCam) return;

        if (sizeValueRaw > -1)
        {
            sizeValueRaw--;
            currentSize = sizeScaleValues[sizeValueRaw + 1];
            currentFOV = FOVValues[sizeValueRaw + 1];
            GameManager.Instance.player.GetComponent<CharacterController>().stepOffset = stepOffsets[sizeValueRaw + 1];
            UIManager.Tip_SizeChange(sizeValueRaw + 1);

            GameManager.Instance.player.transform.DOScale(currentSize, 2).SetUpdate(UpdateType.Fixed);
            Camera.main.DOFieldOfView(currentFOV, 2).SetUpdate(UpdateType.Fixed);
            GameManager.Instance.CreateMuffinTest();
            GameManager.Instance.inventoryManager.DecreaseTab(GameManager.Instance.selectedTab.tabId);

            GameManager.PlaySFX(GameManager.Instance.audioBox.player_eatMuffin);
        }
        else
        {
            UIManager.Tip_RBAppear(GameManager.Instance.spriteBox.UI_CantEat_Muffin, "�Ծ ȿ���� ���� �� �����ϴ�..", 0.5f, 3, 2);
        }

        if (GameManager.Instance.isTutorial)
        {
            if (!GameManager.Instance.isSmalled)
            {
                if (sizeValueRaw == -1)
                {
                    GameManager.Instance.isSmalled = true;
                    UIManager.TutorialPanel("�۾��� ���¿����� \"Space\"Ű�� ���� ������ �� �ֽ��ϴ�.");
                }
            }
        }
    }

    public static void SetSizeInstant(int size)
    {
        sizeValueRaw = size;
        currentSize = sizeScaleValues[sizeValueRaw + 1];
        currentFOV = FOVValues[sizeValueRaw + 1];
        GameManager.Instance.player.GetComponent<CharacterController>().stepOffset = stepOffsets[sizeValueRaw + 1];
        UIManager.Tip_SizeChange(sizeValueRaw + 1);

        GameManager.Instance.player.transform.localScale = new Vector3(currentSize, currentSize, currentSize);
        Camera.main.fieldOfView = currentFOV;
    }
}
