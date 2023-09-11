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
    private bool m_onSkill;
    private bool m_restMode;
    private bool m_isMove;
    private Pattern m_currentPattern;
    private SpriteRenderer m_renderer;
    private int m_rushSpeed;
    public float attackRange;
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
        m_onSkill = false;
        m_restMode = true;
    }

    protected override void stateUpdate()
    {
        if (m_onSkill == false)
        {

            if (Vector2.Distance(m_playerObj.transform.position, transform.position) < attackRange)
            {
                m_animator.SetBool(AttackSkill, true);
            }
            Debug.Log(m_currentPattern.ToString());
            print(m_currentPattern);
            switch (m_currentPattern)
            {
                case Pattern.Dash:
                    StartCoroutine(RushToPlayer(5, m_playerObj.transform.position));
                    break;
                case Pattern.Shield:
                    StartCoroutine(IEInvincible(5));
                    //                    m_animator.SetBool(ShieldSkill, true);
                    break;
                case Pattern.Rest:
                    StartCoroutine(IEEnd(5));
                    break;
            }
        }
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        if (isInvincible)
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
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



    IEnumerator IEEnd(float second)
    {
        m_currentPattern = (Pattern)UnityEngine.Random.Range(0, 1);
        m_onSkill = false;
        m_restMode = false;
        yield return new WaitForSeconds(second);
    }

    IEnumerator IEInvincible(float second)
    {
        m_onSkill = true;
        TowardPlayer();
        m_renderer.color = Color.blue;
        isInvincible = true;
        yield return new WaitForSeconds(second);
        isInvincible = false;
        yield return IEEnd(second);
        m_renderer.color = Color.white;
        EndAttackAnimation();
        m_onSkill = false;
    }



    IEnumerator RushToPlayer(int second, Vector2 lastPlayerPosition)
    {
        m_onSkill = true;
        TowardPlayer();
        m_animator.SetBool(Walking, true);

        float timer = 0f;

        while (Vector2.Distance(transform.position, lastPlayerPosition) >= 3f && timer < second)
        {
            transform.position = Vector2.MoveTowards(transform.position, lastPlayerPosition, m_speed * Time.deltaTime);
            timer = Time.deltaTime;
            yield return null;
        }
        yield return IEEnd(second);
        m_currentPattern = Pattern.Rest;
        EndAttackAnimation();
        m_onSkill = false;
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
        m_onSkill = false;
        m_restMode = true;
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