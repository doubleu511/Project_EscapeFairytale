using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSizeEnter : MonoBehaviour
{
    public bool isCantEatMilkPlace = false;

    public static bool isMilkUsePossible = false;
    public static bool CantEatMilkPlace = false;

    private void OnTriggerEnter(Collider other)
    {
        if(isCantEatMilkPlace)
        {
            CantEatMilkPlace = true;
        }
        else
        {
            isMilkUsePossible = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isCantEatMilkPlace)
        {
            CantEatMilkPlace = false;
        }
        else
        {
            isMilkUsePossible = false;
        }
    }
}
