using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{   
    public static PlayerManager instance;

    #region PublicVariables
    public GameObject m_player;
    #endregion

    #region PrivateVariables
    [SerializeField] private GameObject m_knight;
    [SerializeField] private GameObject m_archer;
    [SerializeField] private GameObject m_wizard;
    [SerializeField] private GameObject m_none;
    #endregion

    #region PublicMethod
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F6))
            m_player.GetComponent<Player>().m_isGod = !m_player.GetComponent<Player>().m_isGod;

        if (Input.GetKeyDown(KeyCode.F1))
        {
            SetClass(PlayerClassType.None);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SetClass(PlayerClassType.Knight);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SetClass(PlayerClassType.Archer);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            SetClass(PlayerClassType.Wizard);
        }
#else
#endif



    }

    public GameObject GetPlayer()
    {
        return m_player;
    }

    public void SetPlayer(GameObject _obj)
    {
        m_player = _obj;
    }

    public void SetClass(PlayerClassType _type)
    {
        Vector3 spawnPos = m_player.transform.position;

        Destroy(m_player);
        
        switch(_type)
        {
            case PlayerClassType.None:
                Instantiate(m_none, spawnPos, Quaternion.identity);
                break;

            case PlayerClassType.Knight:
                Instantiate(m_knight, spawnPos, Quaternion.identity);
                break;

            case PlayerClassType.Archer:
                Instantiate(m_archer, spawnPos, Quaternion.identity);
                break;

            case PlayerClassType.Wizard:
                Instantiate(m_wizard, spawnPos, Quaternion.identity);
                break;
        }
        
        RoomManager.Instance.RemoveSelectedClassObject();
    }
    #endregion

    #region PrivateMethod
    #endregion
}
