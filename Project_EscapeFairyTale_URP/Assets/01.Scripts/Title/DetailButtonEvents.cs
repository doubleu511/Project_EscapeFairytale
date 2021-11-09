using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DetailButtonEvents : MonoBehaviour, IPointerEnterHandler
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
        if (detail != null)
        {
            myBtn.onClick.AddListener(() =>
            {
                detailTitleText.text = title;
                foreach (GameObject item in TitleManager.Instance.moreDetailPanels)
                {
                    if (item.activeInHierarchy)
                    {
                        item.SetActive(false);
                    }
                }
                detail.SetActive(true);
            });
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TitleManager.PlaySFX(TitleManager.Instance.audioBox.ui_tapSound);
    }
}
