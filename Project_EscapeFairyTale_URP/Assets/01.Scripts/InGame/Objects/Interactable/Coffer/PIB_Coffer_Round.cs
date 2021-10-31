using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIB_Coffer_Round : SelectableObject
{
    private bool isHighlighted = false; // true면 마우스가 분침에 간거다.
    private bool dragStart = false;

    private Vector3 beforePos;
    private Vector3 nowPos;
    private Vector3 centerPos = new Vector3(960, 540);

    public Transform round_dummy;

    public override void OnHighlighted(string text)
    {
        base.OnHighlighted(text);
        isHighlighted = true;
    }

    public override void OnDisHighlighted()
    {
        base.OnDisHighlighted();
        isHighlighted = false;
    }

    public override void OnClicked()
    {
        centerPos = UIManager.instance.subCamera.WorldToScreenPoint(transform.position);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isHighlighted)
            {
                dragStart = true;
                beforePos = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragStart = false;
        }

        if (dragStart)
        {
            nowPos = Input.mousePosition;

            Vector3 currentDir = beforePos - centerPos;
            Vector3 mouseDir = nowPos - centerPos;

            float angle = Vector2.SignedAngle(mouseDir, currentDir);

            round_dummy.transform.localRotation *= Quaternion.Euler(0, angle, 0);
            beforePos = nowPos;

            // 각도 딱 나누어 떨어지게 하기.
            int intAngle = Mathf.FloorToInt(-round_dummy.localRotation.eulerAngles.y + 360);

            int angleRemainder = intAngle % 5;

            intAngle = (intAngle / 5) * 5 + (angleRemainder > 2 ? 5 : 0);

            transform.localRotation = Quaternion.Euler(0, intAngle, 0);
        }
    }
}
