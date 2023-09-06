using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomDoor : MonoBehaviour
{
    #region PublicVariables
    public Transform m_spawnPosition;
    #endregion
    
    #region PrivateVariables
    private UIRoom m_UIRoom;
    private RoomType m_roomType;
    [SerializeField] private GameObject m_desactiveObj;
    [SerializeField] private Direction m_direction;
    #endregion

    #region PublicMethod
    public void Init(UIRoom _uiRoom, RoomType _type)
    {
        m_UIRoom = _uiRoom;
        m_roomType = _type;

        m_desactiveObj.SetActive(m_roomType == default);
    }
    #endregion

    #region PrivateMethod
    void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.CompareTag("Player"))
        {
            m_UIRoom.LeaveRoom(m_direction);
        }
    }
    #endregion
}
