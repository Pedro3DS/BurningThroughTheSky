using System.Collections;
using UnityEngine;

public class TigerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private Rigidbody2D _playerRb2d;
    [SerializeField] private Vector3 _playerNewPos;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _tiltAmount = 15f;
    [SerializeField] private float _tiltSpeed = 5f;
    private Vector2 _movementInput;
    public float currentSpeed;
    private bool fliped = false;
    public delegate void OnTigerDie();
    public static event OnTigerDie onTigerDie;

    #region Controllers
        [SerializeField]
        private ControllersData controllData;
        private ControllersManager controller =  new ControllersManager();
    #endregion

    #region Shoot
        [Header("Shoot Roar")]
        [SerializeField] private GameObject bullet;
        [SerializeField] private float shootCadence = 0.5f;
        [SerializeField] private AudioClip roarSound;
        private float _nextShoot = 0f;

    #endregion


    void FixedUpdate()
    {
        controller.UpdateGamepadList();
        ShootRoar();
        HandleInput();
        Movement();
        ClampSpeed();
        ApplyTilt();
        UpdatePlayerPosition();
    }
    void Start()
    {
        controller.UpdateGamepadList();
        controller.GetGamepad(controllData.controllerIndex);
        Player.onPlayerDie += Die;
    }

    void HandleInput()
    {
        _movementInput.x = controller.HorizontalMovement();
        if(_movementInput.x > 0){
            Flip();
        }else{
            Flip();
        }
        _movementInput.y = controller.VerticalMovement(); 
    }

    void Movement()
    {
        // _rb2d.AddForce(_movementInput * _acceleration);
        _rb2d.velocity = 
    }

    void ApplyTilt()
    {
        float targetTilt = _movementInput.y * _tiltAmount;
        float newRotationZ = Mathf.LerpAngle(transform.eulerAngles.z, targetTilt, _tiltSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, newRotationZ);
    }
    void Flip()
    {
        fliped = !fliped;
        Quaternion newTransform = fliped ? Quaternion.Euler(0f,0f,0f) : Quaternion.Euler(0f,-180f,0f);
        transform.rotation = newTransform;
    }

    void ClampSpeed()
    {
        if (_rb2d.velocity.magnitude > _maxSpeed)
        {
            _rb2d.velocity = _rb2d.velocity.normalized * _maxSpeed;
        }
        currentSpeed = _rb2d.velocity.magnitude;
        if(currentSpeed < 0){
            _rb2d.velocity = Vector2.zero;
        }
    }
    void ShootRoar(){
        if(Input.GetButtonDown("Fire2") && Time.time >= _nextShoot){
            _nextShoot = Time.time + shootCadence;
            AudioController.instance.PlayAudio(roarSound);
            Instantiate(bullet, transform.position, Quaternion.identity);
        }
    }

    void UpdatePlayerPosition()
    {
        _playerRb2d.position = _rb2d.position + (Vector2)_playerNewPos;
    }
    public void Die(){
        Destroy(gameObject);
    }
}
