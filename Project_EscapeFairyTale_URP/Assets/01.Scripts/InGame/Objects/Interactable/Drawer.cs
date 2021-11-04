using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Drawer : SelectableObject_Parent
{
    private const string lockTxt = "����ִ� �� �����ϴ�.";
    private const string unlockTxt = "Ŭ���Ͽ� ������ ���ݽ��ϴ�.";

    private BoxCollider itemBlockCollider;
    private Vector3 defaultPos;
    private Vector3 openPos;
    private bool isOpen = false;

    public AudioSource audioSource;
    public bool isLocked = true;

    [Tooltip("isLocked�� true�϶��� �ۼ��Ѵ�. ����� �����Ҷ� �ʿ��� ������ ���̵�. -1�̶�� �ٸ� �̺�Ʈ�� ���Ͽ� �����ؾ��Ѵ�.")]
    public int requireItemId = 0;
    public bool isItemBroke = false;

    protected override void Awake()
    {
        base.Awake();
        itemBlockCollider = GetComponent<BoxCollider>();
    }

    protected override void Start()
    {
        base.Start();
        defaultPos = transform.localPosition;
        openPos = transform.localPosition + new Vector3(0, -0.15f, 0);

        selectText = isLocked ? lockTxt : unlockTxt;

        itemBlockCollider.enabled = true;
    }

    public override void OnClicked()
    {
        if(!isLocked)
        {
            if (requireItemId != -1)
            {
                if (GameManager.Instance.selectedItemId == requireItemId)
                {
                    Unlock(true);
                    if (isItemBroke)
                    {
                        GameManager.Instance.inventoryManager.DecreaseTab(GameManager.Instance.selectedTab.tabId);
                    }
                }
            }

            if (isOpen)
            {
                GameManager.PlaySFX(audioSource, GameManager.Instance.audioBox.object_drawer_close);
                transform.DOLocalMove(defaultPos, 1);
                isOpen = false;
                itemBlockCollider.enabled = true;
            }
            else
            {
                GameManager.PlaySFX(audioSource, GameManager.Instance.audioBox.object_drawer_open);
                transform.DOLocalMove(openPos, 1);
                isOpen = true;
                itemBlockCollider.enabled = false;
            }
        }
        else
        {
            GameManager.PlaySFX(audioSource, GameManager.Instance.audioBox.object_door_lock);
        }
    }

    public void Unlock(bool soundPlay)
    {
        isLocked = false;
        selectText = unlockTxt;

        if(soundPlay) GameManager.PlaySFX(audioSource, GameManager.Instance.audioBox.object_door_unlock);
    }
}
