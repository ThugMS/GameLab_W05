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

    private int m_enemyLayerMask;
    private int m_stopLayerMask;
    private float m_destroyTime = 5f;

    [Header("Speed")]
    [SerializeField] private float m_speed;
    [SerializeField] private float m_decelSpeed = 0.01f;

    [Header("Damage")]
    [SerializeField] private float m_power = 0f;
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

        m_enemyLayerMask = LayerMask.GetMask("Monster", "Boss");
        m_stopLayerMask = LayerMask.GetMask("Wall", "Monster", "Boss");
    }

    private void FixedUpdate()
    {
        Vector2 moveAmount = transform.up * m_speed * Time.deltaTime;
        Vector2 nextPosition = m_rigidbody.position + moveAmount;

        //m_speed -= m_decelSpeed;

        m_rigidbody.MovePosition(nextPosition);
        //m_rigidbody.velocity = m_dir * m_speed;
    }
    #endregion

    #region PrivateMethod
    private void OnCollisionEnter2D(Collision2D collision)
    {   
        if ((m_stopLayerMask & (1<<collision.gameObject.layer)) != 0)
        {
            m_speed = 0;
            
            if((m_enemyLayerMask & (1 << collision.gameObject.layer)) != 0){
                BaseMonster monster;

                collision.transform.TryGetComponent<BaseMonster>(out monster);
                transform.GetComponent<Collider2D>().enabled = false;   

                monster.getDamage(m_power);

                transform.SetParent(monster.transform);
            }

            StartCoroutine(nameof(IE_Destroy));
        }
    }

    private IEnumerator IE_Destroy()
    {
        yield return new WaitForSeconds(m_destroyTime);

        Destroy(gameObject);
    }
    #endregion
}
