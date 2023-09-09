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

    protected override void stateUpdate()
    {


        switch (m_currentState)
        {
            case MonsterState.Patrol:
                base.m_animator.SetFloat("X", (targetPatrolPos - (Vector3)transform.position).x);
                base.m_animator.SetFloat("Y", (targetPatrolPos - (Vector3)transform.position).y);
                if (canSeePlayer() && playerWithinRange())
                {
                    TransitionToState(MonsterState.Pursuit);
                }
                Patrol();
                break;
            case MonsterState.Pursuit:
                base.m_animator.SetFloat("X", (base.m_playerObj.transform.position - (Vector3)transform.position).x);
                base.m_animator.SetFloat("Y", (base.m_playerObj.transform.position - (Vector3)transform.position).y);
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
    protected override void Pursuit()
    {
        m_agent.SetDestination(m_playerObj.transform.position);
    }

    protected override void Patrol()
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




    protected override void Attack()
    {
    }
    #endregion

    #region PrivateMethod
    #endregion

}