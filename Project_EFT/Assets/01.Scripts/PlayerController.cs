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
        //--- ���� ----

        //[����]1. y�ӵ��� �߷��� ��� ���Ѵ�.
        yVelocity += gravity * Time.deltaTime;

        //--- �̵� ----

        // 1. ������� �Է¿� ����
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 2. �յ� �¿�� ������ �����.
        Vector3 dir = Vector3.right * h + Vector3.forward * v;

        //ī�޶� �����ִ� ������ �� �������� �����Ѵ�.
        dir = Camera.main.transform.TransformDirection(dir);
        //���ý����̽����� ���彺���̽��� ��ȯ ���ش�. (Ʈ������ �������� ����� �ٲ۴�.)

        //�밢�� �̵����� �ϸ鼭 ��Ʈ2�� ���̰� �þ�⿡ 1�� ������ش�. (����ȭ:Normalize)
        dir.Normalize();

        //[����]3. y�ӵ��� ���� dir�� y�� �����Ѵ�.
        dir.y = yVelocity;

        // 3. �� �������� �̵��Ѵ�.
        // P = P0 + vt
        //[���� �ڵ�]transform.position += dir * speed * Time.deltaTime;

        cc.Move(dir * speed * Time.deltaTime);
        //Move ���������� �浹 üũ�� ���ش�. ���� �浹�Ѵٸ� �����.
    }
}
