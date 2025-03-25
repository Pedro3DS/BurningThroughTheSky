using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Animator _anim;
    [SerializeField] private string _tagOnExplode = "BombExplosion";
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }
    public void Explode(){
        _anim.SetTrigger("Explode");
        gameObject.tag = _tagOnExplode;
    }
    public void DestroyBomb(){
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Shoot1")||collision.gameObject.CompareTag("RoarShoot")){
            Explode();
        }
    }
}
