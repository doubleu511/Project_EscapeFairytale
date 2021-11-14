using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinderella_Clock_minute : SelectableObject
{
    private bool isHighlighted = false; // true면 마우스가 분침에 간거다.
    private bool dragStart = false;
    private int beforeMinute = 0;

    private Vector3 beforePos;
    private Vector3 nowPos;
    private Vector3 centerPos = new Vector3(960, 540);

    public GameObject minuteParent;
    public Transform hour_clock;
    public Transform hour_minute_dummy;

    public bool trueClock = false;

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

        if(Input.GetMouseButtonDown(1))
        {
            if (isHighlighted)
            {
                if (GameManager.Instance.inventoryManager.TryGetRemainingTab(0, out TabScript tab))
                {
                    if (tab.itemId != -1)
                    {
                        tab.itemCount++;
                    }
                    else
                    {
                        tab.itemId = 0;
                        tab.itemCount = 1;
                    }
                    GameManager.Instance.inventoryManager.SelectedItemRefresh();
                    GameManager.Instance.inventoryManager.TIP_ItemGotTipAppear(GameManager.Instance.itemData.infos[0].itemSprite);
                    GameManager.PlaySFX(GameManager.Instance.audioBox.object_clock_assembly);

                    base.OnDisHighlighted();
                    isHighlighted = false;
                    dragStart = false;
                    minuteParent.SetActive(false);
                }
                else
                {
                    // 인벤토리 꽉참
                    GameManager.Instance.inventoryManager.TIP_FullInventory();
                }
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
            int intAngle_minute = Mathf.FloorToInt(-hour_minute_dummy.localRotation.eulerAngles.y + 360);
            int intAngle_hour = Mathf.FloorToInt(-hour_clock.localRotation.eulerAngles.y + 360);

            int angleRemainder = intAngle_minute % 6;

            intAngle_minute = (intAngle_minute / 6) * 6 + (angleRemainder > 2 ? 6 : 0);
            if (intAngle_minute == 360) intAngle_minute = 0;

            if (trueClock)
            {
                if (Cinderella_Clock.minute != intAngle_minute / 6)
                {
                    Cinderella_Clock.minute = intAngle_minute / 6;
                    Cinderella_Clock.hour = (intAngle_hour / 30 == 0) ? 12 : intAngle_hour / 30;

                    if(!GameManager.Instance.defaultSFXSource.isPlaying)
                    GameManager.PlaySFX(GameManager.Instance.audioBox.object_coffer_tick);
                }
            }
            else
            {
                if(beforeMinute != intAngle_minute / 6)
                {
                    if (!GameManager.Instance.defaultSFXSource.isPlaying)
                        GameManager.PlaySFX(GameManager.Instance.audioBox.object_coffer_tick);
                }
            }

            beforeMinute = intAngle_minute / 6;
            transform.localRotation = Quaternion.Euler(90, 0, intAngle_minute);
        }
    }

    public void SetClockValue(int hour, int minute)
    {
        StartCoroutine(SetClock(hour, minute));
    }

    IEnumerator SetClock(int hour, int minute)
    {
        ignoreRaycast_inSubCam = true;

        while (true)
        {
            yield return null;
            transform.localRotation *= Quaternion.Euler(0, 0, -6f);
            hour_clock.localRotation *= Quaternion.Euler(0, 0, -6f / 12);

            int intAngle_minute = Mathf.FloorToInt(-transform.localRotation.eulerAngles.y + 360);
            int intAngle_hour = Mathf.RoundToInt(-hour_clock.localRotation.eulerAngles.y + 360);

            if (intAngle_minute == 360) intAngle_minute = 0;

            int current_minute = intAngle_minute / 6;
            int current_hour = (intAngle_hour / 30 == 0) ? 12 : intAngle_hour / 30;

            if(current_hour == hour && current_minute == minute)
            {
                break;
            }
        }

        Cinderella_Clock.hour = hour;
        Cinderella_Clock.minute = minute;
        hour_minute_dummy.localRotation = transform.localRotation;
        ignoreRaycast_inSubCam = false;
    }
}
