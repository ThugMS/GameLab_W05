using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSample : MonoBehaviour
{
    public static TestSample Instance;
    
    public string m_keyword1;
    public string m_keyword2;
    public string m_keyword3;
    
    public GameObject[] m_monsterPrefab;

    public GameObject m_reward;

    public GameObject m_boss;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
    