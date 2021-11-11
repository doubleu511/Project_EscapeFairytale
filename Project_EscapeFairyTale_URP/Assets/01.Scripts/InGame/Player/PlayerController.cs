using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnim;

    public bool isSubCam = false;
    public PlayerState playerState = PlayerState.NORMAL;
    public float speed = 5;
    public float jumpSpeed = 5;
    public float gravity = -9.81f;
    public LayerMask _fieldLayer;

    public const int cameraDefaultFOV = 70;

    float yVelocity;
    float airTime;

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

        if (!IsCheckGrounded())
        {
            yVelocity += gravity * Time.deltaTime;
        }
        else
        {
            if (yVelocity < 0)
            {
                if(yVelocity < -3)
                {
                    GameManager.PlaySFX(GameManager.Instance.audioBox.player_fall);
                }

                yVelocity = 0;
            }
        }

        //--- 이동 ----

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = Vector3.right * h + Vector3.forward * v;

        dir = transform.TransformDirection(dir);
        dir.Normalize();


        //--- 점프 ----

        bool jump = Input.GetKeyDown(KeyCode.Space) && Item_SizeChange.sizeValueRaw == -1 && IsCheckGrounded();

        if(jump)
        {
            Debug.Log("점프");
            yVelocity = jumpSpeed;
        }

        dir.y = yVelocity;
        cc.Move(dir * speed * ScaleToSpeed() * Time.deltaTime);
    }

    public bool IsCheckGrounded()
    {
        if (cc.isGrounded) return true;

        var ray = new Ray(this.transform.position, Vector3.down);
        var maxDistance = 0.01f;
        Debug.DrawRay(transform.position, Vector3.down * maxDistance, Color.red);
        return Physics.Raycast(ray, maxDistance, _fieldLayer);
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

            playerAnim.SetFloat("speed", (dis.sqrMagnitude / transform.localScale.x / transform.localScale.x) * 1000); // 너무 작아서 10,000 곱해줌
        }
    }
}
