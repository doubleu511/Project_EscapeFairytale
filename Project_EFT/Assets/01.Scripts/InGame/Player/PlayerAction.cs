using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    RaycastHit hit;

    void Update()
    {
        bool isHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10, 1 << LayerMask.NameToLayer("Default"));
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10);

        if (isHit)
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }
}
