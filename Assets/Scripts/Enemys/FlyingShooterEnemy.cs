using UnityEngine;

public class FlyingShooterEnemy : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction = new Vector2(1, -1); // Diagonal inferior direita
    public GameObject bulletPrefab;
    public float fireRate = 1f;

    private float fireTimer;

    void Start()
    {
        direction.Normalize(); // Garante que a velocidade seja constante
    }

    void Update()
    {
        // Movimento na diagonal
        transform.Translate(direction * speed * Time.deltaTime);

        // Atirar
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }

        // Verifica se saiu da tela (parte de baixo)
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPos.y < 0f)
        {
            Destroy(gameObject);
        }
    }

    void Shoot()
    {
        if (!bulletPrefab) return;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * 10f;
    }

    void OnDestroy()
    {
        if (PointManager.Instance != null)
            PointManager.Instance.SetPoints(150);
    }
}
