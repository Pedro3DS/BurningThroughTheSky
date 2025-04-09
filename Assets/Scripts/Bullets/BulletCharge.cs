using System.Collections;
using UnityEngine;

public class BulletCharge : MonoBehaviour
{
    public float bulletSpeed = 5f;
    public int damage = 1;
    public float bulletSum = 1f;
    public float timeToDestroy = 1.4f;
    public GameObject explosion;

    private bool _launched = false;

    void Update()
    {
        if (_launched)
        {
            transform.position += transform.right * (bulletSpeed + bulletSum) * Time.deltaTime;
        }
    }

    public void Launch()
    {
        _launched = true;
        StartCoroutine(DestroyBullet());
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
