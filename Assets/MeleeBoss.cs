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
        Shield
    }

    protected Vector3 m_targetPos;
    private bool m_onSkill;
    private bool m_restMode;
    private bool m_isMove;
    private Pattern m_currentPattern;
    private SpriteRenderer m_renderer;
    private int m_rushSpeed;
    [Header("Ranged Boss Skill")]
    private static readonly int DashSkill = Animator.StringToHash("dashSkill");
    private static readonly int AttackSkill = Animator.StringToHash("attackSkill");
    private static readonly int ShieldSkill = Animator.StringToHash("shieldSkill");

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

        if (!m_onSkill)
        {
            m_onSkill = true;
            if (m_restMode)
            {
                StartCoroutine(nameof(IERest));
            }
            else
            {
                Debug.Log(m_currentPattern.ToString());
                switch (m_currentPattern)
                {
                    case Pattern.Dash:
                        StartCoroutine(RushToPlayer(5));
                        break;
                    case Pattern.Shield:
                        m_animator.SetBool(ShieldSkill, true);
                        break;
                }
            }
        }

        TowardPlayer();
        if (m_isMove == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, m_playerObj.transform.position, m_speed * Time.deltaTime);
        }
    }

    IEnumerator IERest()
    {
        m_animator.Play("RangedBossIdle");

        m_currentPattern = (Pattern)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Pattern)).Length);
        yield return new WaitForSeconds(1f);

        // 1초간 추적
        m_animator.SetBool(DashSkill, true);
        yield return IEPatrol(1);
        m_animator.SetBool(DashSkill, false);

        m_onSkill = false;
        m_restMode = false;
    }

    IEnumerator IEPatrol(int second, bool isRush = false)
    {
        float timer = second;
        m_isMove = true;
        yield return new WaitForSeconds(second);
        m_isMove = false;
        yield return null;
    }

    IEnumerator RushToPlayer(int second)
    {
        Debug.Log("Rush Start");
        int backup_Speed = m_speed;
        m_speed = m_rushSpeed;

        m_animator.SetBool(DashSkill, true);
        yield return IEPatrol(second);
        m_speed = backup_Speed;
        EndAttackAnimation();

        Debug.Log("Rush End");
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
        m_animator.SetBool(DashSkill, false);
        m_animator.SetBool(AttackSkill, false);
        m_animator.SetBool(ShieldSkill, false);
        m_onSkill = false;
        m_restMode = true;
    }

    public override void getDamage(float _damage)
    {
        m_animator.Play("RangedBossGetHit");
        Health -= _damage;

        if (Health <= 0)
        {
            // [TODO] 공격 추가?
            m_animator.Play("RangedBossDead");
            //이 후 죽음은 애니메이션 재생 후, OnStateExit()에서 ExecuteDeadAfterAnimation() 호출하여 종료
        }
    }

    public void ExecuteDeadAfterAnimation()
    {
        base.Dead();
    }

    void TowardPlayer()
    {
        m_renderer.flipX = (m_playerObj.transform.position.x - transform.position.x) < 0;
    }

}