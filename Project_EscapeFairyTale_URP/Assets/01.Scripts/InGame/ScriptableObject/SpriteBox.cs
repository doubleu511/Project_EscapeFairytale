using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpriteBox", order = 1)]
public class SpriteBox : ScriptableObject
{
    public Sprite Reason_Shoes_Big;
    public Sprite Reason_Shoes_Small;
    public Sprite UI_CantEat_Muffin;
    public Sprite UI_CantEat_Milk;
    public Sprite UI_WoodSegment;
    public Sprite UI_Bird;
    public Sprite UI_Mouse;
    public Sprite UI_Mouse_Click;
    public Sprite UI_Keyboard;
    public Sprite UI_Keyboard_Click;
    public Sprite[] UI_Book_Covers;
    [Header("MainMenu-Game")]
    public Sprite NewGame;
}