using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCharge : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private ShootData _shootData;
    public float minHeight = 0;
    public float maxHeight = 0.5f;

    private float _currentTime;
    private float _chargeTime = 0f;
    private bool _charging = false;
    private GameObject _chargingBullet;

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
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= _currentTime)
        {
            StartCharging();
        }

        if (Input.GetKey(KeyCode.Space) && _charging)
        {
            _chargeTime += Time.deltaTime;
            UpdateChargingBullet();
        }

        if (Input.GetKeyUp(KeyCode.Space) && _charging)
        {
            ReleaseChargedBullet();
        }
    }

    void StartCharging()
    {
        _charging = true;
        _chargeTime = 0f;

        // Cria a bala carregando parada na frente do player
        _chargingBullet = Instantiate(_bullet, transform);
        _chargingBullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void UpdateChargingBullet()
    {
        if (_chargingBullet != null)
        {
            float clampedCharge = Mathf.Clamp(_chargeTime, 0, _shootData.maxChargeTime);
            float chargePercent = clampedCharge / _shootData.maxChargeTime;

            float scale = Mathf.Lerp(minHeight, maxHeight, chargePercent);
            _chargingBullet.transform.localScale = new Vector3(scale, scale, 1f);
        }
    }

    void ReleaseChargedBullet()
    {
        float clampedCharge = Mathf.Clamp(_chargeTime, 0, _shootData.maxChargeTime);
        float chargePercent = clampedCharge / _shootData.maxChargeTime;

        float chargedDamage = Mathf.Lerp(_shootData.minDamage, _shootData.maxDamage, chargePercent);

        if (_chargingBullet != null)
        {
            BulletCharge bulletScript = _chargingBullet.GetComponent<BulletCharge>();
            bulletScript.damage = (int)Mathf.Round(chargedDamage);
            bulletScript.bulletSum = FindAnyObjectByType<TigerMovement>().currentSpeed;
            bulletScript.Launch(); // Ativamos o movimento agora
        }

        _charging = false;
        _chargingBullet = null;
        _chargeTime = 0f;
        _currentTime = Time.time + _shootData.cadence;
    }
}
