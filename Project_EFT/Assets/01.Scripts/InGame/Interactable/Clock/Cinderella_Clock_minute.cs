using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinderella_Clock_minute : SelectableObject
{
    private bool isHighlighted = false; // true면 마우스가 분침에 간거다.
    private bool dragStart = false;

    private Vector3 beforePos;
    private Vector3 nowPos;
    private Vector3 centerPos = new Vector3(960, 540);

    public Transform hour_clock;
    public Transform hour_minute_dummy;

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

    }

    private void Start()
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

            hour_minute_dummy.transform.localRotation *= Quaternion.Euler(0, 0, angle);
            hour_clock.localRotation *= Quaternion.Euler(0, 0, angle / 12);
            beforePos = nowPos;

            // 각도 딱 나누어 떨어지게 하기.
            int intAngle_minute = Mathf.RoundToInt(-hour_minute_dummy.localRotation.eulerAngles.y + 360);
            int intAngle_hour = Mathf.RoundToInt(-hour_clock.localRotation.eulerAngles.y + 360);

            int angleRemainder = intAngle_minute % 6;

            intAngle_minute = (intAngle_minute / 6) * 6 + (angleRemainder > 2 ? 6 : 0);
            if (intAngle_minute == 360) intAngle_minute = 0;

            Cinderella_Clock.minute = intAngle_minute / 6;
            Cinderella_Clock.hour = (intAngle_hour / 30 == 0) ? 12 : intAngle_hour / 30;

            Debug.Log(intAngle_hour + ":" + intAngle_minute);
            Debug.Log(Cinderella_Clock.hour + ":" + Cinderella_Clock.minute);

            transform.localRotation = Quaternion.Euler(90, 0, intAngle_minute);
        }
    }
}
