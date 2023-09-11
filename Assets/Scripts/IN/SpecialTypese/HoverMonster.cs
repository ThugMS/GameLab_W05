using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HoverMonster : BaseMonster
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    protected Vector3 m_targetPos;
    #endregion

    #region PublicMethod

    public override void init()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_originalColor = m_spriteRenderer.color;

        m_playerObj = GameObject.FindGameObjectWithTag("Player");
        m_rb = GetComponent<Rigidbody2D>();
        targetPatrolPos = transform.position;
        m_knockbackTimer = m_knockbackTime;
        damageRefreshTimer = 0.1f;
        m_patrolTimer = m_patrolTime;
        targetPatrolPos = getPatrolPos();
        m_agent.speed = m_speed;
        isOn = true;
    }


    private void Start()
    {
        init();
    }

    protected override void stateUpdate()
    {
        switch (m_currentState)
        {
            case MonsterState.Patrol:
                if (playerWithinRange())
                {
                    TransitionToState(MonsterState.Pursuit);
                }
                Patrol();
                break;
            case MonsterState.Pursuit:
                if (!playerWithinRange())
                {
                    Patrol();
                }
                else
                {
                    Pursuit();
                }
                break;
        }
    }

    //==========================NavMesh를 사용하지 않으므로, 관련 부분 오버라이딩========추후 논의 후 그냥 근접을 Wall  2개로 교체해도 될듯==========
    protected override void Patrol()
    {

        if (Vector2.Distance(transform.position, base.targetPatrolPos) < 0.2f)
        {
            base.m_patrolTimer -= Time.deltaTime;
            if (m_patrolTimer < 0)
            {
                m_patrolTimer = m_patrolTime;
                targetPatrolPos = base.getPatrolPos();
            }
        }
        m_targetPos = new Vector2(UnityEngine.Random.Range(m_initialPosition.x - base.m_range / 2, m_initialPosition.x + base.m_range / 2),
                UnityEngine.Random.Range(m_initialPosition.y - base.m_range / 2, m_initialPosition.y + base.m_range / 2));
    }

    protected override void Pursuit()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_playerObj.transform.position, m_speed * Time.deltaTime);
    }

    protected override IEnumerator IE_KnockBack(float knockbackPower)
    {
        TransitionToState(MonsterState.Knockback);

        Vector2 moveDirection = (transform.position - m_playerObj.transform.position).normalized;
        Vector2 knockbackEndPosition = (Vector2)transform.position + moveDirection * knockbackPower;
        m_knockbackTimer = m_knockbackTime;
        m_rb.velocity = moveDirection * knockbackPower;
        yield return new WaitForSeconds(m_knockbackTime);
        m_rb.velocity = Vector2.zero;
        TransitionToState(MonsterState.Patrol);
        isAttacked = false;
    }


    protected override void TransitionToState(MonsterState newState)
    {
        base.m_currentState = newState;
    }


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        base.DamagePlayer(collision.gameObject);
    }

    protected override void Attack()
    {
    }

    #endregion

    #region PrivateMethod
    #endregion
}