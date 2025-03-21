using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed = 5f;
    public float timeToDestroy = 1.4f;
    public GameObject explosion;
    private Vector2 moveDirection;

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized; // Define a direção da bala
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle); // Ajusta a rotação
    }

    void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    void Update()
    {
        transform.position += (Vector3)moveDirection * bulletSpeed * Time.deltaTime;
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(timeToDestroy);
        if (explosion != null)
        {
            GameObject newExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(newExplosion, 1f);
        }
        Destroy(gameObject);
    }
}
