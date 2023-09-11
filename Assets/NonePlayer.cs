using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class NonePlayer : Player
{
    private bool m_canChangeClass;
    private PlayerClassType m_selectType = PlayerClassType.None;
    [Header("Class")]
    [SerializeField] private GameObject m_knight;
    [SerializeField] private GameObject m_wizard;
    [SerializeField] private GameObject m_archer;
    [SerializeField] private GameObject m_chooseBoxs;
    
    protected override void Start()
    {
        base.Start();

        SetPlayerClassType(PlayerClassType.None);
    }
    
    protected override void SetStatus()
    {
        OnStatusChanged();
    }
    
    protected override void Attack()
    {
        if (m_canChangeClass && m_selectType != PlayerClassType.None)
        {
            ChangeClass();
        }
    }

    protected override void Ability()
    {
        
    }

    public void OnClassChange(InputAction.CallbackContext _context)
    {
    
    }

    private void ChangeClass()
    {
        Debug.Log("Change Class");

        PlayerManager.instance.SetClass(m_selectType);
        
        //var player = Instantiate(newClass, transform.position, transform.rotation);
        
        
        m_chooseBoxs.SetActive(false);
        gameObject.SetActive(false);
    }

    public void SetCanChangeClass(bool canChange, PlayerClassType classType)
    {
        m_canChangeClass = canChange;
        m_selectType = classType;
    }
}
