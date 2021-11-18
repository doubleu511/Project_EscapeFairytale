using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : SelectableObject_Parent, ISaveAble
{
    public AudioSource audioSource;
    public bool sound_itHasLock = true;

    public bool isLocked = true;
    [Tooltip("isLocked가 true일때만 작성한다. 잠금을 해제할때 필요한 아이템 아이디")]
    public int requireItemId = 0;
    public bool isItemBroke = false;

    public int openDir;
    private bool isOpen = false;

    Transform parent_door = null;
    public Door friendDoor = null;

    [Header("Save")]
    public string saveKey;
    private string _eventFlow = "lock";
    public string eventFlow
    {
        get { return _eventFlow; }
        set
        {
            if (_eventFlow != value)
            {
                _eventFlow = value;
                TempSave();
            }
        }
    } // 1이면 언락, 0이면 락

    protected override void Start()
    {
        base.Start();
        parent_door = transform.parent;

        if (!saveKey.Equals(""))
        {
            if (!GameManager.saveDic.ContainsKey(saveKey))
            {
                GameManager.saveDic.Add(saveKey, eventFlow);
            }
            else
            {
                Load();
            }
        }
    }

    public override void OnClicked()
    {
        if(isLocked)
        {
            if(GameManager.Instance.selectedItemId == requireItemId)
            {
                isLocked = false;
                GameManager.PlaySFX(audioSource, GameManager.Instance.audioBox.object_door_unlock, SoundType.SFX);
                GameManager.Save();
                if (isItemBroke)
                {
                    GameManager.Instance.inventoryManager.DecreaseTab(GameManager.Instance.selectedTab.tabId);
                }
                selectText = "클릭하여 문을 여닫습니다.";
                eventFlow = "unlock";

                if(friendDoor != null)
                {
                    friendDoor.isLocked = false;
                    friendDoor.selectText = "클릭하여 문을 여닫습니다.";
                }
            }
            else
            {
                GameManager.PlaySFX(audioSource, GameManager.Instance.audioBox.object_door_lock, SoundType.SFX);
            }
        }
        else
        {
            if(isOpen)
            {
                DoorMove(false);
                if (sound_itHasLock)
                {
                    GameManager.PlaySFX(audioSource, GameManager.Instance.audioBox.object_door_open, SoundType.SFX);
                }
                else
                {
                    GameManager.PlaySFX(audioSource, GameManager.Instance.audioBox.object_door_close, SoundType.SFX);
                }
                isOpen = false;
                if(friendDoor != null)
                {
                    friendDoor.isOpen = false;
                }
            }
            else
            {
                DoorMove(true);
                GameManager.PlaySFX(audioSource, GameManager.Instance.audioBox.object_door_open, SoundType.SFX);
                isOpen = true;
                if (friendDoor != null)
                {
                    friendDoor.isOpen = true;
                }
            }
        }
    }

    private void DoorMove(bool open)
    {
        ignoreRaycast = true;
        parent_door.DOLocalRotate(new Vector3(0, 0, open ? openDir : -openDir), 1.5f).OnComplete(() =>
        {
            ignoreRaycast = false;
        }).SetRelative();

        if (friendDoor != null)
        {
            friendDoor.ignoreRaycast = true;
            friendDoor.parent_door.DOLocalRotate(new Vector3(0, 0, open ? friendDoor.openDir : -friendDoor.openDir), 1.5f).OnComplete(() =>
            {
                friendDoor.ignoreRaycast = false;
            }).SetRelative();
        }
    }

    public void TempSave()
    {
        if (saveKey != "")
        {
            GameManager.saveDic[saveKey] = eventFlow;
        }
    }

    public void Load()
    {
        eventFlow = GameManager.saveDic[saveKey];
        if (eventFlow.Equals("unlock"))
        {
            isLocked = false;
            selectText = "클릭하여 문을 여닫습니다.";
        }
    }
}
