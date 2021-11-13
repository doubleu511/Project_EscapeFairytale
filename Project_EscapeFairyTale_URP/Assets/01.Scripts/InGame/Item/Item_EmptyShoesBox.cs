using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item_EmptyShoesBox : MonoBehaviour
{
    public LayerMask hitAbleLayer;
    RaycastHit hit;

    public GameObject boxShoes_Effect;

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
                StartCoroutine(BoxShoesEffect());
                boxShoes_Effect.SetActive(true);
                boxShoes_Effect.transform.DOShakePosition(10, 0.05f, 20, 90, false, false);
                GameManager.Instance.inventoryManager.DecreaseTab(GameManager.Instance.selectedTab.tabId);
            }
        }
    }

    IEnumerator BoxShoesEffect()
    {
        yield return new WaitForSeconds(10);
        boxShoes_Effect.transform.DOKill();
        boxShoes_Effect.transform.localPosition = new Vector3(0, -0.012f, 0.093f);
        boxShoes_Effect.SetActive(false);
    }
}
