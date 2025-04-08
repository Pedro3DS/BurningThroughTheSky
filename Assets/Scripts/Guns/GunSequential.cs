using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunSequential : MonoBehaviour
{
    [SerializeField] private GameObject[] _bullets;
    [SerializeField] private Color[] _rdnCollors;
    private float _currentTime;
    private int _shootIndex = 0;
    public static GunSequential instance = null;
    [SerializeField ] private ShootData _shootData;
    void Awake()
    {
        if(!instance){
            instance = this;
        }else{
            Destroy(instance);
        }
    }
    public void Shoot(Transform target){
        if (_bullets == null) return;
        if (Time.time >= _currentTime)
        {
            _currentTime = Time.time + _shootData.cadence;
            if(_shootIndex + 1 >= _bullets.Length){
                _shootIndex = 0;
            }else{
                _shootIndex ++;
            }
            GameObject newBullet = Instantiate(_bullets[_shootIndex], target.position, target.rotation);
            newBullet.GetComponent<SpriteRenderer>().color = _rdnCollors[Random.Range(0, _rdnCollors.Length)];
            newBullet.GetComponent<Bullet>().damage = _shootData.minDamage;
            newBullet.GetComponent<Bullet>().bulletSum = FindAnyObjectByType<TigerMovement>().currentSpeed;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
