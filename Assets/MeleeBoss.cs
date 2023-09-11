using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MeleeBoss : BaseMonster
{
    enum Pattern
    {
        Dash,
        Shield,
        Rest
    }
    public GameObject Bullet;
    protected Vector3 m_targetPos;


    private Pattern m_currentPattern;
    private SpriteRenderer m_renderer;

    private int m_rushSpeed;
    public float attackRange;

    private bool m_onAction;

    [Header("Ranged Boss Skill")]
    private static readonly int Walking = Animator.StringToHash("isWalking");
    private static readonly int AttackSkill = Animator.StringToHash("attackSkill");
    private static readonly int ShieldSkill = Animator.StringToHash("shieldSkill");

    public bool isInvincible = false;

    public void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();
        m_renderer = GetComponentInChildren<SpriteRenderer>();
        m_animator = GetComponentInChildren<Animator>();
        m_onAction = false;

    }

    protected override void stateUpdate()
    {
        if (!m_onAction)
        {
            print(m_currentPattern);
            if (Vector2.Distance(m_playerObj.transform.position, transform.position) < attackRange)
            {
                m_animator.SetBool(AttackSkill, true);
                m_onAction = true;
                StartCoroutine(Rest());

            }
            else
            {
                m_animator.SetBool(AttackSkill, false);
            }

            switch (m_currentPattern)
            {
                case Pattern.Dash:
                    m_onAction = true;
                    StartCoroutine(MoveToPlayer(m_playerObj.transform.position));
                    break;
                case Pattern.Shield:
                    m_onAction = true;
                    StartCoroutine(IEInvincible());
                    break;
                case Pattern.Rest:
                    m_onAction = true;
                    StartCoroutine(Rest());
                    break;
            }
        }
    }

    IEnumerator IEEnd()
    {
        EndAttackAnimation();
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
            transform.position = Vector2.MoveTowards(transform.position, lastPlayerPosition, m_speed * Time.deltaTime);
            timer += Time.deltaTime;
        }
        yield return IEEnd();
    }

    IEnumerator Rest()
    {
        m_animator.SetBool(Walking, false);
        float timer = 0f;
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
        else
        {
            reflectObject(collision);
        }
    }



    void reflectObject(Collision2D collision)
    {
        if (isInvincible)
        {
            Rigidbody2D rb = collision.gameObject.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 normal = collision.contacts[0].normal;
                Vector2 reflection = Vector2.Reflect(rb.velocity, normal).normalized;
                Destroy(rb.gameObject);

                GameObject newBullet = Instantiate(Bullet, transform.position, Quaternion.identity);
                Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();

                if (bulletRb != null)
                {
                    bulletRb.velocity = reflection * 5;
                }
            }
        }
    }



    public override void getDamage(float _damage)
    {
        if (isInvincible == false)
        {
            Health -= _damage;
            if (Health <= 0)
            {
                if (isBoss)
                {
                    Dead();
                }
                else
                {
                    StartCoroutine(nameof(IE_PlayDyingEffect));
                }
            }
        }
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

    public void EndAttackAnimation()
    {
        m_animator.SetBool(Walking, false);
        m_animator.SetBool(AttackSkill, false);
        m_animator.SetBool(ShieldSkill, false);
    }



    public void ExecuteDeadAfterAnimation()
    {
        base.Dead();
    }

    void TowardPlayer()
    {
        m_renderer.flipX = (m_playerObj.transform.position.x - transform.position.x) > 0;
    }

}