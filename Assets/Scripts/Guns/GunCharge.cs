using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCharge : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private ShootData _shootData;
    private float _currentTime;
    public int minHeight = 0;
    public int maxHeight = 2;
    private float _chargeTime = 0f;
    private bool _charging = false;

    public static GunCharge instance = null;

    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _charging = true;
            _chargeTime += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (Time.time >= _currentTime && _bullet != null)
            {
                Transform target = transform; // ou outro transform de disparo, ex: firePoint
                GameObject newBullet = Instantiate(_bullet, target.position, target.rotation);

                float clampedCharge = Mathf.Clamp(_chargeTime, 0, _shootData.maxChargeTime);
                float chargePercent = clampedCharge / _shootData.maxChargeTime;

                float chargedDamage = Mathf.Lerp(_shootData.minDamage, _shootData.maxDamage, chargePercent);
                float chargedScale = Mathf.Lerp(minHeight, maxHeight, chargePercent);

                BulletCharge bulletScript = newBullet.GetComponent<BulletCharge>();
                bulletScript.transform.localScale = new Vector2(chargedScale, chargedScale);
                bulletScript.damage = (int)Mathf.Round(chargedDamage);
                bulletScript.bulletSum = FindAnyObjectByType<TigerMovement>().currentSpeed;

                _currentTime = Time.time + _shootData.cadence;
            }

            _charging = false;
            _chargeTime = 0f;
        }
    }
}
