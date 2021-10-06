using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Management")]
    public InventoryManager inventoryManager;

    [Header("Player")]
    public PlayerController player;

    [Header("Inventory")]
    public ItemData itemData;
    public ItemDataInfo[] playerInventory = new ItemDataInfo[6];
    public int selectedItemId;
    public TabScript selectedTab;
    public GameObject selectedItemUI;

    private void Awake()
    {
        if(Instance)
        {
            Debug.LogError("�ټ��� ���ӸŴ��� ������");
        }

        Instance = this;
    }

    void Start()
    {
        MouseEvent.MouseLock(false);
    }

    void Update()
    {
        
    }
}
