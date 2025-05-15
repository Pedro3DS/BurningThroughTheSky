using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordersManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private TigerMovement _player;
    [SerializeField] private List<int> hordeEnemyCounts = new List<int>() { 3, 5, 8 }; // quantidade de inimigos por horda
    [SerializeField] private float cameraSpeedBoost = 0.2f; // quanto a câmera aumenta por horda

    private List<GameObject> activeEnemies = new List<GameObject>();
    private int currentHordeIndex = 0;
    private bool hordeActive = false;

    public void StartHorde()
    {
        if (currentHordeIndex >= hordeEnemyCounts.Count)
        {
            Debug.Log("Todas as hordas concluídas.");
            return;
        }

        hordeActive = true;
        _player.SetInBoss(true);
        CameraFollow.Instance.LockCamera(true);

        SpawnEnemies(hordeEnemyCounts[currentHordeIndex]);
    }

    void SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPos = GetRandomSpawnPositionAroundCamera();
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            activeEnemies.Add(enemy);
            enemy.GetComponent<EnemySpaceShip>().SetManager(this);
        }
    }

    Vector3 GetRandomSpawnPositionAroundCamera()
    {
        float distanceFromCamera = 8f; // distância do spawn em relação à câmera
        Vector2[] directions = new Vector2[]
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        Vector2 randomDir = directions[Random.Range(0, directions.Length)];
        Vector3 cameraPos = Camera.main.transform.position;

        Vector3 spawnPos = cameraPos + (Vector3)randomDir * distanceFromCamera;

        // Adiciona variação
        spawnPos += new Vector3(
            Random.Range(-2f, 2f),
            Random.Range(-2f, 2f),
            0f
        );

        return spawnPos;
    }

    public void NotifyEnemyDeath(GameObject enemy)
    {
        activeEnemies.Remove(enemy);

        if (activeEnemies.Count <= 0)
        {
            EndHorde();
        }
    }

    void EndHorde()
    {
        hordeActive = false;
        _player.SetInBoss(false);
        CameraFollow.Instance.LockCamera(false);
        Debug.Log("Horda finalizada!");

        currentHordeIndex++;

        // Aumenta a velocidade da câmera gradualmente
        CameraFollow.Instance.IncreaseSpeed(cameraSpeedBoost);
    }
}
