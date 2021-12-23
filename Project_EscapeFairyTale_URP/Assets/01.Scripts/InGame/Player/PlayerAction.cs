using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    // 플레이어 카메라에 레이 캐스트를 쏘고, 오브젝트에 따라 함수를 호출한다.

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

        if (!GameManager.Instance.player.isSubCam) // 서브 카메라가 아니라면
        {
            float rayScale = 1;
            if(transform.localScale.x > 2.5f)
            {
                rayScale = 5;
            }
            else if (transform.localScale.x < 0.5f)
            {
                rayScale = 0.5f;
            } // 플레이어의 스케일에 따라 광선의 크기를 조절한다.
            isHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2 * rayScale, hitAbleLayer);
        }
        else
        { // 서브 카메라라면
            ray = UIManager.instance.subCamera.ScreenPointToRay(Input.mousePosition);
            // ScreenPointToRay를 이용하여 마우스를 기준으로 레이를 직선으로 쏜다.
            isHit = Physics.Raycast(ray, out hit, 10, subCam_detailLayer);
            UIManager.instance.cursorBtTipText.transform.position = UIManager.instance.subCamera.WorldToScreenPoint(ray.origin) - new Vector3(0, 50f, 0);
        }

        if (isHit) // 광선이 맞았고, Tag가 SelectableObject라면
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
                            currentObj.GetComponent<SelectableObject>().OnDisHighlighted(); // OnDisHighlighted, OnHighlighted를 호출해준다.
                        }
                        obj.OnHighlighted(obj.selectText);
                        currentObj = hit.collider.gameObject; // 또한 currentObj를 저장하여 PlayerInput에서 클릭했을 때 저장된 오브젝트를 OnClicked를 실행해주는 역할을 한다.
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