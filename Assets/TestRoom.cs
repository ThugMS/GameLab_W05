using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRoom : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    GameObject[] monsters;

    private void Start()
    {
        turnOn();
    }

    private void OnEnable()
    {
        turnOn();
    }

    public void turnOn()
    {

        monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject monster in monsters)
        {
            monster.GetComponent<BaseMonster>().init();
        }
    }

    #endregion

    #region PrivateMethod
    #endregion
}