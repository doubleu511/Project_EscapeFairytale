using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIB_Coffer : MonoBehaviour
{
    private const string lockTxt = "����ִ� �� �����ϴ�.";

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
