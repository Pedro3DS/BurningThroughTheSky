using UnityEngine;

public class FlyingShooterSpawner : MonoBehaviour
{
    public GameObject flyingEnemyPrefab;
    public Transform spawnPoint; // canto superior da tela, por exemplo

    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        Instantiate(flyingEnemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}