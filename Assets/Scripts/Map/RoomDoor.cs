using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomDoor : MonoBehaviour
{
    private UIRoom _mUIRoom;
    [SerializeField] private GameObject m_desactiveObj;
    [SerializeField] private Direction m_direction;
    private bool m_isOpen;

    public Transform spawnPosition;

    public void Init(UIRoom uiRoom, bool _isOpen)
    {
        _mUIRoom = uiRoom;
        m_isOpen = _isOpen; 
    }
    
    /// <summary>
    /// 클리어 시 오픈
    /// </summary>
    /// <param name="_isActive"></param>
    private void SetDoor(bool _isActive)
    {
        m_desactiveObj.SetActive(!_isActive);
    }
    
    public void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.CompareTag("Player"))
        {
            _mUIRoom.LeavedRoom(m_direction);
        }
    }
}
