using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : SelectableObject_Parent
{
    public AudioSource audioSource;

    public bool isLocked = true;
    [Tooltip("isLocked�� true�϶��� �ۼ��Ѵ�. ����� �����Ҷ� �ʿ��� ������ ���̵�")]
    public int requireItemId = 0;
    public bool isItemBroke = false;

    private int currentDir = 0;

    public Transform parent_door = null;

    protected override void Start()
    {
        base.Start();
        parent_door = transform.parent;
    }

    public override void OnClicked()
    {
        if(isLocked)
        {
            if(GameManager.Instance.selectedItemId == requireItemId)
            {
                isLocked = false;
                GameManager.PlaySFX(audioSource, GameManager.Instance.audioBox.object_door_unlock);
                if(isItemBroke)
                {
                    GameManager.Instance.inventoryManager.DecreaseTab(GameManager.Instance.selectedTab.tabId);
                }
                selectText = "Ŭ���Ͽ� ���� ���ݽ��ϴ�.";
            }
            else
            {
                GameManager.PlaySFX(audioSource, GameManager.Instance.audioBox.object_door_lock);
            }
        }
        else
        {
            switch(currentDir)
            {
                case -1:
                    DoorMove(1);
                    currentDir = 0;
                    GameManager.PlaySFX(audioSource, GameManager.Instance.audioBox.object_door_close);
                    break;
                case 0:
                    DoorMove(1);
                    currentDir = 1;
                    GameManager.PlaySFX(audioSource, GameManager.Instance.audioBox.object_door_open);
                    break;
                case 1:
                    DoorMove(-1);
                    currentDir = 0;
                    GameManager.PlaySFX(audioSource, GameManager.Instance.audioBox.object_door_close);
                    break;
            }
        }
    }

    private void DoorMove(int dir)
    {
        ignoreRaycast = true;
        parent_door.DOLocalRotate(new Vector3(0, 0, 90 * dir), 1.5f).SetRelative().OnComplete(() =>
        {
            ignoreRaycast = false;
        });
    }
}
