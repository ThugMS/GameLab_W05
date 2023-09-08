using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAttack : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] private Rigidbody2D m_rigidbody;

    private int m_enemyLayerMask;
    private int m_stopLayerMask;

    [Header("Status")]
    [SerializeField] private float m_curSpeed = 0f;
    [SerializeField] private float m_addSpeed = 0.1f;
    [SerializeField] private float m_maxSpeed = 20f;
    [SerializeField] private float m_power = 5f;
    [SerializeField] private Vector2 m_direction;
    #endregion

    #region PublicMethod
    private void Start()
    {
        TryGetComponent<Rigidbody2D>(out m_rigidbody);

        m_enemyLayerMask = LayerMask.GetMask("Monster", "Boss");
        m_stopLayerMask = LayerMask.GetMask("Wall", "Monster", "Boss");
    }

    public void InitSetting(Vector2 _dir)
    {
        m_direction = _dir;
    }

    private void FixedUpdate()
    {
        SetSpeed();

        Vector2 moveAmount = m_direction * m_curSpeed * Time.deltaTime;
        Vector2 nextPosition = m_rigidbody.position + moveAmount;

        m_rigidbody.MovePosition(nextPosition);
    }
    #endregion

    #region PrivateMethod
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((m_stopLayerMask & (1 << collision.gameObject.layer)) != 0)
        {
            if ((m_enemyLayerMask & (1 << collision.gameObject.layer)) != 0)
            {
                BaseMonster monster;

                collision.transform.TryGetComponent<BaseMonster>(out monster);
                transform.GetComponent<Collider2D>().enabled = false;

                monster.getDamage(m_power);

                transform.SetParent(monster.transform);
            }

            Destroy(gameObject);
        }
    }

    private void SetSpeed()
    {
        m_curSpeed += m_addSpeed;

        if(m_curSpeed >= m_maxSpeed)
        {
            m_curSpeed = m_maxSpeed;
        }
    }
    #endregion
}
