using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeDashMonster : MeleeMonster
{
    #region PublicVariables
    [SerializeField] private bool m_isDashing = false;
    [SerializeField] private float m_originalSpeed;
    [SerializeField] private float m_dashSpeed = 10.0f;
    [SerializeField] private float m_dashDuration = 1.5f;
    [SerializeField] private float m_maxDashDistance = 10.0f;
    [SerializeField] private float m_dashCoolTime = 3.0f;
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    public override void Pursuit()
    {
        if (m_isDashing == false)
        {
            StartCoroutine(Dash());
        }
    }


    #endregion

    #region PrivateMethod

    private IEnumerator Dash()
    {
        m_originalSpeed = base.m_agent.speed;
        m_isDashing = true;

        base.m_agent.speed = m_dashSpeed;

        RaycastHit hit;
        Vector3 dashDirection = (base.m_playerObj.transform.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, dashDirection, out hit, m_maxDashDistance))
        {
            m_maxDashDistance = hit.distance;
        }

        Vector3 dashDestination = transform.position + dashDirection * m_maxDashDistance;

        base.m_agent.SetDestination(dashDestination);

        yield return new WaitForSeconds(m_dashDuration);

        base.m_agent.speed = m_originalSpeed;
        base.m_agent.isStopped = true;
        yield return new WaitForSeconds(m_dashCoolTime);
        base.m_agent.isStopped = false;
        m_isDashing = false;


    }
    #endregion
}