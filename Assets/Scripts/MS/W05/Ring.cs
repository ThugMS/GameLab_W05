using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
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
    [SerializeField] private float m_size;
    #endregion

    #region PublicMethod
    public void InitSetting(ProjectileType _type, float _lifeTime, float _speed, Vector2 _dir, float _power, float _size)
    {
        m_projectileType = _type;
        m_lifeTime = _lifeTime * 3;
        m_speed = _speed;
        m_dir = _dir;
        m_power = _power;
        m_size = _size;

        if (m_size > 1)
            m_size = 1;

        transform.localScale *= m_size;
    }

    public void FixedUpdate()
    {
        Vector2 moveAmount = transform.right * m_speed * Time.deltaTime;
        Vector2 nextPosition = m_rigidbody.position + moveAmount;

        m_rigidbody.MovePosition(nextPosition);
    }

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_layerMask = LayerMask.GetMask("Monster", "Boss");

        StartCoroutine(nameof(IE_Destroy));
    }
    #endregion

    #region PrivateMethod
    private IEnumerator IE_Destroy()
    {
        yield return new WaitForSeconds(m_lifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((m_layerMask & (1 << collision.gameObject.layer)) != 0)
        {
            collision.gameObject.GetComponent<DamageBot>().ShowDamage(m_power);
        }
    }
    #endregion
}
