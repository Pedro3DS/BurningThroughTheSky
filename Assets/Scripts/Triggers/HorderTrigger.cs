using UnityEngine;

public class HordeTrigger : MonoBehaviour
{
    [SerializeField] private HordersManager hordeManager;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            hordeManager.StartHorde();
            gameObject.SetActive(false); // desativa o trigger
        }
    }
}
