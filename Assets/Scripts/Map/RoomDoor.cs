using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomDoor : MonoBehaviour
{
    public Transform m_spawnPosition;
    
    private UIRoom m_UIRoom;
    private RoomType m_roomType; // [TODO] 추후 문 타입에 따라, 문 모양 설정 필요
    [SerializeField] private GameObject m_desactiveObj;
    [SerializeField] private Direction m_direction;
    
    public void Init(UIRoom _uiRoom, RoomType _type)
    {
        m_UIRoom = _uiRoom;
        m_roomType = _type;

        m_desactiveObj.SetActive(m_roomType == default);
    }

    void OnTriggerStay2D(Collider2D _col)
    {
        if (_col.CompareTag("Player"))
        {
            m_UIRoom.LeaveRoom(m_direction);
        }
    }
}
