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
    [SerializeField] private bool m_isReady = false;
    [SerializeField] private float m_readyTime = 0.5f;
    [SerializeField] private float m_curSpeed = 0f;
    [SerializeField] private float m_addSpeed = 0.3f;
    [SerializeField] private float m_maxSpeed = 20f;
    [SerializeField] private float m_power = 5f;
    [SerializeField] private Vector2 m_direction;

    [Header("Animation")]
    [SerializeField] private Animator m_animator;
    #endregion

    #region PublicMethod
    private void Start()
    {
        TryGetComponent<Rigidbody2D>(out m_rigidbody);

        m_enemyLayerMask = LayerMask.GetMask("Monster", "Boss");
        m_stopLayerMask = LayerMask.GetMask("Wall", "Monster", "Boss");

        StartCoroutine(nameof(IE_SetReady));
    }

    public void InitSetting(Vector2 _dir, float _power)
    {
        m_direction = _dir;
        m_power = _power;
    }

    private void FixedUpdate()
    {
        if (m_isReady == true)
        {
            SetSpeed();
        }
        
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
            m_animator.SetTrigger("Hit");
            m_curSpeed = 0;
            m_isReady = false;

            if ((m_enemyLayerMask & (1 << collision.gameObject.layer)) != 0)
            {
                BaseMonster monster;

                collision.transform.TryGetComponent<BaseMonster>(out monster);
                transform.GetComponent<Collider2D>().enabled = false;

                monster.getDamage(m_power);
            }
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

    private IEnumerator IE_SetReady()
    {
        yield return new WaitForSeconds(m_readyTime);
        m_isReady = true;
    }
    #endregion
}
