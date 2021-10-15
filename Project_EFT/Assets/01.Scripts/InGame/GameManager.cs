using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

     public bool isGameOver = false;

    [Header("Management")]
    public InventoryManager inventoryManager;
    public SpriteBox spriteBox;
    public AudioBox audioBox;

    [Header("Player")]
    public PlayerController player;

    [Header("Inventory")]
    public ItemData itemData;
    public int selectedItemId;
    public TabScript selectedTab;
    public GameObject selectedItemUI;

    [Header("ItemEffects")]
    public UnityEvent[] itemUseCallback;

    [Header("AudioSource")]
    public AudioSource defaultSFXSource;

    private void Awake()
    {
        if(Instance)
        {
            Debug.LogError("다수의 게임매니저 실행중");
        }

        Instance = this;
    }

    void Start()
    {
        MouseEvent.MouseLock(false);
    }

    public static void PlaySFX(AudioSource source, AudioClip clip, float volume = 1)
    {
        source.PlayOneShot(clip, volume);
    }

    public static void PlaySFX(AudioClip clip, float volume = 1)
    {
        Instance.defaultSFXSource.PlayOneShot(clip, volume);
    }
}
