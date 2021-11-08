using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailButtonEvents : MonoBehaviour
{
    private Button myBtn;

    public Text detailTitleText;
    public GameObject detail;
    public string title;

    private void Awake()
    {
        myBtn = GetComponent<Button>();
    }

    void Start()
    {
        myBtn.onClick.AddListener(() =>
        {
            detailTitleText.text = title;
            foreach(GameObject item in TitleManager.Instance.moreDetailPanels)
            {
                item.SetActive(false);
            }
            detail.SetActive(true);
        }); 
    }
}
