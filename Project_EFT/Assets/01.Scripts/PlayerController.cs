using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public float gravity = -9.81f;

    float yVelocity;

    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        //--- 점프 ----

        //[점프]1. y속도에 중력을 계속 더한다.
        yVelocity += gravity * Time.deltaTime;

        //--- 이동 ----

        // 1. 사용자의 입력에 따라
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 2. 앞뒤 좌우로 방향을 만든다.
        Vector3 dir = Vector3.right * h + Vector3.forward * v;

        //카메라가 보고있는 방향을 앞 방향으로 변경한다.
        dir = Camera.main.transform.TransformDirection(dir);
        //로컬스페이스에서 월드스페이스로 변환 해준다. (트렌스폼 기준으로 결과를 바꾼다.)

        //대각선 이동으로 하면서 루트2로 길이가 늘어나기에 1로 만들어준다. (정규화:Normalize)
        dir.Normalize();

        //[점프]3. y속도를 최종 dir의 y에 대입한다.
        dir.y = yVelocity;

        // 3. 그 방향으로 이동한다.
        // P = P0 + vt
        //[이전 코드]transform.position += dir * speed * Time.deltaTime;

        cc.Move(dir * speed * Time.deltaTime);
        //Move 움직이전에 충돌 체크를 해준다. 만약 충돌한다면 멈춘다.
    }
}
