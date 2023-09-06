using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class Archer : Player
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [Header("Attack")]
    [SerializeField] protected bool m_isReadyArrow = false;
    [SerializeField] protected bool m_canArrow = false;
    [SerializeField] protected float m_arrowInitSpeed = 5f;
    [SerializeField] protected float m_arrowCurSpeed = 5f;
    [SerializeField] protected float m_arrowMaxSpeed = 10f;
    [SerializeField] protected float m_arrpowaddSpeed = 2f;
    
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
            Debug.Log("end");
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
        
    }

    private IEnumerator ReadyArrowTime()
    {
        yield return null;
    }
    #endregion

}
