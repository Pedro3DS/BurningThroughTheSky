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
    private Transform[] secondFasePositions;
    [SerializeField]
    private Transform centerPosition;

    public float moveSpeed = 8f;
    public float secondMoveSpeed = 20f;
    private Transform targetPosition;
    private float waitTime = 1.2f;
    private float secondWaitTime = 0.5f;
    private float waitTimer;

    private bool _inSecondFase = false;
    private int _restHand = 2;
    private bool _canTakeDamage = true;

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
        float currentMoveSpeed = _inSecondFase ? secondMoveSpeed : moveSpeed;
        float currentTime = _inSecondFase ? secondWaitTime : waitTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, currentMoveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= currentTime)
            {
                waitTimer = 0f;
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
            if(!_inSecondFase)
            newTarget = positions[Random.Range(0, positions.Length)];
            else newTarget = secondFasePositions[Random.Range(0, positions.Length)];
        } while (newTarget == targetPosition);

        targetPosition = newTarget;
    }
    private void TakeHandDamage(){
        DamageBoss(1);
    }

    public void DamageBoss(float damage)
    {
        if(!_canTakeDamage) return;
        float previousHealth = currentHealth;
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;
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

        healthBar.SetHealth(to); // garantir que termine certinho
    }

    public void OnHandDestroyed()
    {
        _restHand--;
        if(_restHand <= 0){
          _inSecondFase = true;
          GetComponent<PolygonCollider2D>().isTrigger = false;   
        };
        DamageBoss(25);
    }
    IEnumerator Damaged(){
        _canTakeDamage = false;
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
        _canTakeDamage= true;
    }

    void Die()
    {
        Debug.Log("Boss derrotado");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(_inSecondFase && collision.gameObject.CompareTag("NoteBullet")){
            collision.gameObject.GetComponent<Bullet>().DestroyThisBullet();
            DamageBoss(1f);

        }
        
    }
}
