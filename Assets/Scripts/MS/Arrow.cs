using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] private Rigidbody2D m_rigidbody;
    [SerializeField] private Vector2 m_dir;

    [Header("Speed")]
    [SerializeField] private float m_speed;
    [SerializeField] private float m_decelSpeed = 0.01f;

    [Header("Damage")]
    [SerializeField] private float m_power;
    #endregion

    #region PublicMethod
    public void InitSetting(float _speed, Vector2 _dir)
    {
        m_speed = _speed;
        m_dir = _dir;
    }

    private void Start()
    {
        TryGetComponent<Rigidbody2D>(out m_rigidbody);
    }

    private void FixedUpdate()
    {
        Vector2 moveAmount = transform.up * m_speed * Time.deltaTime;
        Vector2 nextPosition = m_rigidbody.position + moveAmount;

        m_speed -= m_decelSpeed;

        m_rigidbody.MovePosition(nextPosition);
        //m_rigidbody.velocity = m_dir * m_speed;
    }
    #endregion

    #region PrivateMethod
    #endregion
}
