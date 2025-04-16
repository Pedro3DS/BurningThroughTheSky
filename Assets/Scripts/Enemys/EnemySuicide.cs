using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySuicide : MonoBehaviour
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

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _rb2d = gameObject.GetComponent<Rigidbody2D>();

        Player.onPlayerDie += CheckPlayer;
    }

    void Update() {
        if (player != null) {
            float distanciaDoPlayer = Vector3.Distance(transform.position, player.position);

            if (distanciaDoPlayer < distance) {
                Vector3 direcao = (player.position - transform.position).normalized;
                _rb2d.velocity = direcao * speed * Time.deltaTime;
                // transform.position += direcao * speed * Time.deltaTime;
            } 
        }
    }

    public void SpaceShipDestroy(){
        
        Destroy(gameObject);
    }
    void CheckPlayer(){
        if(_playerAlived) _playerAlived = false;
    }
    void Shooting()
    {
        if (_lastShoot >= shootInterval)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject bullet = Instantiate(shoot, transform.position, Quaternion.Euler(0f, 0f, angle));
            bullet.GetComponent<EnemyBullet>().SetDirection(direction);

            _lastShoot = 0f;
        }
    }
    // void Flip()
    // {
    //     _isFacingRight = !_isFacingRight;
    //     Quaternion newTransform = _isFacingRight ? Quaternion.Euler(0f,0f,0f) : Quaternion.Euler(0f,-180f,0f);
    //     transform.rotation = newTransform;
    // }

    // private void OnTriggerEnter2D(Collider2D other)
    // {

    //     if (other.gameObject.CompareTag("NoteBullet") || other.gameObject.CompareTag("RoarShoot"))
    //     {
    //         TakeDamage(other.gameObject.GetComponent<Bullet>().damage);
    //         CheckLife();
    //     }
    // }
    // void CheckLife(){
    //     if (life <= 0)
    //     {

    //         AudioController.instance.PlayAudio(_destroyAudio);
    //         _lifeObject.SetActive(false);
    //         CameraController.instance.ObjectDestroyed();
    //         GetComponent<Animator>().SetTrigger("Die");
    //     }
    // }
    // void TakeDamage(int damage){
    //     if(_firstDamage){
    //         _lifeObject.SetActive(true);
    //         _firstDamage = false;
    //     }
    //     life -= damage;
    //     _lifeSlider.value = life;
    // }
    
}
