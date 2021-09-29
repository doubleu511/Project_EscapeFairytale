using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (GameManager.Instance.player.playerState == PlayerState.NORMAL)
            {
                GameManager.Instance.inventoryManager.Open();
            }
            else if (GameManager.Instance.player.playerState == PlayerState.OPEN_INVENTORY)
            {
                GameManager.Instance.inventoryManager.Close();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.player.playerState == PlayerState.OPEN_INVENTORY)
            {
                GameManager.Instance.inventoryManager.Close();
            }
        }
    }
}
