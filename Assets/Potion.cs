using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    #region PublicMethod
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealPlayer(collision.gameObject);
        }
    }

    protected virtual void HealPlayer(GameObject collidingObject)
    {
        if (collidingObject.CompareTag("Player"))
        {
            Player player;
            collidingObject.TryGetComponent<Player>(out player);
            
            if (PlayerManager.instance.GetPlayerCurHP() < PlayerManager.instance.GetPlayerMaxHP())
            {
                player.GetHeal(2);
                Destroy(gameObject);
            }
        }
    }
    #endregion
    
}