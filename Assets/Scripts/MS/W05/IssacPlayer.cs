using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class IssacPlayer : Player
{
    #region PublicVariables
    public bool m_canAttack = true;

    
    #endregion

    #region PrivateVariables
    private bool m_isAttackPressed = false;

    private bool m_canChangeClass;
    private PlayerClassType m_selectType = PlayerClassType.None;
    [Header("Class")]
    [SerializeField] private GameObject m_knight;
    [SerializeField] private GameObject m_wizard;
    [SerializeField] private GameObject m_archer;
    [SerializeField] private GameObject m_chooseBoxs;
    [SerializeField] private GameObject m_skillPanel;

    [Header("Stat")]
    [SerializeField] private float m_range = 1f;
    [SerializeField] private float m_attackSpeed = 0.5f;
    [SerializeField] private float m_projectileSpeed = 10f;
    //and m_power, m_speed.

    [Header("Attack")]
    [SerializeField] private AttackType m_attackType = AttackType.Tear;
    [SerializeField] private ProjectileType m_projectileType = ProjectileType.None;
    [SerializeField] private GameObject m_attackStorage;

    [Header("Ring")]
    [SerializeField] private float m_chargeCurTime = 0f;
    [SerializeField] private float m_chargeMaxTime = 2f;

    [Header("Tear")]
    [SerializeField] private int m_turnArr = 1;
    #endregion

    #region PublicMethod
    public void OnClassChange(InputAction.CallbackContext _context)
    {


    }

    public override void OnAttack(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            m_isAttackPressed = true;
        }

        if (_context.canceled)
        {
            m_isAttackPressed = false;
        }
    }

    public void ChangeAttackType(AttackType _type)
    {
        if((int)m_attackType < (int)_type)
            return;

        m_attackType = _type;
    }

    protected override void Start()
    {
        base.Start();

        SetPlayerClassType(PlayerClassType.None);
        PlayerManager.instance.SetInitSetting(3);
        UIManager.Instance.SetHeartUI(m_currentHP, FinalHP);
    }

    protected override void Update()
    {
        base.Update();

        CheckAttackTypePressed();
    }

    public void SetCanChangeClass(bool canChange, PlayerClassType classType)
    {
        m_canChangeClass = canChange;
        m_selectType = classType;
    }

    protected override void SetStatus()
    {
        OnStatusChanged();
    }

    protected override void Attack()
    {
        ShowAttack();
    }

    protected override void Ability()
    {

    }
    #endregion

    #region PrivateMethod
    private void ChangeClass()
    {
        PlayerManager.instance.SetClass(m_selectType);

        //var player = Instantiate(newClass, transform.position, transform.rotation);
    }

    private void ShowAttack()
    {
        string attackPath = GetAttackPath();
        float angle = Vector2.SignedAngle(Vector2.right, m_Direction.normalized);
        //Vector3 offsetPositon = (m_offset) * m_Direction.normalized;

        GameObject obj = (GameObject)Instantiate(Resources.Load(attackPath), transform.position, Quaternion.Euler(0,0,angle), m_attackStorage.transform);
        SetAttackInit(obj);
        
    }

    private void CheckAttackTypePressed()
    {
        switch (m_attackType)
        {
            case AttackType.Bomb:
                AttackBomb();
                break;

            case AttackType.Ring:
                AttackRing();
                break;

            case AttackType.Brimstone:
                AttackBrimstone();
                break;

            case AttackType.Tear:
                AttackTear();
                break;
        }
    }

    private void AttackTear()
    {
        if (m_isAttackPressed == true)
        {
            if (m_canAttack)
            {
                Attack();
                StartCoroutine(nameof(IE_StartAttackCoolTime));
            }
        }
    }

    private void AttackRing()
    {
        if (m_isAttackPressed == true)
        {
            m_chargeCurTime += Time.deltaTime;
        }

        if (m_isAttackPressed == false) 
        {
            if(m_chargeCurTime / m_chargeMaxTime > 0.3)
            {
                ShowAttack();
            }
            m_chargeCurTime = 0;
        }
    }

    private void AttackBrimstone()
    {
        if (m_isAttackPressed == true)
        {
            m_chargeCurTime += Time.deltaTime;
        }

        if (m_isAttackPressed == false)
        {
            if (m_chargeCurTime > m_chargeMaxTime)
            {
                ShowAttack();
            }
            m_chargeCurTime = 0;
        }
    }

    private void AttackBomb()
    {
        if (m_isAttackPressed == true)
        {
            if (m_canAttack)
            {
                Attack();
                StartCoroutine(nameof(IE_StartAttackCoolTime));
            }
        }
    }

    private string GetAttackPath()
    {
        string path = "";

        switch (m_attackType) {
            case AttackType.Bomb:
                path = AttackResouceStore.ATTACK_BOMB;
                break;

            case AttackType.Ring:
                path = AttackResouceStore.ATTACK_RING;
                break;

            case AttackType.Brimstone:
                path = AttackResouceStore.ATTACK_BRIMSTONE;
                break;

            case AttackType.Tear:
                path = AttackResouceStore.ATTACK_TEAR;
                break;
        }

        return path;
    }

    private void SetAttackInit(GameObject _obj)
    {
        switch (m_attackType)
        {
            case AttackType.Bomb:
                _obj.GetComponent<IssacPlayerBomb>().InitSetting(m_projectileType, m_range, m_projectileSpeed, m_Direction, m_power, m_turnArr *= -1);
                break;
            case AttackType.Ring:
                _obj.GetComponent<Ring>().InitSetting(m_projectileType, m_range, m_projectileSpeed, m_Direction, m_power, m_chargeCurTime / m_chargeMaxTime);
                break;

            case AttackType.Brimstone:
                _obj.GetComponent<Brimstone>().InitSetting(m_projectileType, m_range, m_Direction, m_power);
                _obj.transform.SetParent(transform);
                break;

            case AttackType.Tear:
                _obj.GetComponent<Tear>().InitSetting(m_projectileType, m_range, m_projectileSpeed, m_Direction, m_power, m_turnArr *= -1);
                break;
        }
    }

    private IEnumerator IE_StartAttackCoolTime()
    {
        m_canAttack = false;

        yield return new WaitForSeconds(m_attackSpeed);

        m_canAttack = true;
    }
    #endregion

    #region ItemInteract
    public void SetAttackSpeed(float _value)
    {
        m_attackSpeed += _value;
    }

    public void SetPower(float _power)
    {
        m_power += _power;
    }

    public void SetRange(float _range)
    {
        m_range += _range;
    }

    public void SetProjectileSpeed(float _projectileSpeed)
    {
        m_projectileSpeed += _projectileSpeed;
    }

    public void SetAttackType(AttackType _type)
    {
        m_attackType = _type;
    }

    public void SetProjectileType(ProjectileType _type)
    {
        m_projectileType = _type;
    }
    #endregion
}
