using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public HandController leftHand;
    public HandController rightHand;

    public HealthBarUI healthBar;

    [SerializeField]
    private Transform[] positions;
    [SerializeField]
    private Transform centerPosition;

    public float moveSpeed = 1.5f;
    private Transform targetPosition;
    private float waitTime = 2f;
    private float waitTimer;

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
        MoveToTarget();
    }

    void MoveToTarget()
    {
        if (targetPosition == null) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                waitTimer = 0f;
                ChooseNewPosition();
            }
        }
    }

    void ChooseNewPosition()
    {
        if (positions.Length == 0) return;

        Transform newTarget;
        do
        {
            newTarget = positions[Random.Range(0, positions.Length)];
        } while (newTarget == targetPosition);

        targetPosition = newTarget;
    }
    private void TakeHandDamage(){
        DamageBoss(1);
    }

    public void DamageBoss(float damage)
    {
        float previousHealth = currentHealth;
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

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

        healthBar.SetHealth(to); // garantir que termine certinho
    }

    public void OnHandDestroyed()
    {
        DamageBoss(25);
    }

    void Die()
    {
        Debug.Log("Boss derrotado");
    }
}
