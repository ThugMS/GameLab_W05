using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Scriptable Object/Player Data", order = int.MaxValue)]
public class PlayerData : ScriptableObject
{
    #region PublicVariables
    public PlayerClassType Type { get { return m_type; } }
    public float Power { get { return m_power; } }
    public float Speed { get { return m_speed; } }
    public float Health { get { return m_health; } }
    public float CoolTime { get { return m_coolTime; } }

    #endregion

    #region PrivateVariables
    [SerializeField] private PlayerClassType m_type;
    [SerializeField] private float m_power;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_health;
    [SerializeField] private float m_coolTime;
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    #endregion
}
