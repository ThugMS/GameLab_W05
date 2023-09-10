using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverDashMonster2 : HoverMonster
{
    #region PublicVariables
    [SerializeField] private bool m_isDashing = false;
    [SerializeField] private float m_dashForce = 1000.0f;
    [SerializeField] private float m_dashDuration = 1.5f;
    [SerializeField] private float m_dashCoolTime = 3.0f;
    #endregion

    private Rigidbody m_rigidbody;
    private Coroutine m_dashCoroutine;

    #region PublicMethod
    protected override void Pursuit()
    {
        if (!m_isDashing)
        {
            StartDash();
        }
    }
    #endregion

    #region PrivateMethod
    private void StartDash()
    {
        m_isDashing = true;


        Vector3 dashDirection = (base.m_playerObj.transform.position - transform.position).normalized;

        m_rigidbody.AddForce(dashDirection * m_dashForce, ForceMode.Impulse);

        m_dashCoroutine = StartCoroutine(DashCooldown());
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(m_dashDuration);

        m_rigidbody.velocity = Vector3.zero;


        yield return new WaitForSeconds(m_dashCoolTime);

        m_isDashing = false;
    }

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }
    #endregion
}