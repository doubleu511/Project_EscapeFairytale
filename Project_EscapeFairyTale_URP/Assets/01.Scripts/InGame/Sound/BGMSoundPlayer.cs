using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BGMSoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public float maxVolume = 1;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        audioSource.DOKill();
        audioSource.Play();
        audioSource.DOFade(maxVolume, 2);
    }

    private void OnTriggerExit(Collider other)
    {
        audioSource.DOKill();
        audioSource.DOFade(0, 2).OnComplete(() =>
        {
            audioSource.Stop();
        });
    }
}
