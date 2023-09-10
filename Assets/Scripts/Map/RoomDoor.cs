using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomDoor : MonoBehaviour
{
    public Transform m_spawnPosition;
    
    private BaseRoom _mBaseRoom;
    private Direction m_direction;
    [SerializeField] private GameObject m_desactiveObj;
    
    public void Init(BaseRoom baseRoom, Direction _direction)
    {
        _mBaseRoom = baseRoom;
        m_direction = _direction;
        
        //m_desactiveObj.SetActive(m_roomType == default);
    }

    void OnTriggerStay2D(Collider2D _col)
    {
        if (_col.CompareTag("Player"))
        {
            _mBaseRoom.LeaveRoom(m_direction);
        }
    }
}
