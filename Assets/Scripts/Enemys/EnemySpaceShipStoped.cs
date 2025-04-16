using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpaceShipStoped : MonoBehaviour
{
    public float distanceToShoot = 10f;
    public Transform playerPos;

    public GameObject enemyShoot;
    public float shootInterval = 2f;
    public int life = 3;

    private float _lastShot = 0f;
    private Vector3 _lastPlayerPos;
    private bool _isFacingRight = true;
    private bool playerAlived = true;
    [SerializeField] private AudioClip _destroyAudio;
    [SerializeField] private GameObject _lifeObject;
    private Slider _lifeSlider;
    private bool _firstDamage = true;
    void Start()
    {
        Player.onPlayerDie += PlayerDied;
        _lifeSlider = _lifeObject.GetComponent<Slider>();
        _lifeSlider.maxValue = life;
    }
    void PlayerDied(){
        playerAlived = false;
    }

    void Update()
    {
        if (playerAlived)
        {
            _lastShot += Time.deltaTime;

            // if (playerPos.position.x < transform.position.x && _isFacingRight)
            // {
            //     Flip();
            // }
            // else if (playerPos.position.x > transform.position.x && !_isFacingRight)
            // {
            //     Flip();
            // }

            _lastPlayerPos = playerPos.position;

            Shooting();
        }
    }

    public void SpaceShipDestroy(){
        
        Destroy(gameObject);
    }
    void Shooting()
    {
        float distance = Vector2.Distance(transform.position, playerPos.position);
        if (distance <= distanceToShoot && _lastShot >= shootInterval)
        {
            Vector2 direction = (playerPos.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject bullet = Instantiate(enemyShoot, transform.position, Quaternion.Euler(0f, 0f, angle));
            bullet.GetComponent<EnemyBullet>().SetDirection(direction);

            _lastShot = 0f;
        }
    }
    void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Quaternion newTransform = _isFacingRight ? Quaternion.Euler(0f,0f,0f) : Quaternion.Euler(0f,-180f,0f);
        transform.rotation = newTransform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("NoteBullet") || other.gameObject.CompareTag("RoarShoot"))
        {
            TakeDamage(other.gameObject.GetComponent<Bullet>().damage);
            CheckLife();
        }
    }
    void CheckLife(){
        if (life <= 0)
        {

            AudioController.instance.PlayAudio(_destroyAudio);
            _lifeObject.SetActive(false);
            CameraController.instance.ObjectDestroyed();
            GetComponent<Animator>().SetTrigger("Die");
        }
    }
    void TakeDamage(int damage){
        if(_firstDamage){
            _lifeObject.SetActive(true);
            _firstDamage = false;
        }
        life -= damage;
        _lifeSlider.value = life;
    }
    
}
