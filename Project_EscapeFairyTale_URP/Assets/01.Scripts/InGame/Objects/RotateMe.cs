using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMe : MonoBehaviour
{
    public bool isMainTitleCamera;

    [SerializeField] private Vector3 rotate;
    [SerializeField] private int zScale = 1;
    private Vector3 defaultRotate;
    private bool isTurn = false;

    private void Start()
    {
        defaultRotate = transform.eulerAngles;

        if (isMainTitleCamera)
        {
            StartCoroutine(Turn());
        }
    }

    void Update()
    {
        if (isMainTitleCamera)
        {
            if (!isTurn)
            {
                transform.Rotate(new Vector3(rotate.x, rotate.y, 0) * Time.deltaTime);
            }
            else
            {
                transform.Rotate(new Vector3(-rotate.x, rotate.y, 0) * Time.deltaTime);
            }
        }
        else
        {
            transform.Rotate(new Vector3(rotate.x, rotate.y, rotate.z) * Time.deltaTime, Space.Self);
        }

        if(zScale == 0)
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, defaultRotate.z);
        }
    }

    IEnumerator Turn()
    {
        while(true)
        {
            yield return new WaitForSeconds(20f);
            isTurn = !isTurn;
        }
    }
}
