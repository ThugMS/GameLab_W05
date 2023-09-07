using UnityEngine;

public class UIBossRoom : MonoBehaviour, IRoom
{
    private Boss m_boss;

    public void Init(Room _baseRoom)
    {
        var bossRoom = _baseRoom as BossRoom;
        m_boss = bossRoom.m_boss;
    }

    public void Execute()
    {
        // 보스 정보에 따른 GameObject를 생성
    }

    public void End()
    {
    }
}