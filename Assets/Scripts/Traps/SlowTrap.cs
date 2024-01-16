using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrap : MonoBehaviour
{
    public float slowDuration = 8f; 
    public float slowFactor = 0.5f; 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                StartCoroutine(SlowDownPlayer(player)); 
               
            }
        }
    }

    IEnumerator SlowDownPlayer(Player player)
    {
        if (player != null)
        {
            player.ApplySpeedModifier(slowFactor); // Appliquer le ralentissement
            yield return new WaitForSeconds(slowDuration);

            // Retirer le ralentissement après la durée spécifiée
            player.RemoveSpeedModifier(slowFactor);
        }
    }
}
