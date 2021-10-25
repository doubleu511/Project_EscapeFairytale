using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

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

    public int heelSoundIndex = 0;

    [Range(0, 360)]
    public float viewAngle = 120.0f;

    public LayerMask playerLayerMask;

    private bool alreadyChasing = false;

    private WaitForSeconds ws;
    private MoveAgent moveAgent;
    private NavMeshAgent agent;

    public Animator left_anim;
    public Animator right_anim;

    public LayerMask layerMask;

    public AudioSource redShoesAmbientSource;

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
        left_anim.SetFloat("speed", agent.speed / 2);
        right_anim.SetFloat("speed", agent.speed / 2);

        //anim.SetFloat(hashSpeed, moveAgent.speed);
        if (state == EnemyState.TRACE)
        {
            if(!IsContactShoes())
            {
                heelSoundIndex = 1;

                left_anim.SetFloat("speed", agent.speed / 1.5f);
                right_anim.SetFloat("speed", agent.speed / 1.5f);
            }
            else
            {
                heelSoundIndex = 0;
            }
        }
        else
        {
            heelSoundIndex = 0;
        }

        Debug.Log(agent.isOnNavMesh);
    }

    IEnumerator CheckState()
    {
        while(true){
            if(playerTr == null){
                yield return ws;
            }

            if (GameManager.Instance.player.playerState == PlayerState.DEAD) yield break;

            // 여기서 아마 기절 만들듯

            float dist = (playerTr.position - transform.position).sqrMagnitude;

            bool traceCondition1 = IsViewPlayer() && dist <= traceDist * traceDist;
            bool traceCondition2 = unconditionallyTrace;

            //공격사거리 내라면 공격            
            if (dist <= attackDist * attackDist){
                if (IsViewPlayer())
                {
                    // 만약 중간 상태면 겜오버
                    // 작은 상태면 공격
                    if(Item_SizeChange.sizeValueRaw == 0)
                    {
                        if (GameManager.Instance.player.isSubCam) UIManager.ChangeToMainCamera();

                        GameManager.Instance.player.playerState = PlayerState.DEAD;
                        //MouseEvent.MouseLock(false);
                        redShoesAmbientSource.volume = 0;
                        GameManager.PlaySFX(GameManager.Instance.audioBox.ambient_dead_by_shoes);

                        Camera.main.transform.localRotation = Quaternion.identity;
                        Camera.main.transform.DOShakeRotation(8, 1, 70, 5, false).SetDelay(2).SetEase(Ease.OutExpo);
                        StartCoroutine(ScreenToGameOver());
                        GameManager.Instance.player.GetComponent<Animator>().Play("Player_DeadbyShoes");

                        Stop();
                        agent.enabled = false;

                        transform.position = GameManager.Instance.player.transform.position;
                        transform.rotation = Quaternion.Euler(0, GameManager.Instance.player.transform.eulerAngles.y, 0);

                        UIManager.ChangeToMainCamera();

                        if (PlayerAction.currentObj != null)
                        {
                            PlayerAction.currentObj.GetComponent<SelectableObject>().OnDisHighlighted();
                        }
                    }
                    else if (Item_SizeChange.sizeValueRaw == -1)
                    {
                        Debug.Log("공격");
                    }
                    state = EnemyState.ATTACK;
                }
            }else if(traceCondition1 || traceCondition2)
            {
                TraceTime(10);
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
            if (GameManager.Instance.player.playerState == PlayerState.DEAD) yield break;
            switch (state){
                case EnemyState.PATROL:
                    left_anim.SetBool("walk", true);
                    right_anim.SetBool("walk", true);
                    moveAgent.patrolling = true;
                    //anim.SetBool(hashMove, true);
                    break;
                case EnemyState.TRACE:
                    left_anim.SetBool("walk", true);
                    right_anim.SetBool("walk", true);
                    moveAgent.traceTarget = playerTr.position;
                    //anim.SetBool(hashMove, true);
                    break;
                case EnemyState.ATTACK:
                    Stop();
                    //anim.SetBool(hashMove, false);
                    break;
            }
        }
    }

    IEnumerator ScreenToGameOver()
    {
        yield return new WaitForSecondsRealtime(6.5f);
        UIManager.GameOverUI(GameManager.Instance.spriteBox.Reason_Shoes);
    }

    public void Stop()
    {
        left_anim.SetBool("walk", false);
        right_anim.SetBool("walk", false);

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        moveAgent.patrolling = false;
    }

    private bool IsViewPlayer()
    {
        bool isView = false;
        RaycastHit hit;
        Vector3 dir = (playerTr.position - transform.position);
        dir.y = 0;
        Debug.DrawRay(transform.position + new Vector3(0,playerTr.localScale.y,0), dir, Color.red, 1);
        if (Physics.Raycast(transform.position + new Vector3(0, playerTr.localScale.y, 0), dir, out hit, 100, layerMask))
        {
            //Debug.Log(hit.collider.gameObject.name);
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

    private bool IsContactShoes()
    {
        bool isContact = false;
        Collider[] colls = Physics.OverlapSphere(transform.position, traceDist, playerLayerMask);
        if (colls.Length >= 1)
        {
            Vector3 dir = (transform.position - playerTr.position).normalized;

            if (Vector3.Angle(playerTr.forward, dir) < viewAngle * 0.5f)
            {
                isContact = true;
            }
        }
        return isContact;
    }

    private bool unconditionallyTrace = false;

    private void TraceTime(float time)
    {
        if(!alreadyChasing)
        {
            unconditionallyTrace = true;
            StartCoroutine(TraceCoolTime(time));
        }
    }

    IEnumerator TraceCoolTime(float time)
    {
        yield return new WaitForSeconds(time);
        unconditionallyTrace = false;
    }

    #region ANIMATION_EVENTS



    #endregion
}
