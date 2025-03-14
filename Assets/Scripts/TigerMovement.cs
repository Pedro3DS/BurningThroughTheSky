using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _target = 1f;
    public float currentSpeed;
    private Vector2 _movementInput;
    private float _driftForce;
    public Vector2 relativeForce;

    void FixedUpdate()
    {
 
        HandleInput();


        Movement();
   
        ClampSpeed();
    }

    void HandleInput()
    {
        _movementInput.x = Input.GetAxis("Horizontal");
        _movementInput.y = Input.GetAxis("Vertical"); 
    }

    void Movement()
    {
        _rb2d.AddForce(_movementInput * _acceleration);
        float rotationInput = _movementInput.y * _target;
        //AdjustRotation(rotationInput);
    }

    void AdjustRotation(float rotationInput)
    {
        // Calculate rotation based on the direction of movement
        float velocityMagnitude = _rb2d.velocity.magnitude;
        float rotationSpeed = rotationInput * (velocityMagnitude / _maxSpeed);

        if (_rb2d.velocity.magnitude >= 0f)
        {
            _rb2d.rotation += rotationSpeed;
        }
        else
        {
            _rb2d.rotation -= rotationSpeed;
        }
    }

    void ClampSpeed()
    {
        if (_rb2d.velocity.magnitude > _maxSpeed)
        {
            _rb2d.velocity = _rb2d.velocity.normalized * _maxSpeed;
        }

        currentSpeed = _rb2d.velocity.magnitude;
    }

    // Optional: Method for adding special effects (like a rainbow trail)
    // void AddMarc()
    // {
    //     GameObject newRainbow = Instantiate(Resources.Load("Rainbow")) as GameObject;
    //     newRainbow.transform.position = rainbowPoint.position;
    //     newRainbow.transform.rotation = rainbowPoint.rotation;
    //     Destroy(newRainbow, 5f); // Optional: destroy after 5 seconds
    // }
}
