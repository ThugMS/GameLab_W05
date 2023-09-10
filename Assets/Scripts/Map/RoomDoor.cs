using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomDoor : MonoBehaviour
{
    private BaseRoom m_baseRoom;
    private RoomType m_roomType;
    private Direction m_direction;
    private GameObject m_desactiveObj;
    
    private Animator m_animator;
    private GameObject m_animatorObj;
    
    public void Init(BaseRoom baseRoom, RoomType _roomType, Direction _direction)
    {
        m_baseRoom = baseRoom;
        m_roomType = _roomType;
        m_direction = _direction;

        m_desactiveObj = transform.Find("Disabled").gameObject;

        if (_roomType == RoomType.None)
        {
            m_desactiveObj.SetActive(true);
        }
        else
        {
            m_animator = GetComponentInChildren<Animator>();
            m_animatorObj = m_animator.gameObject;
            
            m_animator.gameObject.SetActive(true);
            m_desactiveObj.SetActive(!baseRoom.IsClear);
        }
    }

    public void CloseDoorAnime()
    {
        if (m_roomType != RoomType.None)
        {
            m_animator.Play($"{m_roomType}_Door_Close");
        }
    }
    
    public void OpenDoorAnime() 
    {
        if (m_roomType != RoomType.None)
        {
            var openRoom = m_roomType == RoomType.Start  ? RoomType.Normal : m_roomType;
            m_animator.Play($"{openRoom}_Door_Open");
        }
    }

    public void OpenDoorAfterAnime()
    {
        m_desactiveObj.SetActive(false);
    }
    
    public
    
    void OnTriggerStay2D(Collider2D _col)
    {
        if (m_roomType != RoomType.None)
        {
            if (_col.CompareTag("Player"))
            {
                m_baseRoom.LeaveRoom(m_direction);
            }
        }
    }
}
