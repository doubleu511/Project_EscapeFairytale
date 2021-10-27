using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_MinuteNeedle : MonoBehaviour
{
    public void OnUse()
    {
        if(UIManager.instance.currentShowObject != null)
        {
            Cinderella_Clock clock = UIManager.instance.currentShowObject.GetComponent<Cinderella_Clock>();

            if(clock != null)
            {
                // ���� �´� �ð��� �־����
                clock.OnNeedlePlace();
                GameManager.Instance.inventoryManager.DecreaseTab(GameManager.Instance.selectedTab.tabId);
            }
        }
    }
}
