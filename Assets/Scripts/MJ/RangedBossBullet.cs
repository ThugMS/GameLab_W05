using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedBossBullet : MonoBehaviour
{
    [SerializeField] private int m_damage;
    CircleCollider2D m_collider2D;
    Animator m_animator;
    
    
    // Start is called before the first frame update
    void Start()
    {
        m_collider2D = GetComponent<CircleCollider2D>();
    }

    public void Init(float _bulletSpeed)
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * _bulletSpeed ;;

    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<Player>().GetDamage(m_damage);
            PlayDestroyAnimation();
        }
        else if (col.CompareTag("Walls"))
        {
            PlayDestroyAnimation();
        }
    }

    void PlayDestroyAnimation()
    {
        m_collider2D.enabled = false;
    }

    public void DestroyObjectAfterAnimation()
    {
        Destroy(this);
    }
    
}
