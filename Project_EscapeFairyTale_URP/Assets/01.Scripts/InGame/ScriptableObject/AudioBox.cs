using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AudioBox", order = 1)]
public class AudioBox : ScriptableObject // 오디오를 모두 담는 스크립터블 오브젝트, 게임매니저에서 참조
{
    [Header("SFX")]
    public AudioClip ambient_dead_by_shoes;
    public AudioClip ambient_rising1;
    public AudioClip ambient_rising2;

    public AudioClip object_book_close;
    public AudioClip object_book_nextpage;
    public AudioClip object_book_pickup;
    public AudioClip object_door_lock;
    public AudioClip object_door_unlock;
    public AudioClip object_door_open;
    public AudioClip object_door_close;
    public AudioClip object_drawer_open;
    public AudioClip object_drawer_close;
    public AudioClip object_coffer_tick;
    public AudioClip object_coffer_open;
    public AudioClip object_coffer_wrong;
    public AudioClip object_coffer_correct;
    public AudioClip object_coffer_light_on;
    public AudioClip object_clock_bell;
    public AudioClip object_clock_correct;
    public AudioClip object_clock_ambient;
    public AudioClip object_clock_powerOn;
    public AudioClip object_clock_bomb;
    public AudioClip object_clock_bigbell;
    public AudioClip object_clock_glassbreak;
    public AudioClip object_clock_assembly;
    public AudioClip object_pillow_move;
    public AudioClip object_tree_grow1;
    public AudioClip object_tree_grow2;
    public AudioClip object_tree_break;
    public AudioClip object_pickup;

    public AudioClip RedShoes_redshoes_walk1_left;
    public AudioClip RedShoes_redshoes_walk1_right;
    public AudioClip RedShoes_redshoes_walk2_left;
    public AudioClip RedShoes_redshoes_walk2_right;
    public AudioClip RedShoes_redshoes_struggle;

    public AudioClip ui_menu_select;
    public AudioClip ui_game_start;
    public AudioClip ui_module_clear;
    public AudioClip ui_tapSound;

    public AudioClip player_leftWalk;
    public AudioClip player_rightWalk;
    public AudioClip player_fall;
    public AudioClip player_eatMuffin;
    public AudioClip player_eatMilk;
}