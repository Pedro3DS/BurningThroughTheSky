using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSequential : MonoBehaviour
{
    [SerializeField] private GameObject[] _bullets;
    [SerializeField] private Color[] _rdnCollors;
    private float _currentTime;
    private int _shootIndex = 0;
    public static GunSequential instance = null;
    [SerializeField] private ShootData _shootData;

    [SerializeField] private bool hasSpreadShot = false;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Checa se o power-up já foi pego anteriormente
        if (PlayerPrefs.GetInt("HasSpreadShot", 0) == 1)
        {
            hasSpreadShot = true;
        }
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("HasSpreadShot", 0);
    }

    public void EnableSpreadShot()
    {
        hasSpreadShot = true;
        PlayerPrefs.SetInt("HasSpreadShot", 1);
        PlayerPrefs.Save();
    }

    public void Shoot(Transform target)
    {
        if (_bullets == null) return;

        if (Time.time >= _currentTime)
        {
            _currentTime = Time.time + _shootData.cadence;

            if (_shootIndex + 1 >= _bullets.Length)
            {
                _shootIndex = 0;
            }
            else
            {
                _shootIndex++;
            }

            if (hasSpreadShot)
            {
                float[] angles = { 0f, -10f, 10f }; // Central, esquerda e direita

                foreach (float angle in angles)
                {
                    Quaternion spreadRotation = target.rotation * Quaternion.Euler(0, 0, angle);
                    GameObject newBullet = Instantiate(_bullets[_shootIndex], target.position, spreadRotation);
                    SetupBullet(newBullet);
                }
            }
            else
            {
                GameObject newBullet = Instantiate(_bullets[_shootIndex], target.position, target.rotation);
                SetupBullet(newBullet);
            }
        }
    }

    private void SetupBullet(GameObject bullet)
    {
        var sprite = bullet.GetComponent<SpriteRenderer>();
        var bulletScript = bullet.GetComponent<Bullet>();

        if (sprite != null)
            sprite.color = _rdnCollors[Random.Range(0, _rdnCollors.Length)];

        if (bulletScript != null)
        {
            bulletScript.damage = _shootData.minDamage;
            bulletScript.bulletSum = FindAnyObjectByType<TigerMovement>().currentSpeed;
        }
    }
}
