using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public static GameObject currentObj = null;
    RaycastHit hit;

    void Update()
    {
        bool isHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3, 1 << LayerMask.NameToLayer("Item"));
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 3);

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
}