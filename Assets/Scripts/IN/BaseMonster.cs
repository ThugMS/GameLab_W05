using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseMonster : MonoBehaviour
{
    #region 
    public int stage;
    public bool isOn = false;
    public LayerMask m_detectingLayer;
    public Action DeadListener;
    #endregion
    #region PrivateVariables
    protected enum MonsterState
    {
        Patrol,
        Pursuit,
        Attack,
        Knockback,
        Stop,
        Dead
    }
    [Header("State")]
    [SerializeField] protected MonsterState m_currentState = MonsterState.Patrol;
    protected Rigidbody2D m_rb;
    protected GameObject m_playerObj;
    protected NavMeshAgent m_agent;
    protected bool isAttacked = false;
    [Header("Value")]
    [SerializeField] protected int m_speed;
    [SerializeField] protected int m_range;
    [SerializeField] protected float m_basicAttack;
    [SerializeField] private float m_health;

    [Header("Time")]
    [SerializeField] protected float m_knockBackTime;
    [SerializeField] protected float m_patrolTime;
    //==Positions
    protected Vector3 m_initialPosition;
    protected Vector3 targetPatrolPos;
    protected Animator m_animator;
    //==Timer
    protected float m_timer;
    [Header("KnockBack")]

    [SerializeField] private float knockbackSpeed;
    [SerializeField] private float knockbackTime;
    [SerializeField] private float knockbackDistance;
    private float knockbackTimer;
    #endregion
    #region PublicMethod
    //====================================InteractionWithPlayer========================
    public float Health { get => m_health; set => m_health = value; }
    public void getDamage(float _damage)
    {
        TransitionToState(MonsterState.Knockback);
        Health -= _damage;
        Vector2 moveDirection = (transform.position - m_playerObj.transform.position).normalized;
        m_agent.SetDestination((Vector2)transform.position + moveDirection);
        StartCoroutine(nameof(IE_KnockBack));
        if (Health <= 0)
        {
            TransitionToState(MonsterState.Dead);
            Dead();
        }
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        DamagePlayer(collision.gameObject);
    }
    protected virtual void DamagePlayer(GameObject collidingObject)
    {
        if (collidingObject.CompareTag("Player"))
        {
            Player player;
            collidingObject.TryGetComponent<Player>(out player);

            player.GetDamage(m_basicAttack);
        }
    }
    //===============================InitFunc=================================
    protected void Update ()
    {
        if(isOn == true)
        {
            stateUpdate();
        }
    }
    protected abstract void stateUpdate();
    public void init()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.updateRotation = false;
        m_agent.updateUpAxis = false;

        m_playerObj = GameObject.FindGameObjectWithTag("Player");
        m_rb = GetComponent<Rigidbody2D>();
        targetPatrolPos = transform.position;
        m_timer = m_patrolTime;
        targetPatrolPos = getPatrolPos();
        m_agent.speed = m_speed;
        m_animator = GetComponent<Animator>();
        isOn = true;
    }
    //======================Abstract Behavior according to State===============
    protected abstract void Patrol();
    protected abstract void Pursuit(); 
    protected abstract void Attack();
    protected void Dead()
    {
        Destroy(gameObject);
        DeadListener?.Invoke();
    }
    #endregion
    #region PrivateMethod
    //===================Funcs for Behavior=====================================
    protected virtual void TransitionToState(MonsterState newState)
    {
        m_currentState = newState;
        m_agent.ResetPath();
    }
    protected virtual bool canSeePlayer()
    {
        Vector2 directionToPlayer = m_playerObj.transform.position - transform.position;
        Debug.DrawRay(transform.position, directionToPlayer, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, directionToPlayer.magnitude, m_detectingLayer);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Player")
                return true;
        }
        return false;
    }
    protected virtual bool playerWithinRange()
    {
        if (Vector2.Distance(transform.position, m_playerObj.transform.position) < m_range)
        {
            return true;
        }
        return false;
    }
    protected Vector3 getPatrolPos()
    {
        return new Vector2(UnityEngine.Random.Range(m_initialPosition.x - m_range, m_initialPosition.x + m_range),
            UnityEngine.Random.Range(m_initialPosition.y - m_range, m_initialPosition.y + m_range));
    }
    protected virtual IEnumerator IE_KnockBack()
    {
        TransitionToState(MonsterState.Knockback);

        Vector2 moveDirection = (transform.position - m_playerObj.transform.position).normalized;
        Vector2 knockbackEndPosition = (Vector2)transform.position + moveDirection * knockbackDistance;

        knockbackTimer = knockbackTime;
        while (knockbackTimer > 0)
        {
            knockbackTimer -= Time.time;
            m_agent.SetDestination(knockbackEndPosition);
            yield return null;
        }
        transform.position = knockbackEndPosition;
        TransitionToState(MonsterState.Patrol);
        isAttacked = false;
    }



    //=====================================================GIZMO GUI
    protected virtual void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.yellow; 
            Gizmos.DrawWireSphere(transform.position, m_range);
        }
    }

    //================================================Animation
   



    #endregion

}