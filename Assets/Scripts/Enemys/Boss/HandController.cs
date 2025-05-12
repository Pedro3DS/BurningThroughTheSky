using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour
{
    public Transform player;
    public GameObject laserPrefab;
    public GameObject bombPrefab;

    public float life = 25f;

    public float dashSpeed = 10f;
    public float moveSpeed = 2f;
    public float dashCooldown = 3f;
    public float shootCooldown = 5f;
    public float rotationSpeed = 5f;

    private float dashTimer = 0f;
    private float shootTimer = 0f;

    private enum HandState { Idle, Dashing, Moving }
    private HandState currentState = HandState.Idle;

    private Vector3 moveTarget;
    private bool isDashing = false;

    [SerializeField] private GameObject explosion;
    [SerializeField] private AudioClip explosionSound;

    public delegate void OnHandDestroy();
    public static event OnHandDestroy onHandDestroy;
    public delegate void OnTakeDamage();
    public static event OnTakeDamage onTakeDamage;

    private bool _canTakeDamage = true;

    void Update()
    {
        dashTimer += Time.deltaTime;
        shootTimer += Time.deltaTime;

        switch (currentState)
        {
            case HandState.Idle:
                RotateTowardsPlayer(); // ← Rotaciona suavemente para o jogador

                if (dashTimer >= dashCooldown)
                {
                    ChooseAction();
                    dashTimer = 0f;
                }
                else if (shootTimer >= shootCooldown)
                {
                    Shoot();
                    shootTimer = 0f;
                }
                break;

            case HandState.Moving:
                transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, moveTarget) < 0.1f)
                {
                    currentState = HandState.Idle;
                }
                break;
        }
    }

    void ChooseAction()
    {
        float choice = Random.value;

        if (choice < 0.5f)
        {
            StartCoroutine(DashToPlayer());
        }
        else
        {
            MoveNearPlayer();
        }
    }

    IEnumerator DashToPlayer()
    {
        currentState = HandState.Dashing;
        isDashing = true;

        Vector3 target = player.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1f, 1f), 0);
        Vector3 startPos = transform.position;

        float t = 0f;
        float duration = 0.3f;

        while (t < duration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, target, t / duration);
            yield return null;
        }

        isDashing = false;
        currentState = HandState.Idle;
    }

    void MoveNearPlayer()
    {
        moveTarget = player.position + new Vector3(Random.Range(-3f, 3f), Random.Range(1f, 3f), 0);
        currentState = HandState.Moving;
    }

    void Shoot()
    {
        float chance = Random.value;
        if (chance < 0.3f)
        {
            Instantiate(laserPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(bombPrefab, transform.position, Quaternion.identity);
        }
    }

    void RotateTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    void DestroyHand(){
        Instantiate(explosion, transform.position, Quaternion.identity);
        AudioController.instance.PlayAudio(explosionSound);
        Destroy(gameObject);
    }
    public void TakeDamage(float amount)
    {
        if(life - amount <= 0 && _canTakeDamage){
            onHandDestroy?.Invoke();
            DestroyHand();
            return;
        }
        life -= amount;
        StartCoroutine(Damaged());
        // Aqui você pode implementar a vida da mão se quiser
    }

    IEnumerator Damaged(){
        _canTakeDamage = false;
        GetComponent<SpriteRenderer>().color = Color.red;
        onTakeDamage?.Invoke();
        yield return new WaitForSeconds(.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
        _canTakeDamage= true;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("NoteBullet")){
            other.GetComponent<Bullet>().DestroyThisBullet();
            TakeDamage(15);
        }
    }

}
