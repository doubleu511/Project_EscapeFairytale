using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class EnemyAI : MonoBehaviour, ISaveAble
{
    public enum EnemyState
    {
        PATROL,
        TRACE,
        ATTACK,
        STUN
    }

    private static EnemyAI instance;

    public EnemyState state = EnemyState.PATROL; //처음에는 패트롤 상태로 둔다.
    
    private Transform playerTr;

    public float attackDist = 5.0f;
    public float traceDist = 10.0f;
    public float judgeDelay = 0.3f;

    public GameObject hitbox;
    public Transform hitbox_box;

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
    [HideInInspector] public AudioSource sfxSource;

    public int stunSec = 0;

    [Header("Save")]
    public string saveKey;
    private string _eventFlow = "notexist";
    public string eventFlow
    {
        get { return _eventFlow; }
        set
        {
            if (_eventFlow != value)
            {
                _eventFlow = value;
                TempSave();
            }
        }
    } // exist이면 있는것, notexist면 없는것 

    void Awake()
    {
        instance = this;
        moveAgent = GetComponent<MoveAgent>();
        agent = GetComponent<NavMeshAgent>();
        sfxSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        playerTr = GameManager.Instance.player.transform;
        ws = new WaitForSeconds(judgeDelay);//AI가 판단을 내리는 딜레이시간

        if (!saveKey.Equals(""))
        {
            if (!GameManager.saveDic.ContainsKey(saveKey))
            {
                GameManager.saveDic.Add(saveKey, eventFlow);
                gameObject.SetActive(false);
                return;
            }
            else
            {
                Load();
            }
        }
    }

    void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(DoAction());

        agent.autoBraking = true;
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
    }

    IEnumerator CheckState()
    {
        while(true){
            if(playerTr == null){
                yield return ws;
            }

            if (GameManager.Instance.player.playerState == PlayerState.DEAD) yield break;

            while (stunSec > 0)
            {
                if(state != EnemyState.STUN)
                {
                    GameManager.PlaySFX(GetComponent<AudioSource>(), GameManager.Instance.audioBox.RedShoes_redshoes_struggle, SoundType.SFX);
                }

                state = EnemyState.STUN;
                stunSec--;
                yield return new WaitForSeconds(1);
            }

            float dist = (playerTr.position - transform.position).sqrMagnitude;

            if (agent.path.corners.Length == 1)
            {
                if (state == EnemyState.TRACE)
                {
                    StopTrace();
                }
            }

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
                        GameOver();
                        GameManager.PlaySFX(GameManager.Instance.audioBox.ambient_dead_by_shoes);

                        Camera.main.transform.localRotation = Quaternion.identity;
                        Camera.main.transform.DOShakeRotation(8, 1, 70, 5, false).SetDelay(2).SetEase(Ease.OutExpo);
                        StartCoroutine(ScreenToGameOver());
                        GameManager.Instance.player.GetComponent<Animator>().Play("Player_DeadbyShoes");

                        transform.position = GameManager.Instance.player.transform.position;
                        transform.rotation = Quaternion.Euler(0, GameManager.Instance.player.transform.eulerAngles.y, 0);
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

    public static void AttackTest()
    {
        if (instance.state == EnemyState.ATTACK)
        {
            if (Item_SizeChange.sizeValueRaw == -1)
            {
                float randomRange = 0.6f;

                for (int i = 0; i < 2; i++)
                {
                    Vector3 playerFront = instance.playerTr.position + instance.playerTr.forward;

                    float randomX = Random.Range(playerFront.x - randomRange, playerFront.x + randomRange);
                    float randomZ = Random.Range(playerFront.z - randomRange, playerFront.z + randomRange);

                    Vector3 random = new Vector3(randomX, instance.hitbox_box.position.y, randomZ);
                    Instantiate(instance.hitbox, random, Quaternion.Euler(90, 0, 0), instance.hitbox_box);
                }
            }
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
                    break;
                case EnemyState.TRACE:
                    left_anim.SetBool("walk", true);
                    right_anim.SetBool("walk", true);
                    moveAgent.traceTarget = playerTr.position;
                    break;
                case EnemyState.ATTACK:
                    Stop();
                    break;
                case EnemyState.STUN:
                    Stop();
                    break;
            }
        }
    }

    public static void GameOver()
    {
        if (GameManager.Instance.player.isSubCam) UIManager.ChangeToMainCamera();

        GameManager.Instance.player.playerState = PlayerState.DEAD;
        instance.redShoesAmbientSource.volume = 0;

        if (instance.agent.enabled)
        {
            instance.Stop();
        }
        instance.agent.enabled = false;

        if (PlayerAction.currentObj != null)
        {
            PlayerAction.currentObj.GetComponent<SelectableObject>().OnDisHighlighted();
        }
    }

    IEnumerator ScreenToGameOver()
    {
        yield return new WaitForSecondsRealtime(6.5f);
        UIManager.GameOverUI(GameManager.Instance.spriteBox.Reason_Shoes_Big);
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
        Vector3 dir = (playerTr.position - (transform.position + new Vector3(0, 0.5f, 0)));
        //dir.y = 0;
        Debug.DrawRay(transform.position + new Vector3(0,0.5f,0), dir, Color.red, 1);
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), dir, out hit, 100, layerMask))
        {
            //Debug.Log(hit.collider.gameObject.name);
            isView = (hit.collider.gameObject.CompareTag("Player"));
        }
        return isView && agent.path.corners.Length == 2;
    }

    private bool waiting = false;

    private void PlaySFXTest(AudioClip clip)
    {
        if (!waiting)
        {
            waiting = true;
            GameManager.PlaySFX(clip);
            GameManager.Instance.urpSettings.TryGet<Vignette>(out Vignette vignette);
            DOTween.To(() => vignette.color.value, value => vignette.color.value = value, Color.red, 1).SetLoops(2, LoopType.Yoyo);
            DOTween.To(() => vignette.intensity.value, value => vignette.intensity.value = value, 0.5f, 0.5f).SetLoops(2, LoopType.Yoyo);
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
    Coroutine traceTime;

    private void TraceTime(float time)
    {
        if(!alreadyChasing)
        {
            unconditionallyTrace = true;
            traceTime = StartCoroutine(TraceCoolTime(time));
        }
    }

    private void StopTrace()
    {
        StopCoroutine(traceTime);
        unconditionallyTrace = false;
    }

    IEnumerator TraceCoolTime(float time)
    {
        yield return new WaitForSeconds(time);
        unconditionallyTrace = false;
    }

    public void TempSave()
    {
        if (saveKey != "")
        {
            GameManager.saveDic[saveKey] = eventFlow;
        }
    }

    public void Load()
    {
        eventFlow = GameManager.saveDic[saveKey];
        if (eventFlow.Equals("notexist"))
        {
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(CheckState());
            StartCoroutine(DoAction());

            agent.autoBraking = true;
            left_anim.SetBool("walk", true);
            right_anim.SetBool("walk", true);
        }
    }

    public void ShoesOn()
    {
        gameObject.SetActive(true);
        eventFlow = "exist";
    }

    #region ANIMATION_EVENTS



    #endregion
}
