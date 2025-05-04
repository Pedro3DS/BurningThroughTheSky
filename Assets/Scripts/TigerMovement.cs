using System.Collections;
using UnityEngine;

public class TigerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private Player _player;
    private Vector2 _movementInput;
    public float currentSpeed;

    public delegate void OnTigerDie();
    public static event OnTigerDie onTigerDie;

    [SerializeField]
    private ControllersData controllData;

    [Header("Shoot Roar")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private float shootCadence = 0.5f;
    [SerializeField] private AudioClip roarSound;
    private float _nextShoot = 0f;

    void Start()
    {
        Player.onPlayerDie += Die;
    }

    void FixedUpdate()
    {
        HandleInput();
        Movement();
        ShootRoar();
    }

    void HandleInput()
    {
        _movementInput.x = ControllersManager.Instance.HorizontalMovement(1);
        _movementInput.y = ControllersManager.Instance.VerticalMovement(1);

        // Impede voltar para trás (eixo Y negativo)
        if (_movementInput.y < 0)
            _movementInput.y = 0;
    }

    void Movement()
    {
        _rb2d.velocity = new Vector2(
            _movementInput.x * _acceleration,
            _movementInput.y * _acceleration
        );
        currentSpeed = _rb2d.velocity.magnitude;
    }

    void ShootRoar()
    {
        if (ControllersManager.Instance.ShootAction(1) && Time.time >= _nextShoot)
        {
            _nextShoot = Time.time + shootCadence;
            AudioController.instance.PlayAudio(roarSound);
            Instantiate(bullet, transform.position, Quaternion.identity);
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (
            collision.gameObject.CompareTag("Asteroid") ||
            collision.gameObject.CompareTag("Dust") ||
            collision.gameObject.CompareTag("Enemy")
        )
        {
            _player.Die();
        }

        if (collision.gameObject.CompareTag("Bomb"))
        {
            collision.gameObject.GetComponent<Bomb>().Explode();
        }

        if (collision.gameObject.CompareTag("EnemyShoot"))
        {
            _player.Die();
        }

        if (collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BombExplosion"))
        {
            Die();
        }
    }
}
