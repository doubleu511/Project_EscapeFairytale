using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        PATROL,
        TRACE,
        ATTACK
    }
    
    public EnemyState state = EnemyState.PATROL; //처음에는 패트롤 상태로 둔다.
    
    private Transform playerTr;

    public float attackDist = 5.0f;
    public float traceDist = 10.0f;
    public float judgeDelay = 0.3f;

    private bool alreadyChasing = false;

    private WaitForSeconds ws;
    private MoveAgent moveAgent;
    private NavMeshAgent agent;

    public Animator left_anim;
    public Animator right_anim;

    public LayerMask layerMask;

    void Awake()
    {
        moveAgent = GetComponent<MoveAgent>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        StartCoroutine(CheckState());
        StartCoroutine(DoAction());

        playerTr = GameManager.Instance.player.transform;
        ws = new WaitForSeconds(judgeDelay);//AI가 판단을 내리는 딜레이시간
        left_anim.SetBool("walk", true);
        right_anim.SetBool("walk", true);
    }

    void Update()
    {
        //anim.SetFloat(hashSpeed, moveAgent.speed);
        left_anim.SetFloat("speed", agent.speed / 2);
        right_anim.SetFloat("speed", agent.speed / 2);
    }

    IEnumerator CheckState()
    {
        while(true){
            if(playerTr == null){
                yield return ws;
            }

            // 여기서 아마 기절 만들듯

            float dist = (playerTr.position - transform.position).sqrMagnitude;

            Debug.Log(IsViewPlayer());

            //공격사거리 내라면 공격            
            if(dist <= attackDist * attackDist){
                if (IsViewPlayer())
                {
                    // 만약 중간 상태면 겜오버
                    // 작은 상태면 공격
                    if(Item_SizeChange.sizeValueRaw == 0)
                    {
                        /*
                        GameManager.Instance.player.playerState = PlayerState.DEAD;
                        MouseEvent.MouseLock(false);
                        UIManager.GameOverUI(GameManager.Instance.spriteBox.Reason_Shoes);
                        */
                    }
                    else if (Item_SizeChange.sizeValueRaw == -1)
                    {
                        Debug.Log("공격");
                    }
                    state = EnemyState.ATTACK;
                }
            }else if(IsViewPlayer() && dist <= traceDist * traceDist)
            {
                if (!alreadyChasing)
                {
                    int random = Random.Range(0, 2);
                    switch (random)
                    {
                        case 0:
                            PlaySFXTest(GameManager.Instance.audioBox.ambient_rising1);
                            break;
                        case 1:
                            PlaySFXTest(GameManager.Instance.audioBox.ambient_rising2);
                            break;
                    }
                }
                alreadyChasing = true;
                state = EnemyState.TRACE;
            }else {
                alreadyChasing = false;
                state = EnemyState.PATROL;
            }
            yield return ws; //저지 시간만큼 딜레이
        }
    }

    IEnumerator DoAction()
    {
        while(true){
            yield return ws;
            switch(state){
                case EnemyState.PATROL:
                    moveAgent.patrolling = true;
                    //anim.SetBool(hashMove, true);
                    break;
                case EnemyState.TRACE:
                    moveAgent.traceTarget = playerTr.position;
                    //anim.SetBool(hashMove, true);
                    break;
                case EnemyState.ATTACK:
                    moveAgent.Stop();
                    //anim.SetBool(hashMove, false);
                    break;
            }
        }
    }

    public bool IsViewPlayer()
    {
        bool isView = false;
        RaycastHit hit;
        Vector3 dir = (playerTr.position - transform.position);
        dir.y = 0;
        Debug.DrawRay(transform.position + new Vector3(0,playerTr.localScale.y,0), dir, Color.red, 1);
        if (Physics.Raycast(transform.position + new Vector3(0, playerTr.localScale.y, 0), dir, out hit, 100, layerMask))
        {
            Debug.Log(hit.collider.gameObject.name);
            isView = (hit.collider.gameObject.CompareTag("Player"));
        }
        return isView;
    }

    private bool waiting = false;

    private void PlaySFXTest(AudioClip clip)
    {
        if (!waiting)
        {
            waiting = true;
            GameManager.PlaySFX(clip);
            StartCoroutine(WaitCoolTime());
        }
    }

    IEnumerator WaitCoolTime()
    {
        yield return new WaitForSeconds(10);
        waiting = false;
    }

    #region ANIMATION_EVENTS



    #endregion
}
