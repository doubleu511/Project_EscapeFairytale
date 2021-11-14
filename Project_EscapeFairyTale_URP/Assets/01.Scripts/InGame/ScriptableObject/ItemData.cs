using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    public ItemDataInfo[] infos;
}

[System.Serializable]
public class ItemDataInfo
{
    public int itemId;
    public string itemName;
    [TextArea(5, 10)]
    public string itemLore;
    public Sprite itemSprite;
}