using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : MonoBehaviour
{
    private bool collected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.CompareTag("Tiger"))
        {
            collected = true;

            // Referencie o Tiger ou use evento para adicionar o shield
            TigerMovement tiger = other.GetComponent<TigerMovement>();
            AudioController.instance.PlayAudio(tiger.shieldColect); 
            // AudioController.instance.PlayAudio(tiger.)
            if (tiger != null)
            {
                tiger.CollectShield();
            }

            Destroy(gameObject); // destruir após pegar
        }
    }
}