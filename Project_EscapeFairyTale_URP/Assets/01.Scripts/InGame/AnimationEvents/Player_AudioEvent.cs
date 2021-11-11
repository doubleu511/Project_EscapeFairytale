using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AudioEvent : MonoBehaviour
{
    private bool isLeft = true;

    private void WalkSFX()
    {
        if (GameManager.Instance.player.IsCheckGrounded())
        {
            if (isLeft)
            {
                GameManager.PlaySFX(GameManager.Instance.audioBox.player_leftWalk);
            }
            else
            {
                GameManager.PlaySFX(GameManager.Instance.audioBox.player_rightWalk);
            }
            isLeft = !isLeft;
        }
    }
}
