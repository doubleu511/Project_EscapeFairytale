using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliceRoom_Enter : MonoBehaviour
{
    public Transform playerBody;

    public Transform start;
    public Transform end;
    private float lineLength;

    private bool isEntering = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEntering = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEntering = false;
        }
    }

    private void Update()
    {
        if(isEntering)
        {
            float playerPos = Mathf.Clamp(GameManager.Instance.player.transform.position.z, end.position.z, start.position.z);
            float posNormalized = (start.position.z - playerPos) / (start.position.z - end.position.z);

            playerBody.localEulerAngles = new Vector3(playerBody.localEulerAngles.x, playerBody.localEulerAngles.y, posNormalized * -360);
            GameManager.Instance.player.currentSpeed = GameManager.Instance.player.defaultSpeed - (posNormalized * 2);
            GameManager.Instance.player.currentJumpSpeed = GameManager.Instance.player.defaultJumpSpeed * (-posNormalized + 1);
        }
    }
}
