using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tear : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    private Rigidbody2D m_rigidbody;
    private int m_layerMask;

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

    [Header("Electric")]
    [SerializeField] private LineRenderer m_line;
    [SerializeField] private Vector3[] m_linePoints = new Vector3[2];
    #endregion

    #region PublicMethod
    public void InitSetting(ProjectileType _type, float _lifeTime, float _speed, Vector2 _dir, float _power)
    {
        m_projectileType = _type;
        m_lifeTime = _lifeTime;
        m_speed = _speed;
        m_dir = _dir;
        m_power = _power;
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

        if(m_projectileType == ProjectileType.Planet)
        {
            m_lifeTime = 5f;
        }

        StartCoroutine(nameof(IE_Destroy));
    }

    public void SetElectric(GameObject _obj)
    {
        m_linePoints[0] = transform.position;
        m_linePoints[1] = _obj.transform.position;

        m_line.SetPositions(m_linePoints);
    }
    #endregion

    #region PrivateMethod
    private void CheckProjectileType()
    {
        switch(m_projectileType)
        {
            case ProjectileType.None:
                NoneType();
                break;

            case ProjectileType.Planet:
                PlanetType();
                break;
        }
    }
    
    private void NoneType()
    {
        Vector2 moveAmount = transform.right * m_speed * Time.deltaTime;
        Vector2 nextPosition = m_rigidbody.position + moveAmount;

        m_rigidbody.MovePosition(nextPosition);
    }

    private void PlanetType()
    {
        m_centerPos = PlayerManager.instance.GetPlayer().transform.position;
        m_angle += m_speed * Time.deltaTime;

        m_speed = m_speed - 0.1f < 2f ? m_speed : m_speed - 0.1f;
        m_radius = m_radius + m_radiusAdd > m_radiusMax ? m_radiusMax : m_radius + m_radiusAdd;

        m_rigidbody.MovePosition(m_centerPos + new Vector3(Mathf.Cos(m_angle), Mathf.Sin(m_angle), 0) * m_radius);
    }

    private IEnumerator IE_Destroy()
    {
        yield return new WaitForSeconds(m_lifeTime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((m_layerMask & (1 << collision.gameObject.layer)) != 0)
        {
            collision.gameObject.GetComponent<DamageBot>().ShowDamage(m_power);
            Destroy(gameObject);
        }
    }
    #endregion
}
