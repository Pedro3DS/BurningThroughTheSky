using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 5f;
    public float timeToDestroy = 1.4f;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        DestroyBullet();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * bulletSpeed * Time.deltaTime;
    }

    IEnumerable DestroyBullet(){
        Vector3 oldBulletPos = gameObject.transform.position;
        Destroy(gameObject, timeToDestroy);
        yield return new WaitForSeconds(timeToDestroy);
        GameObject newExplosion = Instantiate(explosion, oldBulletPos, Quaternion.identity);
        Destroy(newExplosion,1f);
    }
}
