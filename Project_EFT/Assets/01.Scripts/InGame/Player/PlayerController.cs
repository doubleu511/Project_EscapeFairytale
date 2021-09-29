using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnim;

    public PlayerState playerState = PlayerState.NORMAL;

    public float speed = 5;
    public float gravity = -9.81f;

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

        dir = Camera.main.transform.TransformDirection(dir);
        dir.Normalize();
        dir.y = yVelocity;

        cc.Move(dir * speed * Time.deltaTime);
    }

    IEnumerator SpeedCheck()
    {
        while(true)
        {
            Vector3 currentPos = transform.position;
            yield return new WaitForFixedUpdate();
            Vector3 secondPos = transform.position;

            playerAnim.SetFloat("speed", Vector3.Distance(currentPos,secondPos));
        }
    }
}
