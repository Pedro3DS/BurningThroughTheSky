using UnityEngine;

public class HordeTrigger : MonoBehaviour
{
    [SerializeField] private HordersManager hordeManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hordeManager.StartHorde();
            gameObject.SetActive(false); // Impede múltiplas ativações
        }
    }
}
