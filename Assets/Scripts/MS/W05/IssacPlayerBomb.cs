using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IssacPlayerBomb : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    private Rigidbody2D m_rigidbody;
    private int m_layerMask;
    private Animator m_animator;

    [Header("Status")]
    [SerializeField] private ProjectileType m_projectileType = ProjectileType.None;
    [SerializeField] private float m_lifeTime = 1.0f;
    [SerializeField] private float m_speed = 5f;
    [SerializeField] private Vector2 m_dir = Vector2.zero;
    [SerializeField] private float m_power = 1f;

    [Header("Planet")]
    [SerializeField] private Vector3 m_centerPos;
    [SerializeField] private float m_angle = 0f;
    [SerializeField] private float m_radius = 0f;
    [SerializeField] private float m_radiusAdd = 0.1f;
    [SerializeField] private float m_radiusMax = 3f;
    [SerializeField] private int m_turnArr = 1;

    [Header("ZigZag")]
    [SerializeField] private float m_zigzagDis = 0.5f;

    [Header("Bomb")]
    private float m_downSpeed = 0f;
    private float m_downAddSpeed = 0.01f;
    
    //[Header("Electric")]
    //[SerializeField] private LineRenderer m_line;
    //[SerializeField] private Vector3[] m_linePoints = new Vector3[2];
    //[SerializeField] private BoxCollider2D m_boxCol;
    #endregion

    #region PublicMethod
    public void InitSetting(ProjectileType _type, float _lifeTime, float _speed, Vector2 _dir, float _power, int _turnArr)
    {
        m_projectileType = _type;
        m_lifeTime = _lifeTime;
        m_speed = _speed;
        m_dir = _dir;
        m_power = _power * 8;
        m_turnArr = _turnArr;
    }

    public void FixedUpdate()
    {
        CheckProjectileType();
    }

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_layerMask = LayerMask.GetMask("Monster", "Boss");
        m_centerPos = transform.position;
        m_animator = GetComponent<Animator>();

        if (m_projectileType == ProjectileType.Planet)
        {
            m_lifeTime = 5f;
        }

        StartCoroutine(nameof(IE_Destroy));
    }

    public void Explosion()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2);

        foreach(var col in cols)
        {
            if ((m_layerMask & (1 << col.gameObject.layer)) != 0)
            {
                col.gameObject.GetComponent<DamageBot>().ShowDamage(m_power);
            }
        }

        
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
    #endregion

    #region PrivateMethod
    private void CheckProjectileType()
    {
        switch (m_projectileType)
        {
            case ProjectileType.None:
                NoneType();
                break;

            case ProjectileType.Planet:
                PlanetType();
                break;

            case ProjectileType.Zigzag:
                ZigZagType();
                break;
        }
    }

    private void NoneType()
    {
        m_rigidbody.velocity = transform.right * m_speed;

        m_speed = m_speed - m_downSpeed < 0 ? 0 : m_speed - m_downSpeed;
        m_downSpeed += m_downAddSpeed;

        
        //Vector2 moveAmount = transform.right * m_speed * Time.deltaTime;
        //Vector2 nextPosition = m_rigidbody.position + moveAmount;

        //m_rigidbody.MovePosition(nextPosition);
    }

    private void PlanetType()
    {
        m_centerPos = PlayerManager.instance.GetPlayer().transform.position;
        m_angle += m_speed * Time.deltaTime * m_turnArr;

        m_speed = m_speed - 0.1f < 2f ? m_speed : m_speed - 0.1f;
        m_radius = m_radius + m_radiusAdd > m_radiusMax ? m_radiusMax : m_radius + m_radiusAdd;

        Vector3 nextPos = m_centerPos + new Vector3(Mathf.Cos(m_angle), Mathf.Sin(m_angle), 0) * m_radius;

        m_rigidbody.MovePosition(nextPos);
    }

    private void ZigZagType()
    {
        Vector2 axis = transform.up;
        Vector2 moveAmount = transform.right * m_speed * Time.deltaTime;
        Vector2 zigzag = axis * Mathf.Sin(Time.time * 10f) * 0.1f * m_turnArr;
        Vector2 nextPosition = m_rigidbody.position + moveAmount + zigzag;

        m_rigidbody.MovePosition(nextPosition);
    }

    private IEnumerator IE_Destroy()
    {
        yield return new WaitForSeconds(m_lifeTime);

        m_animator.Play("Bomb");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    #endregion
}
