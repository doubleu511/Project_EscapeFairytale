using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AudioBox", order = 1)]
public class AudioBox : ScriptableObject
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

    public AudioClip RedShoes_redshoes_walk1_left;
    public AudioClip RedShoes_redshoes_walk1_right;
    public AudioClip RedShoes_redshoes_walk2_left;
    public AudioClip RedShoes_redshoes_walk2_right;
}