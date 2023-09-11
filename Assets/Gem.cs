using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GemType
{
    HP,
    Power,
    Speed,
    None
}

public class Gem : MonoBehaviour
{
    [SerializeField] private GemType m_gemType;
    public GemType GemType => m_gemType;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player;
            collision.gameObject.TryGetComponent<Player>(out player);

            if (player.IsSameGem(m_gemType))
                return;
            
            player.OnGemTriggerEnter(this);
        }
    }
}
