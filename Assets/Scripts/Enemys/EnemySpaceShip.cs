using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaceShip : MonoBehaviour
{
    public float distanceToShoot = 10f;
    public Transform playerPos;

    public GameObject enemyShoot;
    public float shootInterval = 2f;
    public int life = 3;
    public GameObject explosion;


    private float timeSinceLastShot = 0f;
    private Vector3 lastKnownPlayerPosition;
    private bool isFacingRight = true;
    private Player player;
    private bool playerAlived = true;
    [SerializeField] private AudioClip _destroyAudio;
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        Player.onPlayerDie += PlayerDied;
    }
    void PlayerDied(){
        playerAlived = false;
    }

    void Update()
    {
        if (playerAlived)
        {
            timeSinceLastShot += Time.deltaTime;

            if (playerPos.position.x < transform.position.x && isFacingRight)
            {
                Flip();
            }
            else if (playerPos.position.x > transform.position.x && !isFacingRight)
            {
                Flip();
            }

            lastKnownPlayerPosition = playerPos.position;

            Shooting();
        }
    }

    public void SpaceShipDestroy(){
        Destroy(gameObject);
    }
    void Shooting()
    {
        float distance = Vector2.Distance(transform.position, playerPos.position);
        if (distance <= distanceToShoot && timeSinceLastShot >= shootInterval)
        {
            //animator.SetBool("IsShooting", true); TODO: Arrumar animacao do tiro e sincronizar.
            GameObject bullet = Instantiate(enemyShoot, transform.position, Quaternion.identity);
            bullet.GetComponent<EnemyBullet>().SetTargetPosition(lastKnownPlayerPosition);
            timeSinceLastShot = 0f; // Reinicia o tempo desde o último tiro
        }
        else
        {
            //animator.SetBool("IsShooting", false);
        }
    }
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Quaternion newTransform = isFacingRight ? Quaternion.Euler(0f,0f,0f) : Quaternion.Euler(0f,-180f,0f);
        transform.rotation = newTransform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Shoot1"))
        {
            TakeDamage(1);
            CheckLife();
        }
    }
    void CheckLife(){
        if (life <= 0)
        {
            AudioController.instance.PlayAudio(_destroyAudio);
            GetComponent<Animator>().SetTrigger("Die");
        }
    }
    void TakeDamage(int damage){
        life -= damage;
    }
}
