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

    void FixedUpdate()
    {
        HandleInput();
        Movement();
        ClampSpeed();
        ApplyTilt();
        UpdatePlayerPosition();
    }
    void Start()
    {
        Player.onPlayerDie += Die;
    }
    void OnDestroy()
    {
        Player.onPlayerDie -= Die;
        
    }

    void HandleInput()
    {
        _movementInput.x = Input.GetAxis("Horizontal");
        if(_movementInput.x > 0){
            Flip();
        }else{
            Flip();
        }
        _movementInput.y = Input.GetAxis("Vertical"); 
    }

    void Movement()
    {
        _rb2d.AddForce(_movementInput * _acceleration);
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

    void UpdatePlayerPosition()
    {
        _playerRb2d.position = _rb2d.position + (Vector2)_playerNewPos;
    }
    public void Die(){
        Destroy(gameObject);
    }
}
