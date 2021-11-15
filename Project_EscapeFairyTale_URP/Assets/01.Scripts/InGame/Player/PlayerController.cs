using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnim;

    public bool isSubCam = false;
    public PlayerState playerState = PlayerState.NORMAL;
    public float defaultSpeed = 3;

    [HideInInspector]
    public float currentSpeed = 3;
    public float defaultJumpSpeed = 5;

    [HideInInspector]
    public float currentJumpSpeed = 5;
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
        currentSpeed = defaultSpeed;
        currentJumpSpeed = defaultJumpSpeed;
        cc = GetComponent<CharacterController>();
        StartCoroutine(SpeedCheck());
    }

    void Update()
    {
        if (playerState == PlayerState.DEAD) return;
        if (playerState == PlayerState.WAKING_UP) return;

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

        Vector3 dir = Vector3.zero;
        if (Cursor.lockState != CursorLockMode.None)
        {
            //--- �̵� ----

            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            dir = Vector3.right * h + Vector3.forward * v;

            dir = transform.TransformDirection(dir);
            dir.Normalize();


            //--- ���� ----

            bool jump = Input.GetKeyDown(KeyCode.Space) && Item_SizeChange.sizeValueRaw == -1 && IsCheckGrounded();

            if (jump)
            {
                yVelocity = currentJumpSpeed;
            }
        }

        dir.y = yVelocity;
        cc.Move(dir * currentSpeed * ScaleToSpeed() * Time.deltaTime);
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

            playerAnim.SetFloat("speed", (dis.sqrMagnitude / transform.localScale.x / transform.localScale.x) * 1000); // �ʹ� �۾Ƽ� 10,000 ������
        }
    }

    void Event_WakeEnd()
    {
        UIManager.InGameAppear(true);
        playerState = PlayerState.NORMAL;
        playerAnim.Play("Player_idle");

        UIManager.TutorialPanel("���콺�� �̿��Ͽ� �þ߸� �����Դϴ�.");
        UIManager.TutorialPanel("W/A/S/D �� �̿��Ͽ� �̵��� �� �ֽ��ϴ�.");
    }
}
