using System;
using UnityEngine;

public class ClearReward
{
    public string m_name;
    public string m_description;
    public Sprite m_sprite;
    public Action m_action;

    public ClearReward(string _name, string _description, Sprite _sprite, Action _action)
    {
        m_name = _name;
        m_description = _description;
        m_sprite = _sprite;
        m_action = _action;
    }
}
