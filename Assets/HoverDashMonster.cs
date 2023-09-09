using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HoverDashMonster : HoverMonster
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
                    TransitionToState(MonsterState.Pursuit);
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

    protected override void Pursuit()
    {
            if (m_isDashing == false)
            {
                dashDirection = (base.m_playerObj.transform.position - transform.position).normalized;

                StartCoroutine(nameof(IE_Dash));
            }
    }

    private IEnumerator IE_Dash()
    {
        m_originalSpeed = base.m_agent.speed;
        m_isDashing = true;
        base.m_rb.AddForce(dashDirection * m_maxDashForce);
        m_isDashing = false;


        yield return new WaitForSeconds(m_dashCoolTime);
        base.m_rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1f);

    }

    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    #endregion
}