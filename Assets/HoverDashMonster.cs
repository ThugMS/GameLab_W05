using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverDashMonster : HoverMonster
{
    [SerializeField] private float m_dashStartDistance;
    private float m_originalSpeed;
    [SerializeField] private bool m_isDashing;
    [SerializeField] private float m_dashSpeed;
    [SerializeField] private float m_maxDashDistance;
    [SerializeField] private float m_dashDuration;
    [SerializeField] private float m_dashCoolTime;
    #region PublicVariables
    #endregion

    #region PrivateVariables
    protected override void Pursuit()
    {
        if (Vector2.Distance(transform.position, m_playerObj.transform.position) > m_dashStartDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, m_playerObj.transform.position, m_speed * Time.deltaTime);
        }
        else
        {
            StartCoroutine(nameof(IE_Dash));
        }
    }

    private IEnumerator IE_Dash()
    {
        m_originalSpeed = base.m_agent.speed;
        m_isDashing = true;

        base.m_animator.SetBool("isDashing", true);

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
        base.m_animator.SetBool("isDashing", false);

        yield return new WaitForSeconds(m_dashCoolTime);
        base.m_agent.isStopped = false;
        m_isDashing = false;

    }

    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    #endregion
}