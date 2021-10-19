using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public static GameObject currentObj = null;

    public LayerMask hitAbleLayer;
    RaycastHit hit;
    Ray ray;

    void Update()
    {
        bool isHit = false;
        if (!GameManager.Instance.player.isSubCam)
        {
            isHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2 * (transform.localScale.x > 2.5f ? 5 : 1), hitAbleLayer);
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 3);
        }
        else
        {
            ray = UIManager.instance.subCamera.ScreenPointToRay(Input.mousePosition);
            
            isHit = Physics.Raycast(ray, out hit, 10, hitAbleLayer);
            UIManager.instance.cursorBtTipText.transform.position = UIManager.instance.subCamera.WorldToScreenPoint(ray.origin) - new Vector3(0, 50f, 0);
        }

        if (isHit)
        {
            if (hit.collider.CompareTag("SelectableObject"))
            {
                SelectableObject obj = hit.collider.GetComponent<SelectableObject>();
                obj.OnHighlighted(obj.selectText);

                if (currentObj != hit.collider.gameObject)
                {
                    if (currentObj != null)
                    {
                        currentObj.GetComponent<SelectableObject>().OnDisHighlighted();
                    }
                    currentObj = hit.collider.gameObject;
                }
            }
            else
            {
                if (currentObj != null)
                {
                    currentObj.GetComponent<SelectableObject>().OnDisHighlighted();
                    currentObj = null;
                }
            }
        }
        else
        {
            if (currentObj != null)
            {
                currentObj.GetComponent<SelectableObject>().OnDisHighlighted();
                currentObj = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(ray);
    }
}