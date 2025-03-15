using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 5f;
    public float timeToDestroy = 1.4f;
    public GameObject explosion;

    void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    void Update()
    {
        transform.position += transform.right * bulletSpeed * Time.deltaTime; 
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
