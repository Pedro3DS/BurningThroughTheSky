using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordersManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyCount = 5;
    [SerializeField] private float spawnPadding = 1f;
    private List<GameObject> currentEnemies = new List<GameObject>();

    private Camera mainCamera;
    private bool hordeActive = false;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void StartHorde()
    {
        hordeActive = true;
        CameraFollow.Instance.LockCamera(true); // trava câmera

        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 spawnPos = GetRandomEdgePosition();
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            currentEnemies.Add(enemy);

            enemy.GetComponent<Enemy>().OnEnemyDeath += OnEnemyDeath;
        }
    }

    Vector2 GetRandomEdgePosition()
    {
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        Vector3 camPos = mainCamera.transform.position;
        int edge = Random.Range(0, 4); // 0 = top, 1 = bottom, 2 = left, 3 = right

        switch (edge)
        {
            case 0: return new Vector2(Random.Range(camPos.x - camWidth / 2, camPos.x + camWidth / 2), camPos.y + camHeight / 2 + spawnPadding);
            case 1: return new Vector2(Random.Range(camPos.x - camWidth / 2, camPos.x + camWidth / 2), camPos.y - camHeight / 2 - spawnPadding);
            case 2: return new Vector2(camPos.x - camWidth / 2 - spawnPadding, Random.Range(camPos.y - camHeight / 2, camPos.y + camHeight / 2));
            case 3: return new Vector2(camPos.x + camWidth / 2 + spawnPadding, Random.Range(camPos.y - camHeight / 2, camPos.y + camHeight / 2));
        }

        return camPos;
    }

    void OnEnemyDeath(GameObject enemy)
    {
        currentEnemies.Remove(enemy);

        if (currentEnemies.Count <= 0)
        {
            EndHorde();
        }
    }

    void EndHorde()
    {
        hordeActive = false;
        CameraFollow.Instance.LockCamera(false); // destrava a câmera
    }
}
