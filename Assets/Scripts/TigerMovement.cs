using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TigerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private float baseSpeed = 10f;
    [SerializeField] private float speedIncreaseRate = 0.1f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float shootCadence = 0.5f;
    [SerializeField] private AudioClip roarSound;
    [SerializeField] private AudioClip shield;
    [SerializeField] private AudioClip colect;

    private float _nextShoot;
    private bool _canUseShield = true;
    [SerializeField] private int maxShields = 4;
    private int currentShields = 0;


    private Vector2 _input;
    private float _elapsedTime;
    private bool _inBoss = false;
    public float currentSpeed;

    [SerializeField] private Player _player;

    [Header("Shield")]
    [SerializeField] private Slider shieldSlider;
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private float shieldDuration = 5f;
    [SerializeField] private float shieldCooldown = 10f;

    private bool shieldActive = false;
    private bool shieldOnCooldown = false;
    public bool playerCanTakeDamage = true;
    [SerializeField] private GameObject explosion;
    void Start()
    {
        UiController.Instance.UpdateShieldCount(currentShields);
    }
    void FixedUpdate()
    {
        if (!GameManager.Instance.isGameStarted) return;

        _input = new Vector2(
            ControllersManager.Instance.HorizontalMovement(1),
            ControllersManager.Instance.VerticalMovement(1)
        );

        _elapsedTime += Time.fixedDeltaTime;
        float speed = Mathf.Min(baseSpeed + _elapsedTime * speedIncreaseRate, maxSpeed);

        if (!_inBoss)
            _rb2d.velocity = new Vector2(_input.x * speed, (_input.y * speed) + 5f);
        else
            _rb2d.velocity = _input * speed;

        currentSpeed = _rb2d.velocity.magnitude;
    }

    void Update()
    {
        if (!GameManager.Instance.isGameStarted) return;

        if (ControllersManager.Instance.ShootAction(1) && Time.time >= _nextShoot)
        {
            _nextShoot = Time.time + shootCadence;
            Instantiate(bullet, transform.position, Quaternion.identity);
            AudioController.instance.PlayAudio(roarSound);
        }

        // Ativar escudo se houver algum disponível, não estiver em uso e não estiver em cooldown
        if (ControllersManager.Instance.ShootAction(1) && currentShields >= 0 && !shieldActive && !shieldOnCooldown)
        {
            StartCoroutine(ActivateShield());
        }
        // if (Jo && currentShields >= 0 && !shieldActive && !shieldOnCooldown)
        // {
        //     StartCoroutine(ActivateShield());
        // }

        // Morrer se sair da tela
        if (Camera.main.WorldToViewportPoint(transform.position).y < 0f)
            _player.Die();
    }

    IEnumerator ActivateShield()
    {
        currentShields--;
        UiController.Instance.UpdateShieldCount(currentShields);
        AudioController.instance.PlayAudio(shield);

        shieldActive = true;
        playerCanTakeDamage = false;
        shieldOnCooldown = true;
        shieldObject.SetActive(true);
        shieldSlider.gameObject.SetActive(true);
        shieldSlider.maxValue = shieldDuration;
        shieldSlider.value = shieldDuration;

        // float elapsed = 0f;
        // while (elapsed < shieldDuration)
        // {
        //     elapsed += Time.deltaTime;
        //     shieldSlider.value = shieldDuration - elapsed;
        //     yield return null;
        // }
        for (int i = 0; i <= shieldDuration; i++)
        {
            shieldSlider.value = shieldDuration - i;
            yield return new WaitForSeconds(1);
            
        }
        shieldObject.SetActive(false);
        playerCanTakeDamage = true;
        shieldActive = false;

        shieldSlider.maxValue = shieldCooldown;
        shieldSlider.value = 0;
        
        for (int i = 0; i <= shieldCooldown; i++)
        {
            yield return new WaitForSeconds(i);
            shieldSlider.value += i;

        }


        shieldSlider.gameObject.SetActive(false);
        shieldOnCooldown = false;
    }


    public void SetInBoss(bool state)
    {
        _inBoss = state;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (playerCanTakeDamage)
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Asteroid") || other.CompareTag("EnemyShoot") || other.CompareTag("Dust"))
            {
                _player.Die();
            }
            
            if (other.CompareTag("Bomb"))
            {
                other.GetComponent<Bomb>().Explode();
            }
        }

        if (other.CompareTag("Coin"))
        {
            AudioController.instance.PlayAudio(colect);
            Destroy(other.gameObject);
            PointManager.Instance.SetPoints(100);
        }

        if (other.CompareTag("SpeedPickUp"))
        {
            Destroy(other.gameObject);
            maxSpeed += 2;
            CameraFollow.Instance.smoothSpeed += 1;
        }
        if (other.CompareTag("ShieldPickup"))
        {
            Destroy(other.gameObject);
            if (currentShields <= maxShields)
            {
                currentShields+=1;
                UiController.Instance.UpdateShieldCount(currentShields);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("BombExplosion"))
        {
            _player.Die();
        }
    }
}
