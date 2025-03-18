using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed = 5f;
    public float timeToDestroy = 1.4f;
    public GameObject explosion;
    private Vector3 targetPosition; 
    public bool shootFliped =false;

    void Start()
    {
        StartCoroutine(DestroyBullet());
    }
   
    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;
    }

    void Update()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.Translate(direction * bulletSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Destroy(gameObject);
        }
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
