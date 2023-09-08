using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAttack : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [Header("Status")]
    [SerializeField] private float m_curSpeed = 0f;
    [SerializeField] private float m_addSpeed = 0.1f;
    [SerializeField] private float m_maxSpeed = 20f;
    [SerializeField] private Vector2 m_direction;
    #endregion

    #region PublicMethod
    public void InitSetting(Vector2 _dir)
    {
        m_direction = _dir;
    }
    #endregion

    #region PrivateMethod
    #endregion
}
