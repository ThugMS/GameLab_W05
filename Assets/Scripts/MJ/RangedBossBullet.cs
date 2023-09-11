using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedBossBullet : MonoBehaviour
{
    [SerializeField] private int m_damage;
    CircleCollider2D m_collider2D;
    Animator m_animator;
    private static readonly int Destory = Animator.StringToHash("Destory");


    // Start is called before the first frame update
    void Start()
    {
        m_collider2D = GetComponent<CircleCollider2D>();
        m_animator = GetComponent<Animator>();
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
        
        GetComponent<Rigidbody2D>().velocity = transform.right * 0 ;
        m_animator.SetBool(Destory, true);
        m_collider2D.enabled = false;
    }

    public void DestroyObjectAfterAnimation()
    {
        Destroy(this.gameObject);
    }
    
}
