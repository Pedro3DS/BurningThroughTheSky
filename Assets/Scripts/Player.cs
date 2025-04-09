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
    public delegate void OnPlayerGetCoint();
    public static event OnPlayerGetCoint onPlayerGetCoint;
    [SerializeField] private float shootCadence = 0.5f;
    private float _nextShoot = 0f;

    [SerializeField]
    private ControllersData controllData;
    private ControllersManager controller = new ControllersManager();
    
    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        controller.UpdateGamepadList();
        controller.GetGamepad(controllData.controllerIndex);
        Transition.onTransitionEnd += DestroyPlayer;

    }

    void Update()
    {
        // float rotationValue = -Input.GetAxis("PlayerHorizontal");
        float rotationValue = -controller.HorizontalMovement();

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
        if ((Input.GetKeyDown(KeyCode.Space) || controller.ShootAction()) && Mathf.Abs(_rotationZ) <= 180)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // GunSequential.instance.Shoot(gameObject.transform);
        
    }
    public void Die(){
        onPlayerDie?.Invoke();
        Player.onPlayerDie = null;
    }
    private void DestroyPlayer(){
        SceneController.instance.ChangeScene("Game2");
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(
            collision.gameObject.CompareTag("Asteroid") || 
            collision.gameObject.CompareTag("Dust") ||
            collision.gameObject.CompareTag("Enemy")
            ){
            Die();
        }
        if(collision.gameObject.CompareTag("Bomb")){
            collision.gameObject.GetComponent<Bomb>().Explode();
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("BombExplosion")){
            Die();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("EnemyShoot")){
            Die();
        }
        if(collision.gameObject.CompareTag("Coin")){
            Destroy(collision.gameObject);
            CoinsManager.Instance.SetCoins(1);
            onPlayerGetCoint?.Invoke();
            Player.onPlayerGetCoint = null;
        }
    }

}
