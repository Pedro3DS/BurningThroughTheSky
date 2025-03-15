using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    private Rigidbody2D _rb2d;
    private float _mousePos;

    void Awake()
    {
        _rb2d = gameObject.GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rotationValue = Input.GetAxis("Horizontal");
        // _rb2d.rotation = Input.GetAxis("Horizontal");
        if(Input.GetAxis("Horizontal") != 0){
            if(rotationValue < 0){
                    float rotation = -Input.GetAxis("Horizontal")* 180;
                    _rb2d.rotation =rotation;
                if(transform.rotation.y >= -180){
                    // _rb2d.rotation = rotation;
                    // if(rotation >= 90){
                    //     _rb2d.rotation = 0;
                    //     transform.rotation = Quaternion.Euler(transform.rotation.x,180f,transform.rotation.z);
                    // }
                }
                
            }else{
                float rotation = Input.GetAxis("Horizontal")* 180;
                    _rb2d.rotation =rotation;
            }
            // if(_rb2d.rotation < 0 || _rb2d.rotation > 180){

            // }

        }
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) {
            Shoot();
        }
        
    }
    void Shoot()
    {
        if (bullet == null) return;
        GameObject newBullet = Instantiate(bullet, _rb2d.transform.position, _rb2d.transform.rotation);
    }
}
