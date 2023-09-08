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

    public override void Patrol()
    {

        if (Vector2.Distance(transform.position, base.targetPatrolPos) < 0.2f)
        {
            base.m_timer -= Time.deltaTime;
            if (m_timer < 0)
            {
                m_timer = m_patrolTime;
                targetPatrolPos = base.getPatrolPos();
            }
        }
        m_targetPos = new Vector2(UnityEngine.Random.Range(m_initialPosition.x - base.m_range / 2, m_initialPosition.x + base.m_range / 2),
                UnityEngine.Random.Range(m_initialPosition.y - base.m_range / 2, m_initialPosition.y + base.m_range / 2));
    }

    public override void Pursuit()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_playerObj.transform.position, m_speed * Time.deltaTime);
    }

    protected override IEnumerator IE_KnockBack()
    {
        yield return new WaitForSeconds(base.m_knockBackTime);
        base.isAttacked = false;
        TransitionToState(MonsterState.Patrol);
        yield return null;
    }

    protected override void TransitionToState(MonsterState newState)
    {
        base.m_currentState = newState;
    }


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        base.DamagePlayer(collision.gameObject);
    }

    public override void Attack()
    {
    }

    #endregion

    #region PrivateMethod
    #endregion
}