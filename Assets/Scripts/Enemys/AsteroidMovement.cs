using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AsteroidMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    public bool followPlayer = false;
    [SerializeField]
    private float _forwardDetectionRange = 5f;
    public Transform target;
    public int direction = 0;
    private Rigidbody2D _rb2d;


    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(followPlayer){
            Vector3 direction = (target.position - transform.position).normalized;
            _rb2d.velocity = direction * _speed * Time.deltaTime;
        }else{
            // Move Right
            if(direction == 1){
                _rb2d.velocity = Vector2.right * _speed;
            }else{
                _rb2d.velocity = Vector2.left * _speed;
            }

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Shoot")){
            Destroy(gameObject);
        }
    }
}
