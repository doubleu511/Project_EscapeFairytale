using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMe : MonoBehaviour
{
    public bool isMainTitleCamera;

    [SerializeField] private Vector3 rotate;
    private bool isTurn = false;

    private void Start()
    {
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
            transform.Rotate(rotate * Time.deltaTime);
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
