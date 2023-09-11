using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public enum PlayerClassType
{
    Knight,
    Archer,
    Wizard,
    None
}

public abstract class Player : MonoBehaviour
{
    #region PublicVariables
    public enum ANIMATION_DIRECTION
    { Up, Right, Down, Left}

    [Header("Editor")]
    public bool m_isGod = false;

    public bool m_isAttacked = false;
    #endregion

    #region PrivateVariables
    [SerializeField] protected Rigidbody2D m_rigidbody;
    [SerializeField] protected bool m_blink = false;

    [FormerlySerializedAs("m_heart")]
    [Header("Status")]
    [SerializeField] protected float m_currentHP = 12f;
    [SerializeField] protected float m_power;
    [SerializeField] protected float m_offset = 0.5f;
    [SerializeField] protected float m_maxHP = 20f;
    [SerializeField] protected float m_attackedTime = 1f;

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
    [SerializeField] protected float m_coolTime;

    [Header("Animation")]
    [SerializeField] protected Animator m_animator;

    [Header("PlusStatus")] 
    protected float m_plusHP;
    protected float m_plusPower;
    protected float m_plusSpeed;
    
    public float FinalHP => m_maxHP + m_plusHP;
    public float FinalPower => m_power + m_plusPower;
    public float FinalSpeed => m_maxSpeed + m_plusSpeed;
    
    protected Gem m_currentTriggerGem;
    
    [Header("Type")] 
    protected PlayerClassType m_PlayerClassType;
    protected GemType m_currentGemType = GemType.None;
    public GemType CurrentGemType => m_currentGemType;



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
        if (m_isGod == true)
            return;

        if (m_isAttacked == true)
            return;

        m_isAttacked = true;
        StartCoroutine(nameof(IE_ResetAttackedTime));

        m_currentHP -= _damage;
        
        if(m_currentHP > FinalHP)
        {
            m_currentHP = FinalHP;
        }

        UIManager.Instance.DecreaseHeart(m_currentHP, FinalHP);

        if(m_currentHP <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {   
        SetCanAct(false);
        SetCanMove(false);
        m_isMove = false;
        m_isAct = false;

        m_animator.SetTrigger("Dead");
    }

    public void SetCanMove(bool _value)
    {
        m_canMove = _value;
    }

    public void SetCanAct(bool _value)
    {
        m_canAct = _value;
    }

    public bool IsSameGem(GemType gemType)
    {
        return m_currentGemType == gemType;
    }

    public void OnGemTriggerEnter(Gem gem)
    {
        if (IsSameGem(gem.GemType))
            return;
        
        m_currentTriggerGem = gem;
        
        if(m_currentGemType == GemType.None)
            ChangeGem(gem.GemType);
        else
        {
            UIManager.Instance.OpenGemPanel(this, gem);
        }
    }

    public void ChangeGem(GemType gemType)
    {
        if (IsSameGem(gemType))
            return;
        
        if (gemType != GemType.None)
        {
            ChangePlusStatus(gemType);
        }
        else
        {
            AbsorbStatus();
        }
        
        m_currentGemType = gemType;

        Destroy(m_currentTriggerGem.gameObject);
        m_currentTriggerGem = null;
        
        UIManager.Instance.OnGemChanged(this);
    }

    protected void ChangePlusStatus(GemType gemType)
    {
        ResetPlusStatus();
        
        switch (gemType)
        {
            case GemType.Power:
                m_plusPower += 3;
                break;
            case GemType.HP:
                m_plusHP += 4;
                break;
            case GemType.Speed:
                m_plusSpeed += 3;
                break;
        }
        
        OnStatusChanged();
    }

    protected virtual void Update()
    {
        if(m_isAttacked == true)
        {   
            if(m_blink == false)
            {
                transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
            }
            else
            {

                transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 1f);

                
            }
            m_blink = !m_blink;
        }
        if(m_isAttacked == false)
        {
            transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        }
    }

    protected void ResetPlusStatus()
    {
        m_plusHP = 0;
        m_plusPower = 0;
        m_plusSpeed = 0;
        
        OnStatusChanged();
    }
    
    protected void AbsorbStatus()
    {
        m_maxHP += m_plusHP;
        m_power += m_plusPower;
        m_maxSpeed += m_plusSpeed;

        ResetPlusStatus();
        OnStatusChanged();
    }
    
    protected virtual void Awake()
    {
        TryGetComponent<Rigidbody2D>(out m_rigidbody);
    }

    protected virtual void Start()
    {
        PlayerManager.instance.SetPlayer(gameObject);
        SetStatus();
        UIManager.Instance.SetHeartUI(m_currentHP, FinalHP);
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

    public void InitSetting(PlayerData _data)
    {
        m_power = _data.Power;
        m_maxHP = _data.Health;
        m_currentHP = _data.Health;
        m_maxSpeed = _data.Speed;
        m_coolTime = _data.CoolTime;
    }

    protected abstract void SetStatus();

    protected abstract void Attack();

    protected abstract void Ability();
#endregion

    #region PrivateMethod
    private void Move(int _arrow)
    {
        SetMoveSpeed(_arrow);

        Vector2 moveAmount = m_inputDirection.normalized * (m_curSpeed * Time.deltaTime);
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
         SetPlayerInput();
         OnStatusChanged();
    }

    private void SetPlayerInput()
    {
        var input = GetComponent<PlayerInput>();
         
        if(input != null)
        {
            UIManager.Instance.SetPlayerInput(input);
        }
    }

    private IEnumerator IE_ResetAttackedTime()
    {
        yield return new WaitForSeconds(m_attackedTime);

        m_isAttacked = false;
    }

    protected void SetPlayerClassType(PlayerClassType playerClassType)
    {
        m_PlayerClassType = playerClassType;
        
        UIManager.Instance.SetSKillSlot(m_PlayerClassType);
        UIManager.Instance.SetProfileImage(m_PlayerClassType);
        
    }

    // TODO : 스탯 변할 때 마다 호출
    protected void OnStatusChanged()
    {
        UIManager.Instance.SetProfileStatus(m_maxHP, m_power, m_maxSpeed, m_plusHP, m_plusPower, m_plusSpeed);
        UIManager.Instance.SetHeartUI(m_currentHP, FinalHP);
    }


    #endregion
}
