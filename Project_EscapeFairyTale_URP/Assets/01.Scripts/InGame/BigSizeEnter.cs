using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSizeEnter : MonoBehaviour
{
    public static bool isMilkUsePossible = false;

    private void OnTriggerEnter(Collider other)
    {
        isMilkUsePossible = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isMilkUsePossible = false;
    }
}
