using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb2d;
    private float _rotationZ;
    public delegate void OnPlayerDie();
    public static event OnPlayerDie onPlayerDie;
    [SerializeField] private float shootCadence = 0.5f;
    private float _nextShoot = 0f;
    
    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }
    void Start()
    {

    }

    void Update()
    {
        float rotationValue = -Input.GetAxis("PlayerHorizontal");

        if (rotationValue != 0)
        {
            // Calcula a rotação desejada no eixo Z
            _rotationZ += rotationValue * 200f * Time.deltaTime; // Multiplica para controlar a velocidade
            _rotationZ = Mathf.Clamp(_rotationZ, 0f, 180f); // Limita a rotação

            // Verifica se precisa inverter o eixo Y
            bool flipY = _rotationZ > 90f ;
            transform.rotation = Quaternion.Euler(0, 0, _rotationZ);
        }

        // Disparar somente se a rotação estiver entre 0 e 180 graus
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) && Mathf.Abs(_rotationZ) <= 180)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GunSequential.instance.Shoot(shootCadence, gameObject.transform);
        
    }
    public void Die(){
        onPlayerDie?.Invoke();
        Player.onPlayerDie = null;
        Destroy(gameObject);
    }

}
