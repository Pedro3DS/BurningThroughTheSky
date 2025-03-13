using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private float _maxSpeed, _acceleration, _target;
    public float currentSpeed;
    private Vector2 _speed;
    public float hori, vert;
    private float _aux;
    private float _driftForce;
    public Vector2 relativeForce;

    private List<GameObject> rainbows = new List<GameObject>();
    [SerializeField] private Transform rainbowPoint;
    // Start is called before the first frame update

    void FixedUpdate()
    {
            hori = -Input.GetAxis("Horizontal");


        // vert = Input.GetAxis("Vertical");

        _speed = transform.up * _acceleration;

        _rb2d.AddForce(_speed);

        _aux = Vector2.Dot(_rb2d.velocity, _rb2d.GetRelativeVector(Vector2.up));

        if (_aux >= 0.0f)
        {


            _rb2d.rotation += hori * _target * (_rb2d.velocity.magnitude / _maxSpeed * 0.8f);


        }
        else
        {

            _rb2d.rotation -= hori * _target * (_rb2d.velocity.magnitude / _maxSpeed * 0.8f);

        }
        _driftForce = Vector2.Dot(_rb2d.velocity, _rb2d.GetRelativeVector(Vector2.left)) * 2.0f;

        relativeForce = Vector2.right * _driftForce;

        _rb2d.AddForce(_rb2d.GetRelativeVector(relativeForce));

        if (_rb2d.velocity.magnitude > _maxSpeed)
        {

            _rb2d.velocity = _rb2d.velocity.normalized * _maxSpeed;

        }

        currentSpeed = _rb2d.velocity.magnitude;

        if (_rb2d.velocity.magnitude <= 4.9f)
        {
            AddMarc();
        }
    }
    void AddMarc()
    {
        GameObject newRainbow = GameObject.Instantiate(Resources.Load("Rainbow")) as GameObject;

        newRainbow.transform.position = rainbowPoint.position;
        newRainbow.transform.rotation = rainbowPoint.rotation;

        rainbows.Add(newRainbow);

        Destroy(newRainbow, 5f);
    }
}
