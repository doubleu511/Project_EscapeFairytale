using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropContainer : MonoBehaviour
{
    public int itemId;
    public Image image;
    // public MyData data;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }
}
