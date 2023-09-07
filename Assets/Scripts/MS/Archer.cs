using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
 
public class Archer : Player
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [Header("Arrow")]
    [SerializeField] protected GameObject m_arrow;

    [Header("Attack")]
    [SerializeField] protected bool m_isReadyArrow = false;
    [SerializeField] protected bool m_canArrow = false;
    [SerializeField] protected float m_arrowInitSpeed = 5f;
    [SerializeField] protected float m_arrowCurSpeed = 5f;
    [SerializeField] protected float m_arrowMaxSpeed = 20f;
    [SerializeField] protected float m_arrowAddSpeed = 0.1f;
    [SerializeField] protected float m_minReadyTime = 0.5f;
    [SerializeField] protected float m_arrowInitPower = 2f;
    [SerializeField] protected float m_arrowCurPower = 2f;
    [SerializeField] protected float m_arrowAddPower = 0.1f;
    [SerializeField] protected float m_arrowMaxPower = 5f;

    #endregion

    #region 
    public override void OnAttack(InputAction.CallbackContext _context)
    {
        if (_context.started == true)
        {
            m_isReadyArrow = true;
            m_canMove = false;
            ReadyArrow();
        }

        if(_context.canceled == true)
        {
            m_canMove = true;
            ShootArrow();
            ResetArrowStat();
        }
    }
    protected override void SetStatus()
    {
        m_power = 3;
    }
    

    protected override void Attack()
    {
        ReadyArrow();
    }

    protected override void Ability()
    {

    }
    #endregion

    #region PrivateMethod
    private void ReadyArrow()
    {
        StartCoroutine(nameof(IE_ReadyArrowTime));
    }

    private void ShootArrow()
    {   
        if(m_canArrow == false)
        {
            return;
        }
            
        StopCoroutine(nameof(IE_ReadyArrowTime));

        float angle = Vector2.SignedAngle(Vector2.up, m_Direction.normalized);
        Debug.Log(angle);

        GameObject arrow = Instantiate(m_arrow, transform.position, Quaternion.Euler(0, 0, angle), transform);
        arrow.GetComponent<Arrow>().InitSetting(m_arrowCurSpeed, m_Direction.normalized);
    }

    private void ResetArrowStat()
    {
        m_arrowCurSpeed = m_arrowInitSpeed;
        m_arrowCurPower = m_arrowInitPower;
        m_canArrow = false;
    }

    private IEnumerator IE_ReadyArrowTime()
    {
        yield return new WaitForSeconds(m_minReadyTime);
        m_canArrow = true;

        while (true)
        {
            m_arrowCurSpeed += m_arrowAddSpeed;
            m_arrowCurPower += m_arrowAddPower;

            if(m_arrowCurPower >= m_arrowMaxPower)
            {
                m_arrowCurPower = m_arrowMaxPower;
            }

            if(m_arrowCurSpeed >= m_arrowMaxSpeed)
            {
                m_arrowCurSpeed = m_arrowMaxSpeed;
            }

            yield return null;
        }
    }
    #endregion

}
