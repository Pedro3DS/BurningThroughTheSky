using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public HandController leftHand;
    public HandController rightHand;

    public HealthBarUI healthBar;

    [SerializeField] private Transform[] positions;
    [SerializeField] private Transform[] secondFasePositions;
    [SerializeField] private Transform centerPosition;

    public GameObject laserPrefab;
    public GameObject knifePrefab;
    public GameObject enemyPrefab;
    public GameObject explosionPrefab;
    public float explosionInterval = 0.3f;
    public int explosionCount = 10;

    public float moveSpeed = 8f;
    public float secondMoveSpeed = 20f;
    private Transform targetPosition;
    private float waitTime = 1.2f;
    private float secondWaitTime = 0.5f;
    private float waitTimer;

    private bool _inSecondFase = false;
    private int _restHand = 2;
    private bool _canTakeDamage = true;

    private bool _isSpawningEnemies = false;
    private int _spawnedEnemies = 0;

    [SerializeField] private BackgroundSwitcher backgroundSwitcher;

    private bool _isDead = false;
    [SerializeField] private Transform targetPlayer;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        transform.position = centerPosition.position;
        ChooseNewPosition();
    }

    void OnEnable()
    {
        HandController.onHandDestroy += OnHandDestroyed;
        HandController.onTakeDamage += TakeHandDamage;
    }

    void OnDisable()
    {
        HandController.onHandDestroy -= OnHandDestroyed;
        HandController.onTakeDamage -= TakeHandDamage;
    }

    void Update()
    {
        if (_isDead) return;
        MoveToTarget();
    }

    void MoveToTarget()
    {
        if (targetPosition == null) return;

        float currentMoveSpeed = _isSpawningEnemies ? moveSpeed : (_inSecondFase ? secondMoveSpeed : moveSpeed);
        float currentTime = _inSecondFase ? secondWaitTime : waitTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, currentMoveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= currentTime)
            {
                waitTimer = 0f;

                if (_inSecondFase)
                {
                    StartCoroutine(SecondFaseAttackRoutine());
                }

                ChooseNewPosition();
            }
        }
    }

    void ChooseNewPosition()
    {
        if (positions.Length == 0 || secondFasePositions.Length == 0) return;

        Transform newTarget;
        do
        {
            newTarget = !_inSecondFase
                ? positions[Random.Range(0, positions.Length)]
                : secondFasePositions[Random.Range(0, secondFasePositions.Length)];
        } while (newTarget == targetPosition);

        targetPosition = newTarget;
    }

    IEnumerator SecondFaseAttackRoutine()
    {
        FireLaserOrKnife();
        yield return new WaitForSeconds(0.5f);

        if (!_isSpawningEnemies && Random.value < 0.5f)
        {
            StartCoroutine(SpawnEnemies(3));
        }
    }

    void FireLaserOrKnife()
    {
        // GameObject prefab = (Random.value < 0.5f) ? laserPrefab : knifePrefab;

        // Instantiate(prefab, transform.position, Quaternion.identity);

        Vector2 direction = (targetPlayer.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject bulletInstance = Instantiate((Random.value < 0.5f) ? laserPrefab : knifePrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
        bulletInstance.GetComponent<EnemyBullet>().SetDirection(direction);
    }

    IEnumerator SpawnEnemies(int count)
    {
        _isSpawningEnemies = true;
        _spawnedEnemies = count;

        for (int i = 0; i < count; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position + Vector3.down, Quaternion.identity);
            enemy.GetComponent<EnemySpaceShip>().onEnemyDeath += OnSpawnedEnemyDeath;
            yield return new WaitForSeconds(0.5f);
        }
    }

    void OnSpawnedEnemyDeath()
    {
        _spawnedEnemies--;

        if (_spawnedEnemies <= 0)
        {
            _isSpawningEnemies = false;
        }
    }

    private void TakeHandDamage()
    {
        DamageBoss(25);
    }

    public void DamageBoss(float damage)
    {
        if (!_canTakeDamage || _isDead) return;

        float previousHealth = currentHealth;
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        StartCoroutine(Damaged());
        StartCoroutine(AnimateHealthBar(previousHealth, currentHealth));

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator AnimateHealthBar(float from, float to)
    {
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float currentValue = Mathf.Lerp(from, to, elapsed / duration);
            healthBar.SetHealth(currentValue);
            elapsed += Time.deltaTime;
            yield return null;
        }

        healthBar.SetHealth(to);
    }

    public void OnHandDestroyed()
    {
        _restHand--;
        if (_restHand <= 0)
        {
            _inSecondFase = true;
            GetComponent<PolygonCollider2D>().isTrigger = false;
            if (backgroundSwitcher != null)
                backgroundSwitcher.StartSwitching();
        }

        DamageBoss(25);
    }

    IEnumerator Damaged()
    {
        _canTakeDamage = false;
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
        _canTakeDamage = true;
    }

    void Die()
    {
        Debug.Log("Boss derrotado");
        _isDead = true;
        StopAllCoroutines();
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(ExplosionSequence());
    }

    IEnumerator ExplosionSequence()
    {
        for (int i = 0; i < explosionCount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * 1.5f;
            Vector3 spawnPos = transform.position + new Vector3(offset.x, offset.y, 0);
            Instantiate(explosionPrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(explosionInterval);
        }

        Destroy(gameObject); // Ou você pode tocar uma animação de fim antes disso
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_inSecondFase && collision.CompareTag("NoteBullet"))
        {
            collision.GetComponent<Bullet>().DestroyThisBullet();
            DamageBoss(1f);
        }
    }
}
