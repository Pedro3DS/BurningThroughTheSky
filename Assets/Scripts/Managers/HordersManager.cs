using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HordersManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private TigerMovement _player;
    [SerializeField] private TMP_Text hordeText;
    [SerializeField] private float spawnInterval = 1f;
    private float _addSpeed = 0;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool hordeActive = false;

    public static HordersManager Instance;

    void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);
    }

    public void StartHordeAtPoint(HordePoint point)
    {
        if (hordeActive) return;

        hordeActive = true;
        _player.SetInBoss(true);
        CameraFollow.Instance.LockCamera(true);

        StartCoroutine(HordeRoutine(point.enemiesToSpawn, point.enemy, point.speed));
    }

    IEnumerator HordeRoutine(int enemiesToSpawn, GameObject enemyPrefab, float speed)
    {
        hordeText.gameObject.SetActive(true);
        hordeText.text = $"Horda com {enemiesToSpawn} inimigos!";
        yield return new WaitForSeconds(2f);
        hordeText.gameObject.SetActive(false);
        _addSpeed = speed;
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemyAtScreenEdge(enemyPrefab);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemyAtScreenEdge(GameObject newEnemyPrefab)
    {
        Vector3 spawnPosition = GetRandomScreenEdgePosition();
        GameObject enemy = Instantiate(newEnemyPrefab, spawnPosition, Quaternion.identity);
        enemy.GetComponent<EnemySpaceShip>().SetManager(this);
        activeEnemies.Add(enemy);
    }

    Vector3 GetRandomScreenEdgePosition()
    {
        Camera cam = Camera.main;
        float z = Mathf.Abs(cam.transform.position.z);

        int edge = Random.Range(0, 4); // 0=left, 1=right, 2=top, 3=bottom
        Vector3 viewportPos = Vector3.zero;

        switch (edge)
        {
            case 0: viewportPos = new Vector3(0f, Random.Range(0f, 1f), z); break;
            case 1: viewportPos = new Vector3(1f, Random.Range(0f, 1f), z); break;
            case 2: viewportPos = new Vector3(Random.Range(0f, 1f), 1f, z); break;
            case 3: viewportPos = new Vector3(Random.Range(0f, 1f), 0f, z); break;
        }

        Vector3 worldPos = cam.ViewportToWorldPoint(viewportPos);
        worldPos.z = 0f;
        return worldPos;
    }

    public void NotifyEnemyDeath(GameObject enemy)
    {
        activeEnemies.Remove(enemy);

        if (activeEnemies.Count <= 0)
        {
            CameraFollow.Instance.smoothSpeed += _addSpeed;
            EndHorde();
            
        }
    }

    void EndHorde()
    {
        hordeActive = false;
        _addSpeed = 0;
        _player.SetInBoss(false);
        CameraFollow.Instance.LockCamera(false);
        CameraFollow.Instance.IncreaseSpeed(0.1f);
        Debug.Log("Horda finalizada!");
    }
}
