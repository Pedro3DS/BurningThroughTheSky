using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpaceShip : MonoBehaviour
{
    public float speed = 3.0f;
    public float distance = 5.0f;
    public GameObject shoot;
    public float shootInterval = 2.0f;
    public Transform shootPoints;

    private Transform player;
    private float _lastShoot = 0f;
    private Rigidbody2D _rb2d;
    private bool _playerAlived = true;
    private bool _playerDetected = false;

    [Header("Life")]
    public int life = 1;
    public Slider _lifeSlider;
    public GameObject _lifeObject;
    private bool _firstDamage = true;

    [Header("Audio")]
    public AudioClip _destroyAudio;
    private HordersManager _manager;

    [SerializeField] private GameObject explosion;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        _rb2d = GetComponent<Rigidbody2D>();
        Player.onPlayerDie += CheckPlayer;
    }

    void Update()
    {
        if (!_playerAlived || !_playerDetected || player == null) return;

        _lastShoot += Time.deltaTime;

        float distanciaDoPlayer = Vector3.Distance(transform.position, player.position);

        if (distanciaDoPlayer < distance)
        {
            _rb2d.velocity = Vector2.zero;
            Shooting();
        }
        else
        {
            Vector3 direcao = (player.position - transform.position).normalized;
            _rb2d.velocity = direcao * speed;
        }

        // Rotaciona para olhar o player
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnBecameVisible()
    {
        _playerDetected = true;
    }

    public void SpaceShipDestroy()
    {
        Destroy(gameObject);
    }

    void CheckPlayer()
    {
        _playerAlived = false;
    }

    void Shooting()
    {
        if (_lastShoot >= shootInterval)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            GameObject bulletInstance = Instantiate(shoot, shootPoints.position, Quaternion.Euler(0f, 0f, angle));
            bulletInstance.GetComponent<EnemyBullet>().SetDirection(direction);

            _lastShoot = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("NoteBullet"))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(1);
                bullet.DestroyThisBullet();
                CheckLife();
            }
        }
    }
    public void SetManager(HordersManager manager)
    {
        _manager = manager;
    }
    public void NotifyDeath()
    {
        if (_manager != null)
            _manager.NotifyEnemyDeath(gameObject);
    }
    void TakeDamage(int damage)
    {
        if (_firstDamage)
        {
            _lifeObject.SetActive(true);
            _firstDamage = false;
        }
        life -= damage;

        _lifeSlider.value = life;
    }

    void CheckLife()
    {
        if (life <= 0)
        {
            AudioController.instance.PlayAudio(_destroyAudio);
            _lifeObject.SetActive(false);
            // CameraController.instance.ObjectDestroyed();
            NotifyDeath();

            // Notifica o manager da morte
            if (_manager)
                _manager?.NotifyEnemyDeath(gameObject);

            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
            // GetComponent<Animator>().SetTrigger("Die");
        }
    }
    
}
