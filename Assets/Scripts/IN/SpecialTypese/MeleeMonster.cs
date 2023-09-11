using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MeleeMonster : BaseMonster
{
    #region PublicVariables

    #endregion
    [SerializeField] private bool m_isHovering;
    #region PrivateVariables

    protected override void stateUpdate()
    {
        switch (m_currentState)
        {
            case MonsterState.Patrol:
                setAnimationDir(targetPatrolPos);
                if (canSeePlayer() && playerWithinRange())
                {
                    TransitionToState(MonsterState.Pursuit);
                }
                Patrol();
                break;
            case MonsterState.Pursuit:
                setAnimationDir(base.m_playerObj.transform.position);
                if (!canSeePlayer() || !playerWithinRange())
                {
                    Patrol();
                }
                else
                {
                    Pursuit();
                }
                break;
            case MonsterState.Knockback:
                setAnimationDir(base.m_playerObj.transform.position);
                print("Knockback");
                break;
        }
    }

    protected virtual void setAnimationDir(Vector3 pos)
    {
        if (!m_isHovering)
        {
            base.m_animator.SetFloat("X", (pos - (Vector3)transform.position).x);
            base.m_animator.SetFloat("Y", (pos - (Vector3)transform.position).y);
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
            base.m_patrolTimer -= Time.deltaTime;
            if (base.m_patrolTimer < 0)
            {
                base.m_patrolTimer = m_patrolTime;
                targetPatrolPos = getPatrolPos();
            }
        }
    }

    protected override void Attack()
    {
    }


    protected override bool playerWithinRange()
    {
        if (Vector2.Distance(transform.position, m_playerObj.transform.position) < m_range)
        {
            return true;
        } else { return false; }
    }

    protected override bool canSeePlayer()
    {
        if (m_isHovering == false)
        {
            return (raycastPlayer().CompareTag("Player")) ? true : false;
        }
        else
            return true;
    }


    #endregion

    #region PrivateMethod
    #endregion

}