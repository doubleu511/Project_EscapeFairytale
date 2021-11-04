using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveInteract : SelectableObject_Parent
{
    private Vector3 defaultPos;
    private Vector3 openPos;
    public Vector3 openToPos;


    private Quaternion defaultRotate;
    private Quaternion openRotate;
    public Vector3 openToRotate;

    private bool isOpen = false;


    public AudioSource audioSource;

    public AudioClip SFX_Open;
    public AudioClip SFX_Back;

    protected override void Start()
    {
        base.Start();
        defaultPos = transform.localPosition;
        openPos = transform.localPosition + openToPos;

        defaultRotate = transform.localRotation;
        openRotate = transform.localRotation *= Quaternion.Euler(openToRotate);
    }

    public override void OnClicked()
    {
        if (isOpen)
        {
            if(SFX_Back != null) GameManager.PlaySFX(audioSource, SFX_Back);
            transform.DOLocalMove(defaultPos, 1);
            transform.DOLocalRotateQuaternion(defaultRotate, 1);
            isOpen = false;
        }
        else
        {
            if (SFX_Open != null) GameManager.PlaySFX(audioSource, SFX_Open);
            transform.DOLocalMove(openPos, 1);
            transform.DOLocalRotateQuaternion(openRotate, 1);
            isOpen = true;
        }
    }
}
