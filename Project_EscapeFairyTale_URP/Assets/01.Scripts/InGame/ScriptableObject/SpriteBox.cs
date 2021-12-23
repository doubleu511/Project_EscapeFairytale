using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpriteBox", order = 1)]
public class SpriteBox : ScriptableObject
{
    // 오브젝트가 여러개여서 또는 몇개 없는 스프라이트를 변수로 사용하기 애매할떄, 여기에 써서 게임매니저에서 참조하여 사용한다.

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