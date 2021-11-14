using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cinderella_Clock_Button : SelectableObject
{
    private void Start()
    {
        GetComponent<Outline>().enabled = true;
        GameManager.Instance.clock_color_select = Color.white;
    }
}