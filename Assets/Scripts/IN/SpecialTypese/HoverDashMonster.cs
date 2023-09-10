using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HoverDashMonster : MeleeDashMonster
{
    [SerializeField] private float m_dashStartDistance;
    private float m_originalSpeed;
    [SerializeField]private float m_maxDashForce;
    private bool m_isDashing = false;
    [SerializeField]private float m_dashDuration;
    [SerializeField] private float m_dashCoolTime;
    Vector2 dashDirection;
    #region PublicVariables
    #endregion

    #region PrivateVariables

    protected override void stateUpdate()
    {
        switch (m_currentState)
        {
            case MonsterState.Patrol:
                base.m_animator.SetFloat("X", (targetPatrolPos - (Vector3)transform.position).x);
                base.m_animator.SetFloat("Y", (targetPatrolPos - (Vector3)transform.position).y);
                if (playerWithinRange())
                {
                    TransitionToState(MonsterState.Pursuit);
                }
                Patrol();
                break;
            case MonsterState.Pursuit:
                base.m_animator.SetFloat("X", (base.m_playerObj.transform.position - (Vector3)transform.position).x);
                base.m_animator.SetFloat("Y", (base.m_playerObj.transform.position - (Vector3)transform.position).y);
                if (!playerWithinRange())
                {
                    Patrol();
                }
                else
                {
                    Pursuit();
                }
                break;
            case MonsterState.Knockback:
                base.m_animator.SetFloat("X", (base.m_playerObj.transform.position - (Vector3)transform.position).x);
                base.m_animator.SetFloat("Y", (base.m_playerObj.transform.position - (Vector3)transform.position).y);
                print("Knockback");
                break;
        }
    }

    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    #endregion
}