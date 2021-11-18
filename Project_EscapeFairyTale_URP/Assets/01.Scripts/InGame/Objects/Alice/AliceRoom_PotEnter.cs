using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AliceRoom_PotEnter : MonoBehaviour
{
    public bool isEnter = false;
    private PlayerState prevState;

    private static bool isCoolTime = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!isCoolTime)
            {
                if (isEnter)
                {
                    prevState = GameManager.Instance.player.playerState;
                    GameManager.Instance.player.playerState = PlayerState.SLIPPING;
                    Moving(true);
                    StartCoroutine(MoveCoolTime());
                    isCoolTime = true;
                }
                else
                {
                    if(Alice_Ladder.ladderLevel >= 3)
                    {
                        prevState = GameManager.Instance.player.playerState;
                        GameManager.Instance.player.playerState = PlayerState.SLIPPING;
                        Moving(false);
                        StartCoroutine(MoveCoolTime());
                        isCoolTime = true;
                    }
                }
            }
        }
    }

    void Moving(bool entering)
    {
        if (entering)
        {
            GameManager.Instance.player.transform.DOMove(new Vector3(-4.133374f, 0.1134f, 3.5393f), 0.5f).OnStart(() =>
            {
                GameManager.Instance.player.transform.position = new Vector3(-4.133374f, 0.14f, 3.5393f);
                GameManager.Instance.player.transform.eulerAngles = new Vector3(0, 180, 0);
                Camera.main.transform.localEulerAngles = new Vector3(43.1f, 0, 0);
            }).OnComplete(() =>
            {
                GameManager.Instance.player.transform.DOMove(new Vector3(-4.133374f, 0.0489f, 3.3952f), 0.5f).OnComplete(() =>
                {
                    GameManager.Instance.player.playerState = prevState;
                });
            });
        }
        else
        {
            GameManager.Instance.player.transform.DOMove(new Vector3(-4.133374f, 0.1134f, 3.5393f), 0.5f).OnStart(() =>
            {
                GameManager.Instance.player.transform.position = new Vector3(-4.133374f, 0.0489f, 3.3952f);
                GameManager.Instance.player.transform.eulerAngles = new Vector3(0, 0, 0);
                Camera.main.transform.localEulerAngles = new Vector3(-43.1f, 0, 0);
            }).OnComplete(() =>
            {
                GameManager.Instance.player.transform.DOMove(new Vector3(-4.133374f, 0.14f, 3.5393f), 0.5f).OnComplete(() =>
                {
                    GameManager.Instance.player.playerState = prevState;
                });
            });
        }
    }
    
    IEnumerator MoveCoolTime()
    {
        yield return new WaitForSeconds(2f);
        isCoolTime = false;
    }
}
