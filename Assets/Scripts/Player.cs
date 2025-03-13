using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    private Rigidbody2D _rb2d;

    void Awake()
    {
        _rb2d = gameObject.GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            Shoot();
        }
    }
    void Shoot()
    {
        if (bullet == null) return;
        GameObject newBullet = Instantiate(bullet, _rb2d.transform.position * Vector2.up, Quaternion.identity);
        Destroy(newBullet,3);
    }
}
