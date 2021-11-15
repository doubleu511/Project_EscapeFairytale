using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabScript : MonoBehaviour, IDragHandler, IBeginDragHandler, IDropHandler, IEndDragHandler, IPointerClickHandler
{
    public int itemId;
    public int itemCount = -1;
    [System.NonSerialized] public int tabId;

    public Image myImg;
    public Text myCountText;
    public DragAndDropContainer dragAndDropContainer;


    bool isDragging = false;

    private void Awake()
    {
        for(int i = 0; i <GameManager.Instance.inventoryManager.tabs.Length;i++)
        {
            if(GameManager.Instance.inventoryManager.tabs[i] == this)
            {
                tabId = i;
            }
        }
    }

    public void Refresh()
    {
        if (itemId == -1)
        {
            myImg.enabled = false;
            itemCount = -1;
            myCountText.text = "";
        }
        else
        {
            myImg.enabled = true;
            myImg.sprite = GameManager.Instance.itemData.infos[itemId].itemSprite;
            myCountText.text = itemCount > 1 ? $"x{itemCount}" : "";
        }
    }

    // 드래그 오브젝트에서 발생
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!myImg.enabled)
        {
            return;
        }

        // Activate Container
        dragAndDropContainer.gameObject.SetActive(true);
        // Set Data
        dragAndDropContainer.itemId = itemId;
        dragAndDropContainer.itemCount = itemCount;
        dragAndDropContainer.image.sprite = myImg.sprite;
        dragAndDropContainer.movingTab = this;
        isDragging = true;
    }
    // 드래그 오브젝트에서 발생
    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            dragAndDropContainer.transform.position = eventData.position;
        }
    }
    // 드래그 오브젝트에서 발생
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            if (dragAndDropContainer.image.sprite != null)
            {
                // set data from dropped object
                itemId = dragAndDropContainer.itemId;
                itemCount = dragAndDropContainer.itemCount;
                myImg.enabled = true;
                myImg.sprite = dragAndDropContainer.image.sprite;
                myCountText.text = itemCount > 1 ? $"x{itemCount}" : "";
            }
            else
            {
                // Clear Data
                itemId = -1;
                itemCount = -1;
                myImg.enabled = false;
                myCountText.text = "";
            }
        }

        isDragging = false;
        // Reset Contatiner
        dragAndDropContainer.image.sprite = null;
        dragAndDropContainer.gameObject.SetActive(false);
    }

    // 드롭 오브젝트에서 발생
    public void OnDrop(PointerEventData eventData)
    {
        if (dragAndDropContainer.image.sprite != null)
        {
            // keep data instance for swap 
            Sprite tempSprite = myImg.sprite;
            int tempItemId = itemId;
            int tempItemCount = itemCount;

            // set data from drag object on Container
            myImg.sprite = dragAndDropContainer.image.sprite;
            itemId = dragAndDropContainer.itemId;
            itemCount = dragAndDropContainer.itemCount;
            myCountText.text = itemCount > 1 ? $"x{itemCount}" : "";

            // put data from drop object to Container.  
            dragAndDropContainer.image.sprite = tempSprite;
            dragAndDropContainer.itemId = tempItemId;
            dragAndDropContainer.itemCount = tempItemCount;

            if (dragAndDropContainer.movingTab == GameManager.Instance.selectedTab)
            {
                GameManager.Instance.selectedItemUI.transform.position = transform.position;
                GameManager.Instance.selectedTab = this;
            }
            else if (GameManager.Instance.selectedTab == this)
            {
                GameManager.Instance.selectedItemUI.transform.position = dragAndDropContainer.movingTab.transform.position;
                GameManager.Instance.selectedTab = dragAndDropContainer.movingTab;
            }
        }
        else
        {
            dragAndDropContainer.image.sprite = null;
            dragAndDropContainer.itemCount = -1;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemId != -1)
        {
            if(GameManager.Instance.selectedTab == this)
            {
                GameManager.Instance.inventoryManager.tabClickCount++;

                if(GameManager.Instance.inventoryManager.tabClickCount >= 2)
                {
                    GameManager.Instance.inventoryManager.tabClickCount = 0;
                    GameManager.Instance.inventoryManager.ItemDetailOpen();
                }
            }
            else
            {
                GameManager.Instance.selectedItemId = itemId;
                GameManager.Instance.inventoryManager.tabClickCount = 0;
                GameManager.Instance.selectedTab = this;

                if (PlayerAction.currentObj != null)
                {
                    PlayerAction.currentObj.GetComponent<SelectableObject>().OnHighlighted(PlayerAction.currentObj.GetComponent<SelectableObject>().selectText);
                }
            }
            GameManager.Instance.inventoryManager.SelectedItemRefresh();

            GameManager.Instance.selectedItemUI.SetActive(true);
            GameManager.Instance.selectedItemUI.transform.position = transform.position;
        }
    }
}
