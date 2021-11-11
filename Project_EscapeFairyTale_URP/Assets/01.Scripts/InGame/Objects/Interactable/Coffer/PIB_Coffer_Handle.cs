using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class PIB_Coffer_Handle : SelectableObject
{
    private bool isLocked = true;
    public UnityEvent clearEvent;
    public Transform coffer_Door;
    public AudioSource coffer_audioSource;

    public void UnLock()
    {
        isLocked = false;
    }

    public override void OnClicked()
    {
        if(!isLocked)
        {
            clearEvent.Invoke();
            coffer_Door.DOLocalRotate(new Vector3(0, 0, 130), 2);
            GameManager.PlaySFX(coffer_audioSource, GameManager.Instance.audioBox.object_coffer_open, SoundType.SFX);
        }
    }
}
