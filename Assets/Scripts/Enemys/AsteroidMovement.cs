using System.Collections;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2f;

    public bool followPlayer = false;

    [SerializeField]
    private float _forwardDetectionRange = 5f;

    public Transform target;
    public int direction = 0; // 1 = cima, 0 = baixo

    private Rigidbody2D _rb2d;

    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (followPlayer && target != null)
        {
            Vector2 moveDirection = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            _rb2d.velocity = moveDirection * _speed;
        }
        else
        {
            // Move para cima ou para baixo
            if (direction == 1)
            {
                _rb2d.velocity = Vector2.up * _speed;
            }
            else
            {
                _rb2d.velocity = Vector2.down * _speed;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shoot"))
        {
            Destroy(gameObject);
        }
    }
}
