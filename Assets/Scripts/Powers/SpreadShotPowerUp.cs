using UnityEngine;

public class SpreadShotPowerUp : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tiger"))
        {
            GunSequential.instance.EnableSpreadShot();

            if (pickupSound != null)
            {
                AudioController.instance.PlayAudio(pickupSound);
            }

            Destroy(gameObject);
        }
    }
}
