using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpriteBox", order = 1)]
public class SpriteBox : ScriptableObject
{
    public Sprite Reason_Shoes_Big;
    public Sprite Reason_Shoes_Small;
    public Sprite UI_CantEat_Muffin;
    public Sprite UI_CantEat_Milk;
    [Header("MainMenu-Game")]
    public Sprite NewGame;
}