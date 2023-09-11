using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIGift : MonoBehaviour
{
    [SerializeField] private GameObject m_giftOpenObject;
    [SerializeField] private GameObject m_giftCloseObject;

    private void Start()
    {
        m_giftCloseObject.SetActive(true);
        m_giftOpenObject.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            m_giftCloseObject.SetActive(false);
            m_giftOpenObject.SetActive(true);
            
            var jams = ResourceManager.Instance.m_jamPrefabs;
            int index = Random.Range(0, jams.Count);

            
            //Instantiate(jams[index], transform);
            
        }
    }
}
