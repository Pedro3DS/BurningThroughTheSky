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
    private bool _gameStarted = false;



    [Header("Shield")]
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private Transform shieldSpawnPoint;
    [SerializeField] private float shieldDuration = 5f;
    [SerializeField] private float shieldCooldown = 10f;
    [SerializeField] private float boostedSpeed = 15f;
    [SerializeField] private float boostedCameraSpeed = 0.25f;

    private bool _shieldActive = false;
    private bool _canUseShield = true;
    private GameObject _currentShield;
    void Start()
    {
        Player.onPlayerDie += Die;
    }
    void OnEnable()
    {
        GameManager.onGameStarted += GameState;
    }
    private void GameState(){
        _gameStarted = true;
        Debug.Log("bnm,");
    }
    void FixedUpdate()
    {
        
        HandleInput();
        Movement();
        ShootRoar();
    }
    void Update()
    {
        // if (!GameManager.Instance.isGameStarted) return;
        ExitCamera();
        HandleShield();
    }
    void HandleShield()
    {
        if (ControllersManager.Instance.ShootAction(1) && _canUseShield)
        {
            StartCoroutine(ActivateShield());
        }
    }
    IEnumerator ActivateShield()
    {
        _canUseShield = false;
        _shieldActive = true;

        // Instancia o escudo
        _currentShield = Instantiate(shieldPrefab, shieldSpawnPoint.position, Quaternion.identity, transform);
        UiController.Instance.shieldSlider.value = 0;

        // Aumenta a velocidade
        float originalSpeed = _acceleration;
        float originalCameraSpeed = CameraFollow.Instance.smoothSpeed;
        _acceleration = boostedSpeed;
        CameraFollow.Instance.smoothSpeed = boostedCameraSpeed * 2000;

        float timer = shieldDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        // Reseta valores
        _acceleration = originalSpeed;
        CameraFollow.Instance.smoothSpeed = originalCameraSpeed;
        Destroy(_currentShield);
        _shieldActive = false;

        UiController.Instance.shieldSlider.maxValue = shieldCooldown;
        // Aguarda o cooldown
        for(int i = 0; i <= shieldCooldown; i++){

            UiController.Instance.shieldSlider.value += 1;
            // UiController.Instance.shieldSlider.colors += 1;
            yield return new WaitForSeconds(1);

        }
        _canUseShield = true;
    }


    void HandleInput()
    {
        _movementInput.x = ControllersManager.Instance.HorizontalMovement(1);
        _movementInput.y = ControllersManager.Instance.VerticalMovement(1);

        // Impede voltar para trás (eixo Y negativo)
        // if (_movementInput.y < 0)
        //     _movementInput.y = 0;
    }

    void Movement()
    {
        // if (!_gameStarted) return;
        _rb2d.velocity = new Vector2(
            _movementInput.x * _acceleration,
            _movementInput.y * _acceleration
        );
        currentSpeed = _rb2d.velocity.magnitude;
    }
    void ExitCamera(){
        // Clamping to camera
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewportPos.x < 0f) // saiu pela esquerda
            transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0f, viewportPos.y, Camera.main.nearClipPlane));

        if (viewportPos.x > 1f) // saiu pela direita
            transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1f, viewportPos.y, Camera.main.nearClipPlane));

        if (viewportPos.y > 1f) // saiu por cima
            transform.position = Camera.main.ViewportToWorldPoint(new Vector3(viewportPos.x, 1f, Camera.main.nearClipPlane));

        // Morre se ficou para trás (saiu da parte de baixo da tela)
        if (viewportPos.y < 0f)
        {
            _player.Die(); // ou _player.Die() dependendo do script
        }
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
            PointManager.Instance.SetPoints(100);
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
