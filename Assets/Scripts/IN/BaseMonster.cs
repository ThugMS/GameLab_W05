using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseMonster : MonoBehaviour
{
    #region PublicVariables

    #endregion

    #region PrivateVariables
    //GETCOMPONENTS
    protected Rigidbody2D m_rb;
    protected GameObject m_playerObj;
    protected NavMeshAgent m_agent;
    //BOOL
    private bool isAttacked = false;
    //VALUES
    [SerializeField] protected int m_speed;
    [SerializeField] protected int m_range;
    [SerializeField] protected int m_attackTime;
    [SerializeField] protected float m_Knockback;

    [SerializeField] private float m_knockBackTime;
    [SerializeField] private float m_health;
    [SerializeField] private float m_patrolTime;

    private Vector3 m_initialPosition;
    private Vector3 targetPatrolPos;

    private float m_timer;

    #endregion

    #region PublicMethod
    public float Health { get => m_health; set => m_health = value; }
    public virtual void Start()
    {
        //NavMesh
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.updateRotation = false;
        m_agent.updateUpAxis = false;
        //Init
        m_playerObj = GameObject.FindGameObjectWithTag("Player");
        m_rb = GetComponent<Rigidbody2D>();
        targetPatrolPos = transform.position;
        m_timer = m_patrolTime;
        targetPatrolPos = getPatrolPos();
       }

    public virtual void Update()
    {
        if (!isAttacked && Vector3.Distance(transform.position, m_playerObj.transform.position) < m_range)//Persuit if Within Range
        {
            m_agent.ResetPath();
            Persuit();
        }
        else if (Vector3.Distance(transform.position, targetPatrolPos) > m_range)
        {//resete Anchor pos and patrol
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


    }

    public virtual void Attack()
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
        isAttacked = true;
        Health -= value;
        Vector2 moveDirection = (transform.position - m_playerObj.transform.position).normalized;
        m_agent.SetDestination((Vector2)transform.position + moveDirection);
        StartCoroutine(knockBack());

        if (Health <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    #region PrivateMethod

    IEnumerator knockBack()
    {
        yield return new WaitForSeconds(m_knockBackTime);
        m_agent.ResetPath();
        isAttacked = false;
        yield return null;
    }

    #endregion
    #region PrivateMethod
    private Vector3 getPatrolPos()
    {
       return new Vector2(UnityEngine.Random.Range(m_initialPosition.x - m_range, m_initialPosition.x + m_range),
           UnityEngine.Random.Range(m_initialPosition.y - m_range, m_initialPosition.y + m_range));
    }

    #endregion
    #endregion

}