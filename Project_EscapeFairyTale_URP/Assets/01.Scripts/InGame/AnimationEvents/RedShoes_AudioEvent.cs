using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RedShoes_AudioEvent : MonoBehaviour
{
    private AudioSource redShoesAudioSource;
    public EnemyAI enemyAI;

    void Awake()
    {
        redShoesAudioSource = transform.parent.GetComponent<AudioSource>();
    }

    private void LeftWalkSFX()
    {
        EnemyAI.AttackTest();
        switch (enemyAI.heelSoundIndex)
        {
            case 0:
                GameManager.PlaySFX(redShoesAudioSource, GameManager.Instance.audioBox.RedShoes_redshoes_walk1_left, SoundType.SFX);
                if(!GameManager.Instance.player.isSubCam) Camera.main.DOFieldOfView(Item_SizeChange.currentFOV, 0.2f).SetLoops(7, LoopType.Yoyo);
                break;
            case 1:
                GameManager.PlaySFX(redShoesAudioSource, GameManager.Instance.audioBox.RedShoes_redshoes_walk2_left, SoundType.SFX);
                if (!GameManager.Instance.player.isSubCam) Camera.main.DOFieldOfView(Item_SizeChange.currentFOV * 0.92f, 0.2f).SetLoops(2, LoopType.Yoyo);
                break;
        }
    }

    private void RightWalkSFX()
    {
        EnemyAI.AttackTest();
        switch (enemyAI.heelSoundIndex)
        {
            case 0:
                GameManager.PlaySFX(redShoesAudioSource, GameManager.Instance.audioBox.RedShoes_redshoes_walk1_right, SoundType.SFX);
                if (!GameManager.Instance.player.isSubCam) Camera.main.DOFieldOfView(Item_SizeChange.currentFOV, 0.2f).SetLoops(7, LoopType.Yoyo);
                break;
            case 1:
                GameManager.PlaySFX(redShoesAudioSource, GameManager.Instance.audioBox.RedShoes_redshoes_walk2_right, SoundType.SFX);

                if (!GameManager.Instance.player.isSubCam) Camera.main.DOFieldOfView(Item_SizeChange.currentFOV, 0.2f).SetLoops(2, LoopType.Yoyo);
                break;
        }
    }
}
