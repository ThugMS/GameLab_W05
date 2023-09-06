using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMonster : MonoBehaviour
{
    #region PublicVariables

    #endregion

    #region PrivateVariables
    protected GameObject m_playerObj;
    [SerializeField] protected int m_speed;
    [SerializeField] protected int m_range;
    [SerializeField] private float health;
    [SerializeField] protected int m_attackTime;

    protected float Health { get => health; set => health = value; }
    #endregion

    #region PublicMethod
    #endregion

    public virtual void Start()
    {
        m_playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    public virtual void Update()
    {
        Persuit();
        
    }

    public virtual void Attack()
    {

    }
    public virtual void Move()
    {
        
    }

    public virtual void Persuit()
    {
        
    }

    public virtual Transform detectingPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player").transform;
    }


    public void getDamage(float value)
    {
        Health -= value;
        if(Health < 0)
        {
            Death();
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    #region PrivateMethod

    #endregion
}