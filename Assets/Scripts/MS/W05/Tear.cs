using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tear : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    private Rigidbody2D m_rigidbody;

    [Header("Status")]
    [SerializeField] private ProjectileType m_projectileType = ProjectileType.None;
    [SerializeField] private float m_lifeTime = 1.0f;
    [SerializeField] private float m_speed = 5f;
    [SerializeField] private Vector2 m_dir = Vector2.zero;
    [SerializeField] private float m_power = 1f;
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
        Vector2 moveAmount = transform.up * m_speed * Time.deltaTime;
        Vector2 nextPosition = m_rigidbody.position + moveAmount;

        m_rigidbody.MovePosition(nextPosition);
    }

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();

        StartCoroutine(nameof(IE_Destroy));
    }
    #endregion

    #region PrivateMethod
    private IEnumerator IE_Destroy()
    {
        yield return new WaitForSeconds(m_lifeTime);
        Destroy(gameObject);
    }
    #endregion
}
