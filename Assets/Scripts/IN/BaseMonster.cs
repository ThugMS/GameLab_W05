using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseMonster : MonoBehaviour
{
    #region PublicVariables

    #endregion

    #region PrivateVariables
    //States
    protected enum MonsterState
    {
        Patrol,
        Pursuit,
        Attack,
        Knockback,
        Dead
    }
    protected MonsterState currentState = MonsterState.Patrol;

    //==References
    protected Rigidbody2D m_rb;
    protected GameObject m_playerObj;
    protected NavMeshAgent m_agent;
    private bool isAttacked = false;
    [Header("Value")]
    [SerializeField] protected int m_speed;
    [SerializeField] protected int m_range;
    [SerializeField] protected float m_basicAttack;
    [Header("Time")]
    [SerializeField] private float m_knockBackTime;
    [SerializeField] private float m_health;
    [SerializeField] private float m_patrolTime;
    //==Positions
    private Vector3 m_initialPosition;
    private Vector3 targetPatrolPos;
    //==Timer
    private float m_timer;

    #endregion

    #region PublicMethod
    //=GetSet
    public float Health { get => m_health; set => m_health = value; }
    public void getDamage(float _damage)
    {
        TransitionToState(MonsterState.Knockback);
        Health -= _damage;
        Vector2 moveDirection = (transform.position - m_playerObj.transform.position).normalized;
        m_agent.SetDestination((Vector2)transform.position + moveDirection);
        StartCoroutine(IE_KnockBack());

        if (Health <= 0)
        {
            TransitionToState(MonsterState.Dead);
            Dead();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player;
            collision.gameObject.TryGetComponent<Player>(out player);

            player.GetDamage(m_basicAttack);
        }
    }

    public virtual void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.updateRotation = false;
        m_agent.updateUpAxis = false;

        m_playerObj = GameObject.FindGameObjectWithTag("Player");
        m_rb = GetComponent<Rigidbody2D>();
        targetPatrolPos = transform.position;
        m_timer = m_patrolTime;
        targetPatrolPos = getPatrolPos();
    }

    public virtual void Update()
    {
        switch (currentState)
        {
            case MonsterState.Patrol:
                if (Vector3.Distance(transform.position, m_playerObj.transform.position) < m_range)
                {
                    TransitionToState(MonsterState.Pursuit);
                }
                Patrol();
                break;
            case MonsterState.Pursuit:
                if (Vector3.Distance(transform.position, m_playerObj.transform.position) >= m_range)
                {
                    Patrol();
                }
                else
                {
                    Pursuit();
                }
                break;
        }
    }

    public virtual void Patrol()
    {
        m_agent.ResetPath();
        m_agent.SetDestination(targetPatrolPos);
        if (Vector2.Distance(transform.position, targetPatrolPos) < 0.2f)
        {
            m_timer -= Time.deltaTime;
            if (m_timer < 0)
            {
                m_timer = m_patrolTime;
                targetPatrolPos = getPatrolPos();
            }
        }
    }
    public virtual void Pursuit()
    {
        m_agent.SetDestination(m_playerObj.transform.position);
    }

    public virtual void Attack() { }



    public void Dead()
    {
        Destroy(gameObject);
    }

    #endregion
    #region PrivateMethod

    protected void TransitionToState(MonsterState newState)
    {
        currentState = newState;
        m_agent.ResetPath();
    }

    protected virtual Transform detectingPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player").transform;
    }

    private Vector3 getPatrolPos()
    {
        return new Vector2(UnityEngine.Random.Range(m_initialPosition.x - m_range, m_initialPosition.x + m_range),
            UnityEngine.Random.Range(m_initialPosition.y - m_range, m_initialPosition.y + m_range));
    }


    private IEnumerator IE_KnockBack()
    {
        yield return new WaitForSeconds(m_knockBackTime);
        m_agent.ResetPath();
        isAttacked = false;
        TransitionToState(MonsterState.Patrol);
        yield return null;
    }



    #endregion

}