using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] protected int m_helath;
    [SerializeField] protected int m_speed;
    [SerializeField] protected int m_rewards;
    #endregion

    #region PublicMethod
    public virtual void Attack()
    {

    }

    public virtual void Move()
    {

    }

    #endregion

    #region PrivateMethod


    #endregion

}