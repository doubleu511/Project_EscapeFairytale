using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class PIB_Coffer_Handle : SelectableObject
{
    private bool isLocked = true;
    public UnityEvent clearEvent;
    public UnityEvent loadEvent;
    public Transform coffer_Door;
    public AudioSource coffer_audioSource;

    public void UnLock(bool instant)
    {
        isLocked = false;
        if(instant)
        {
            coffer_Door.localEulerAngles = new Vector3(0, 0, 130);
            loadEvent.Invoke();
        }
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
