using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseBox : MonoBehaviour
{
    private Animator m_animator;
    [SerializeField] private PlayerClassType m_classType;
    [SerializeField] private GameObject m_keyHint;
    [SerializeField] private NonePlayer m_player;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_keyHint.SetActive(false);
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_animator.SetBool("IsSelect", true);
            m_keyHint.SetActive(true);

            if (collision.gameObject.TryGetComponent<NonePlayer>(out var player))
            {
                player.SetCanChangeClass(true, m_classType);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_animator.SetBool("IsSelect", false);
            m_keyHint.SetActive(false);
            
            if (collision.gameObject.TryGetComponent<NonePlayer>(out var player))
            {
                player.SetCanChangeClass(false, PlayerClassType.None);
            }
        }
    }

    private void OnChanged()
    {
        Destroy(this.gameObject);
    }
}
