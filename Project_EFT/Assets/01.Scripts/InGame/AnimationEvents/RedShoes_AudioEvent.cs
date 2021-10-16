using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedShoes_AudioEvent : MonoBehaviour
{
    private AudioSource redShoesAudioSource;

    void Awake()
    {
        redShoesAudioSource = transform.parent.GetComponent<AudioSource>();
    }

    private void LeftWalkSFX(int index)
    {
        switch (index)
        {
            case 0:
                GameManager.PlaySFX(redShoesAudioSource, GameManager.Instance.audioBox.RedShoes_redshoes_walk1_left);
                break;
            case 1:
                GameManager.PlaySFX(redShoesAudioSource, GameManager.Instance.audioBox.RedShoes_redshoes_walk2_left);
                break;
        }
    }

    private void RightWalkSFX(int index)
    {

        switch (index)
        {
            case 0:
                GameManager.PlaySFX(redShoesAudioSource, GameManager.Instance.audioBox.RedShoes_redshoes_walk1_right);
                break;
            case 1:
                GameManager.PlaySFX(redShoesAudioSource, GameManager.Instance.audioBox.RedShoes_redshoes_walk2_right);
                break;
        }
    }
}
