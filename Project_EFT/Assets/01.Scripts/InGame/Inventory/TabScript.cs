using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabScript : MonoBehaviour, IDragHandler, IBeginDragHandler, IDropHandler, IEndDragHandler, IPointerClickHandler
{
    public int itemId;

    public Image myImg;
    public DragAndDropContainer dragAndDropContainer;

    bool isDragging = false;

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
                myImg.sprite = null;
                itemId = -1;
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
            GameManager.Instance.selectedItemId = itemId;
            GameManager.Instance.inventoryManager.SelectedItemRefresh();
        }
    }
}
