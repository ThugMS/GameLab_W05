using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Progress;

public class MeleeBoss : BaseMonster
{
    enum Pattern
    {
        Dash,
        Generate,
        Attack,
        Invincible,
        Rest,
        Dead
    }
    public GameObject minions;
    protected Vector3 m_targetPos;


    [SerializeField] private Pattern m_currentPattern;
    private SpriteRenderer m_renderer;

    private int m_rushSpeed;
    public float attackRange;

    private bool m_onAction;

    [Header("Melee Boss Skill")]
    private static readonly int Walking = Animator.StringToHash("isWalking");
    private static readonly int AttackSkill = Animator.StringToHash("attackSkill");
    private static readonly int ShieldSkill = Animator.StringToHash("shieldSkill");
    [SerializeField] private Collider2D m_playerCol;
    private int m_playerLayerMask;
    public bool isInvincible = false;
    [SerializeField] private float m_power;
    [SerializeField] private Vector2 m_attackBoxSize;
    [SerializeField] private float m_attackBoxYOffset;


    public void Start()
    {
        init();
        m_playerLayerMask = LayerMask.GetMask("Player");
        isBoss = true;
    }

    public override void init()
    {
        base.init();
        m_renderer = GetComponentInChildren<SpriteRenderer>();
        m_animator = GetComponentInChildren<Animator>();
        m_onAction = false;

    }

    public void AttackPlayer()
    {
        CheckCollider();
        if (m_playerCol == null)
            return;

        Player player;
        m_playerCol.TryGetComponent<Player>(out player);

        player.GetDamage(m_power);
    }

    private void CheckCollider()
    {
        m_playerCol = null;

        m_playerCol = Physics2D.OverlapBox(transform.position - Vector3.up * m_attackBoxYOffset, m_attackBoxSize, 0, m_playerLayerMask);
    }

    protected override void stateUpdate()
    {
        if (!m_onAction)
        {
            print(m_currentPattern);
            if (Vector2.Distance(m_playerObj.transform.position, transform.position) < attackRange)
            {
                m_currentPattern = Pattern.Attack;

            }
            switch (m_currentPattern)
            {
                case Pattern.Attack:
                    m_onAction = true;
                    m_animator.SetTrigger("attackSkill");
                    break;
                case Pattern.Dash:
                    m_onAction = true;
                    StartCoroutine(MoveToPlayer(m_playerObj.transform.position));
                    break;
                case Pattern.Generate:
                    m_onAction = true;
                    makeMinions(3);
                    break;
                case Pattern.Rest:
                    m_onAction = true;
                    StartCoroutine(Rest());
                    break;
                case Pattern.Invincible:
                    m_onAction = true;
                    StartCoroutine(IEInvincible());
                    break;
                case Pattern.Dead:
                    m_onAction = true;
                    StartCoroutine(IE_PlayDyingEffect());
                    break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red; // You can change the color to your preference.
        Gizmos.DrawWireCube(transform.position - Vector3.up * m_attackBoxYOffset, m_attackBoxSize);
    }

    public void End()
    {
        EndWalkingAnimation();
        m_onAction = false;
        m_currentPattern = (Pattern)UnityEngine.Random.Range(0, 2);
    }

    IEnumerator IEEnd()
    {
        EndWalkingAnimation();
        m_onAction = false;
        m_currentPattern = (Pattern)UnityEngine.Random.Range(0, 2);
        yield return null;
    }

    //=============================================================

    IEnumerator MoveToPlayer(Vector2 lastPlayerPosition)
    {
        print("MoveToPlayer");
        TowardPlayer();
        m_animator.SetBool(Walking, true);
        float timer = 0f;
        while (Vector2.Distance(transform.position, lastPlayerPosition) >= 3f && timer < 3)
        {
            if (m_currentPattern == Pattern.Dash) 
            {
                transform.position = Vector2.MoveTowards(transform.position, lastPlayerPosition, m_speed * Time.deltaTime);
                timer += Time.deltaTime;
            }

            
            yield return null;
        }
        yield return IEEnd();
    }

    public IEnumerator Rest()
    {
        m_animator.SetBool(Walking, false);
        yield return new WaitForSeconds(5);
        yield return IEEnd();
    }


    IEnumerator IEInvincible()
    {
        m_animator.SetBool(Walking, false);
        TowardPlayer();
        m_renderer.color = Color.blue;
        isInvincible = true;
        yield return new WaitForSeconds(5);
        isInvincible = false;
        m_renderer.color = Color.white;
        yield return IEEnd();
    }

    //=================================


    protected override void OnCollisionStay2D(Collision2D collision)
    {


        if (collision.gameObject.CompareTag("Player"))
        {
            DamagePlayer(collision.gameObject);
        }

    }

    public override void getDamage(float _damage)
    {
        if (isInvincible == false)
        {
            Health -= _damage;
            StartCoroutine(IE_TweenDamage());
            
            if (Health <= 0)
            {
                m_onAction = true;
                m_animator.SetBool("isDead", true);
            }
        }
    }

    private IEnumerator IE_TweenDamage()
    {
        transform.DOPunchScale(new Vector3(-0.05f, -0.05f, 0f), 0.2f);

        m_spriteRenderer.DOColor(Color.red, 0.25f);

        yield return new WaitForSeconds(0.25f);

        transform.DOPunchScale(new Vector3(0.05f, 0.05f, 0f), 0.2f);

        m_spriteRenderer.DOColor(m_originalColor, 0.25f);
    }


    public void makeMinions(int num)
    {
        m_animator.SetTrigger("generateSkill");
        for (int i = 0; i < num; i++)
        {
            GameObject temp = Instantiate(minions, transform);
            temp.GetComponent<BaseMonster>().init();
        }
        StartCoroutine(Rest());
    }




    protected override void Patrol()
    {
    }

    protected override void Pursuit()
    {
    }

    protected override void Attack()
    {

    }

    public void EndWalkingAnimation()
    {
        m_animator.SetBool(Walking, false);
        
    }



    public void ExecuteDeadAfterAnimation()
    {
        DeadListener?.Invoke();
        Destroy(gameObject);
    }

    void TowardPlayer()
    {
        m_renderer.flipX = (m_playerObj.transform.position.x - transform.position.x) > 0;
    }

}