using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    public float upRotateSpeed;
    public float rightRotateSpeed;

    public Vector3 mPrevPos = Vector3.zero;
    public Vector3 mPosDelta = Vector3.zero;

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            mPosDelta = Input.mousePosition - mPrevPos;
            transform.Rotate(transform.up, -Vector3.Dot(mPosDelta, GameManager.Instance.player.transform.right) * Time.deltaTime * upRotateSpeed, Space.World);
            transform.Rotate(GameManager.Instance.player.transform.right, Vector3.Dot(mPosDelta, GameManager.Instance.player.transform.up) * Time.deltaTime * rightRotateSpeed, Space.World);
        }

        mPrevPos = Input.mousePosition;
    }
}