using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public abstract class Player : MonoBehaviour
{
    public enum PlayerClassType
    {
        Knight,
        Archer,
        Wizard
    }
    
    #region PublicVariables
    public enum ANIMATION_DIRECTION
    { Up, Right, Down, Left}
    #endregion

    #region PrivateVariables
    [SerializeField] protected Rigidbody2D m_rigidbody;
    
    [FormerlySerializedAs("m_heart")]
    [Header("Status")]
    [SerializeField] protected float m_currentHP = 12f;
    [SerializeField] protected float m_power;
    [SerializeField] protected float m_offset = 0.5f;
    [SerializeField] protected float m_maxHP = 20f;

    [Header("Move")]
    [SerializeField] protected float m_maxSpeed = 5f;
    [SerializeField] protected float m_accelSpeed = 0.5f;
    [SerializeField] protected float m_decelSpeed = 1.0f;
    [SerializeField] protected float m_curSpeed = 0f;
    [SerializeField] protected Vector2 m_inputDirection = Vector2.zero;
    [SerializeField] protected Vector2 m_Direction = Vector2.down;
    [SerializeField] protected bool m_isMove = false;
    [SerializeField] protected bool m_canMove = true;

    [Header("Action")]
    [SerializeField] protected bool m_canAct = true;
    [SerializeField] protected bool m_isAct = false;

    [Header("Animation")]
    [SerializeField] protected Animator m_animator;
    
    [Header("Type")]
    protected PlayerClassType MPlayerClassType;
    #endregion

    #region PublicMethod
    public void OnMovement(InputAction.CallbackContext _context)
    {   
        m_inputDirection = _context.ReadValue<Vector2>();

        if (m_inputDirection == Vector2.zero)
        {
            SetIsMove(false);
        }
        else
        {
            if (m_isAct == false)
            {
                SetIsMove(true);
            }
        
            m_Direction = m_inputDirection;
        }
    }

    public virtual void OnAttack(InputAction.CallbackContext _context)
    {
        if(_context.started == false)
        {
            return;
        }

        Attack();
    }

    public void OnAbility(InputAction.CallbackContext _context)
    {
        if (_context.started == false)
        {
            return;
        }

        Ability();
    }
    
    public void GetDamage(float _damage)
    {
        m_currentHP -= _damage;
        
        if(m_currentHP > m_maxHP)
        {
            m_currentHP = m_maxHP;
        }

        UIManager.Instance.DecreaseHeart(m_currentHP, m_maxHP);

        if(m_currentHP <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        UIManager.Instance.PlayGameOverEffect();
    }

    public void SetCanMove(bool _value)
    {
        m_canMove = _value;
    }

    public void SetCanAct(bool _value)
    {
        m_canAct = _value;
    }

    protected virtual void Awake()
    {
        TryGetComponent<Rigidbody2D>(out m_rigidbody);
    }

    protected virtual void Start()
    {
        SetStatus();
        UIManager.Instance.SetHeartUI(m_currentHP, m_maxHP);
    }

    protected virtual void FixedUpdate()
    {
        #region Animation
        if(m_isMove == true)
        {
            m_animator.SetBool("IsMove", true);
            m_animator.SetFloat("XDir", m_Direction.x);
            m_animator.SetFloat("YDir", m_Direction.y);
        }
        else
        {   if(m_canMove == true)
            {
                m_animator.SetBool("IsMove", false);
                m_animator.SetFloat("XDir", m_Direction.x);
                m_animator.SetFloat("YDir", m_Direction.y);
            }
        }
        #endregion

        #region Move
        if (m_isMove == true && m_canMove == true)
        {
            Move(1);
        }
        else
        {
            Move(-1);
        }
        #endregion
    }

    protected abstract void SetStatus();

    protected abstract void Attack();

    protected abstract void Ability();
    #endregion

    #region PrivateMethod
    private void Move(int _arrow)
    {
        SetMoveSpeed(_arrow);

        Vector2 moveAmount = m_inputDirection.normalized * m_curSpeed * Time.deltaTime;
        Vector2 nextPosition = m_rigidbody.position + moveAmount;

        m_rigidbody.MovePosition(nextPosition);
    }

    private void SetMoveSpeed(int _arrow)
    {
        if (m_curSpeed <= m_maxSpeed)
        {
            if (_arrow < 0)
            {
                m_curSpeed -= m_decelSpeed;
            }
            else
            {
                m_curSpeed += m_accelSpeed;
            }
        }

        if (m_curSpeed < 0)
        {
            m_curSpeed = 0;
        }

        if (m_curSpeed > m_maxSpeed)
        {
            m_curSpeed = m_maxSpeed;
        }
    }

    private void SetIsMove(bool _value)
    {
        m_isMove= _value;
    }

    private bool GetIsMove()
    {
        return m_isMove;
    }

    private void OnEnable()
    {
         var input = GetComponent<PlayerInput>();
         
         if(input != null)
         {
             UIManager.Instance.SetPlayerInput(input);
         }
    }

    protected void SetCharType(PlayerClassType playerClassType)
    {
        MPlayerClassType = playerClassType;
        
        UIManager.Instance.SetSKillSlot(MPlayerClassType);
    }

    #endregion
}
