using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_EmptyShoesBox : MonoBehaviour
{
    public LayerMask hitAbleLayer;
    RaycastHit hit;

    public void OnUse()
    {
        bool isHit = false;
        isHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2 * (transform.localScale.x > 2.5f ? 5 : 1), hitAbleLayer);

        if (isHit)
        {
            EnemyAI ai = hit.collider.GetComponent<EnemyAI>();
            if(ai != null)
            {
                ai.stunSec = 10;
                GameManager.Instance.inventoryManager.SetNullTab(GameManager.Instance.selectedTab.tabId);
            }
        }
    }
}
