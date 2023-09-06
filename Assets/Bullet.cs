using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    private float m_timer;
    [SerializeField] private float m_limitTime;
    [SerializeField] private float m_damage;
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void Start()
    {
        m_timer = m_limitTime;
    }

    private void Update()
    {
        m_timer -= Time.deltaTime;
        if(m_timer < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")== true) {
            //Player.getDamage(m_damage)
        }
    }

    #endregion
}