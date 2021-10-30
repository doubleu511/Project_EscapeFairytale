using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnim;

    public bool isSubCam = false;
    public PlayerState playerState = PlayerState.NORMAL;
    public float speed = 5;
    public float gravity = -9.81f;

    public const int cameraDefaultFOV = 70;

    float yVelocity;

    CharacterController cc;

    private void Awake()
    {
        playerAnim = GetComponent<Animator>();
    }

    void Start()
    {
        cc = GetComponent<CharacterController>();
        StartCoroutine(SpeedCheck());
    }

    void Update()
    {
        if (GameManager.Instance.player.playerState == PlayerState.DEAD) return;
        if (Cursor.lockState == CursorLockMode.None) return;

        if (!cc.isGrounded)
        {
            yVelocity += gravity * Time.deltaTime;
        }
        else
        {
            yVelocity = 0;
        }

        //--- ¿Ãµø ----

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = Vector3.right * h + Vector3.forward * v;

        dir = transform.TransformDirection(dir);
        dir.Normalize();
        dir.y = yVelocity;

        cc.Move(dir * speed * ScaleToSpeed() * Time.deltaTime);
    }

    float ScaleToSpeed()
    {
        if(transform.localScale.x <= 0.04)
        {
            return transform.localScale.x * 6;
        }
        else
        {
            return transform.localScale.x;
        }
    }

    IEnumerator SpeedCheck()
    {
        while(true)
        {
            Vector3 currentPos = transform.position;
            yield return new WaitForSeconds(0.1f);
            Vector3 secondPos = transform.position;

            Vector3 dis = currentPos - secondPos;

            playerAnim.SetFloat("speed", (dis.sqrMagnitude / transform.localScale.x / transform.localScale.x) * 1000); // ≥ π´ ¿€æ∆º≠ 10,000 ∞ˆ«ÿ¡‹
        }
    }
}
