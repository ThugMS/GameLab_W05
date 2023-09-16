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

        if(m_isAttackPressed == true)
        {
            if (m_canAttack)
            {
                Attack();
                StartCoroutine(nameof(IE_StartAttackCoolTime)); 
            }
        }
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
        float angle = Vector2.SignedAngle(Vector2.up, m_Direction.normalized);
        //Vector3 offsetPositon = (m_offset) * m_Direction.normalized;

        GameObject obj = (GameObject)Instantiate(Resources.Load(attackPath), transform.position, Quaternion.Euler(0,0,angle), m_attackStorage.transform);
        SetAttackInit(obj);
        
    }

    private string GetAttackPath()
    {
        string path = "";

        switch (m_attackType) { 
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
            case AttackType.Tear:
                _obj.GetComponent<Tear>().InitSetting(m_projectileType, m_range, m_projectileSpeed, m_Direction, m_power);
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
}
