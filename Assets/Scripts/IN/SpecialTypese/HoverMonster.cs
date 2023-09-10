using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverMonster : BaseMonster
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    protected Vector3 m_targetPos;
    #endregion

    #region PublicMethod
    protected virtual void Start()
    {
        base.init();
    }

    protected override void stateUpdate()
    {
        switch (m_currentState)
        {
            case MonsterState.Patrol:
                if (canSeePlayer() && playerWithinRange())
                {
                    TransitionToState(MonsterState.Pursuit);
                }
                Patrol();
                break;
            case MonsterState.Pursuit:
                if (!canSeePlayer() && playerWithinRange())
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

    //==========================NavMesh�� ������� �����Ƿ�, ���� �κ� �������̵�========���� ���� �� �׳� ������ Wall  2���� ��ü�ص� �ɵ�==========
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