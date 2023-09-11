using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices.WindowsRuntime;
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
    [Header("Boss")]
    [SerializeField] protected bool isBoss;

    [Header("State")]
    [SerializeField] protected MonsterState m_currentState = MonsterState.Patrol;
    protected Rigidbody2D m_rb;
    protected GameObject m_playerObj;
    protected NavMeshAgent m_agent;
    protected bool isAttacked = false;
    [Header("Value")]
    [SerializeField] protected int m_speed;
    [SerializeField] protected int m_range;
    [SerializeField] private float m_health;
   
    [Header("Attack")]
    [SerializeField] protected float m_basicAttack;
    [SerializeField] protected float m_damageRefreshTime;
    protected float damageRefreshTimer;

    [Header("Time")]
    [SerializeField] protected float m_knockbackTime;
    [SerializeField] protected float m_patrolTime;
    //==Positions
    protected Vector3 m_initialPosition;
    protected Vector3 targetPatrolPos;
    protected Animator m_animator;
    //==Timer
    protected float m_knockbackTimer;
    protected float m_patrolTimer;
    protected SpriteRenderer m_spriteRenderer;
    protected Color m_originalColor;


    #endregion
    #region PublicMethod
    //====================================InteractionWithPlayer========================
    public float Health { get => m_health; set => m_health = value; }
    public virtual void getDamage(float _damage, float knockbackPower)
    {
        TransitionToState(MonsterState.Knockback);
        Health -= _damage;
        Vector2 moveDirection = (transform.position - m_playerObj.transform.position).normalized;
        if (m_agent.isActiveAndEnabled == true)
        {
            m_agent.SetDestination((Vector2)transform.position + moveDirection);
        }
        StartCoroutine(IE_KnockBack(knockbackPower));
        if (Health <= 0)
        {
            TransitionToState(MonsterState.Dead);
            Dead();
        }
    }

    public virtual void getDamage(float _damage)
    {
        //TransitionToState(MonsterState.Knockback);
        Health -= _damage;
        StartCoroutine(nameof(IE_TweenDamage));
        if (Health <= 0)
        {
            if (isBoss)
            {
                Dead();
            } else
            {
                StartCoroutine(nameof(IE_PlayDyingEffect));
            }
        }
    }


    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           
             DamagePlayer(collision.gameObject);
           
        }
    }

    private IEnumerator IE_TweenDamage()
    {
        transform.DOPunchScale(new Vector3(-0.05f, -0.05f, 0f), 0.2f);

        m_spriteRenderer.DOColor(Color.red, 0.25f);

        yield return new WaitForSeconds(0.25f);

        transform.DOPunchScale(new Vector3(0.05f, 0.05f, 0f), 0.2f);

        m_spriteRenderer.DOColor(m_originalColor, 0.25f);
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

    protected virtual IEnumerator IE_PlayDyingEffect()
    {
        TransitionToState(MonsterState.Stop);
        if (m_agent.isActiveAndEnabled)
        {
            m_agent.isStopped = true;
        }
        yield return new WaitForSeconds(0.5f);
        Vector3 targetScale = new Vector3(1f, 0f, 1f);

        transform.DOScale(targetScale, .4f);
        transform.DOMoveY(transform.position.y - transform.localScale.y / 1f, .4f);
        m_spriteRenderer.DOFade(0f, 0.1f).SetEase(Ease.OutExpo);


        yield return new WaitForSeconds(.2f); 
        DeadListener?.Invoke();
        Destroy(gameObject);
    }

    //===============================InitFunc=================================
    protected virtual void Update ()
    {
        if(isOn == true)
        {
            stateUpdate();
        }
    }
    protected abstract void stateUpdate();
    public virtual void init()
    {

        m_agent = GetComponent<NavMeshAgent>();
        m_agent.updateRotation = false;
        m_agent.updateUpAxis = false;
        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_originalColor = m_spriteRenderer.color;

        m_playerObj = GameObject.FindGameObjectWithTag("Player");
        m_rb = GetComponent<Rigidbody2D>();
        targetPatrolPos = transform.position;
        m_knockbackTimer = m_knockbackTime;
        damageRefreshTimer = 0.1f;
        m_patrolTimer = m_patrolTime;
        targetPatrolPos = getPatrolPos();
        m_agent.speed = m_speed;
        m_animator = GetComponent<Animator>();
        isOn = true;
    }

    //======================Abstract Behavior according to State===============
    protected abstract void Patrol();
    protected abstract void Pursuit(); 
    protected abstract void Attack();
    protected virtual void Dead()
    {
        StartCoroutine(nameof(IE_PlayDyingEffect));
        DeadListener?.Invoke();
        Destroy(gameObject);
    }
    #endregion
    #region PrivateMethod
    //======================KnockBack=============================
    protected virtual IEnumerator IE_KnockBack(float knockbackDistance)
    {
        TransitionToState(MonsterState.Knockback);

        Vector2 moveDirection = (transform.position - m_playerObj.transform.position).normalized;
        Vector2 knockbackEndPosition = (Vector2)transform.position + moveDirection * knockbackDistance;
        m_agent.enabled = false;
        m_knockbackTimer = m_knockbackTime;

        m_rb.velocity = moveDirection * knockbackDistance;
        yield return new WaitForSeconds(m_knockbackTime);
        m_rb.velocity = Vector2.zero;
        //      transform.position = knockbackEndPosition;
        m_agent.enabled = true;
        TransitionToState(MonsterState.Patrol);
        isAttacked = false;
    }



    //===================Funcs for Behavior=====================================
    protected virtual void TransitionToState(MonsterState newState)
    {
        m_currentState = newState;
        if (m_agent.isActiveAndEnabled == true)
        {
            m_agent.ResetPath();
        }
    }

    protected virtual bool canSeePlayer()
    {
        return (raycastPlayer().CompareTag("Player"))? true: false;
    }

    protected virtual Collider2D raycastPlayer()
    {
        Vector2 directionToPlayer = m_playerObj.transform.position - transform.position;
        Debug.DrawRay(transform.position, directionToPlayer, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, directionToPlayer.magnitude, m_detectingLayer);

        return hit.collider;
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