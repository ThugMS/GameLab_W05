using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MeleeMonster : BaseMonster
{
    #region PublicVariables

    #endregion

    #region PrivateVariables
    protected virtual void Start()
    {
        init();
    }

    protected virtual void Update()
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
    #endregion

    #region PublicMethod
    public override void Pursuit()
    {
        m_agent.SetDestination(m_playerObj.transform.position);
    }

    public override void Patrol()
    {
        m_agent.ResetPath();
        m_agent.SetDestination(targetPatrolPos);
        if (Vector2.Distance(transform.position, targetPatrolPos) < 0.2f)
        {
            m_timer -= Time.deltaTime;
            if (m_timer < 0)
            {
                m_timer = m_patrolTime;
                targetPatrolPos = getPatrolPos();
            }
        }
    }

    public override void Attack()
    {
    }
    #endregion

    #region PrivateMethod
    #endregion

}