using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    // �÷��̾� ī�޶� ���� ĳ��Ʈ�� ���, ������Ʈ�� ���� �Լ��� ȣ���Ѵ�.

    public static GameObject currentObj = null;
    public static Vector3 hitPos;

    public LayerMask hitAbleLayer;
    public LayerMask subCam_detailLayer;
    RaycastHit hit;
    public static Ray ray;

    void Update()
    {
        bool isHit = false;

        if (GameManager.Instance.player.playerState == PlayerState.DEAD) return;

        if (!GameManager.Instance.player.isSubCam) // ���� ī�޶� �ƴ϶��
        {
            float rayScale = 1;
            if(transform.localScale.x > 2.5f)
            {
                rayScale = 5;
            }
            else if (transform.localScale.x < 0.5f)
            {
                rayScale = 0.5f;
            } // �÷��̾��� �����Ͽ� ���� ������ ũ�⸦ �����Ѵ�.
            isHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2 * rayScale, hitAbleLayer);
        }
        else
        { // ���� ī�޶���
            ray = UIManager.instance.subCamera.ScreenPointToRay(Input.mousePosition);
            // ScreenPointToRay�� �̿��Ͽ� ���콺�� �������� ���̸� �������� ���.
            isHit = Physics.Raycast(ray, out hit, 10, subCam_detailLayer);
            UIManager.instance.cursorBtTipText.transform.position = UIManager.instance.subCamera.WorldToScreenPoint(ray.origin) - new Vector3(0, 50f, 0);
        }

        if (isHit) // ������ �¾Ұ�, Tag�� SelectableObject���
        {
            if (hit.collider.CompareTag("SelectableObject"))//
            {
                SelectableObject obj = hit.collider.GetComponent<SelectableObject>();

                if ((!GameManager.Instance.player.isSubCam && !obj.ignoreRaycast) || (GameManager.Instance.player.isSubCam && !obj.ignoreRaycast_inSubCam))
                {
                    if (currentObj != obj.gameObject)
                    {
                        if (currentObj != null)
                        {
                            currentObj.GetComponent<SelectableObject>().OnDisHighlighted(); // OnDisHighlighted, OnHighlighted�� ȣ�����ش�.
                        }
                        obj.OnHighlighted(obj.selectText);
                        currentObj = hit.collider.gameObject; // ���� currentObj�� �����Ͽ� PlayerInput���� Ŭ������ �� ����� ������Ʈ�� OnClicked�� �������ִ� ������ �Ѵ�.
                    }
                }
                else
                {
                    obj.OnDisHighlighted();
                    currentObj = null;
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