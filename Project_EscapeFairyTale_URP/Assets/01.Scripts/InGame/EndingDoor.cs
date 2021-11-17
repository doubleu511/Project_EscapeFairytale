using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDoor : MonoBehaviour
{
    public EnemyAI ai;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.player.playerState = PlayerState.ENDING;
            ai.sfxSource.volume = 0;
            ai.redShoesAmbientSource.volume = 0;

            UIManager.CanvasGroup_DefaultShow(UIManager.instance.blackScreenCanvasGroup, true, true, 2);
        }
    }
}
