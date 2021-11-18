using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pinocio_Key : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject plant;
    public static GameObject _plant;

    private UnityEngine.UI.Outline[] keyOutlines;
    private bool isHighlighted = false;

    private void Awake()
    {
        _plant = plant;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Pinocio_Tree.isKeyAppear) return;

        isHighlighted = true;
        UIManager.instance.cursorBtTipText.text = "클릭하여 아이템을 줍습니다.";

        foreach (UnityEngine.UI.Outline item in keyOutlines)
        {
            item.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!Pinocio_Tree.isKeyAppear) return;

        isHighlighted = false;
        UIManager.instance.cursorBtTipText.text = "";

        foreach (UnityEngine.UI.Outline item in keyOutlines)
        {
            item.enabled = false;
        }
    }

    void Start()
    {
        keyOutlines = GetComponentsInChildren<UnityEngine.UI.Outline>();
    }

    void Update()
    {
        if(isHighlighted)
        {
            if(Input.GetMouseButtonDown(0))
            {
                const int WOODKEY_ID = 8;

                if (GameManager.Instance.inventoryManager.TryGetRemainingTab(WOODKEY_ID, out TabScript tab))
                {
                    isHighlighted = false;
                    if (tab.itemId != -1)
                    {
                        tab.itemCount++;
                    }
                    else
                    {
                        tab.itemId = WOODKEY_ID;
                        tab.itemCount = 1;
                    }
                    GameManager.Instance.inventoryManager.SelectedItemRefresh();
                    Pinocio_Tree.eventFlow = "hasnotkey";
                    GameManager.Instance.inventoryManager.TIP_ItemGotTipAppear(GameManager.Instance.itemData.infos[WOODKEY_ID].itemSprite);
                    UIManager.instance.cursorBtTipText.text = "";
                    plant.SetActive(false);
                    gameObject.SetActive(false);
                }
                else
                {
                    GameManager.Instance.inventoryManager.TIP_FullInventory();
                }
            }
        }
    }
}
