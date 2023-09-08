using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Player : MonoBehaviour
{
    #region PublicVariables
    
    #endregion

    #region PrivateVariables
    [SerializeField] protected Rigidbody2D m_rigidbody;

    [Header("Status")]
    [SerializeField] protected float m_heart = 3f;
    [SerializeField] protected float m_power;
    [SerializeField] protected float m_offset = 0.5f;

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
            SetIsMove(true);
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
        m_heart -= _damage;

        UIManager.Instance.DecreaseLife();

        if(m_heart <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {

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
    }

    protected virtual void FixedUpdate()
    {
        #region Move
        if(m_isMove == true && m_canMove == true)
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
    #endregion
}
