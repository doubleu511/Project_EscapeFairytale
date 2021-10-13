using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabScript : MonoBehaviour, IDragHandler, IBeginDragHandler, IDropHandler, IEndDragHandler, IPointerClickHandler
{
    public int itemId;
    [System.NonSerialized] public int tabId;

    public Image myImg;
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
            myImg.sprite = null;
        }
        else
        {
            myImg.sprite = GameManager.Instance.itemData.infos[itemId].itemSprite;
        }
    }

    // 드래그 오브젝트에서 발생
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (myImg.sprite == null)
        {
            return;
        }

        // Activate Container
        dragAndDropContainer.gameObject.SetActive(true);
        // Set Data
        dragAndDropContainer.itemId = itemId;
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
                myImg.sprite = dragAndDropContainer.image.sprite;
            }
            else
            {
                // Clear Data
                itemId = -1;
                myImg.sprite = null;
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

            // set data from drag object on Container
            myImg.sprite = dragAndDropContainer.image.sprite;
            itemId = dragAndDropContainer.itemId;

            // put data from drop object to Container.  
            dragAndDropContainer.image.sprite = tempSprite;
            dragAndDropContainer.itemId = tempItemId;

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
            }
            GameManager.Instance.inventoryManager.SelectedItemRefresh();

            GameManager.Instance.selectedItemUI.SetActive(true);
            GameManager.Instance.selectedItemUI.transform.position = transform.position;
        }
    }
}
