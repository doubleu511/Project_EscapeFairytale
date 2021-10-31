using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIB_Coffer : MonoBehaviour
{
    private const string lockTxt = "잠겨있는 것 같습니다.";

    private bool isOpen = false;

    public AudioSource audioSource;
    public bool isLocked = true;

    public void Unlock()
    {
        isLocked = false;
    }

    public void Open()
    {

    }
}
