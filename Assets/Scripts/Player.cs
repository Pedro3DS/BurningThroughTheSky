using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb2d;
    private float _rotationZ;
    public delegate void OnPlayerDie();
    public static event OnPlayerDie onPlayerDie;
    public delegate void OnPlayerGetCoint(int value);
    public static event OnPlayerGetCoint onPlayerGetCoint;

    [SerializeField] private float shootCadence = 0.5f;
    private float _nextShoot = 0f;

    [SerializeField]
    private ControllersData controllData;
    

    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
        // Transition.onTransitionEnd += DestroyPlayer;
    }
    

    void Update()
    {
        // if (!GameManager.Instance.isGameStarted) return;
        float rotationValue = -ControllersManager.Instance.HorizontalMovement(0);

        if (rotationValue != 0)
        {
            _rotationZ += rotationValue * 400f * Time.deltaTime;
            _rotationZ = Mathf.Clamp(_rotationZ, -80f, 80f);
            transform.rotation = Quaternion.Euler(0, 0, _rotationZ);
        }

        if ((Input.GetKeyDown(KeyCode.Space) || ControllersManager.Instance.ShootAction(0)) && Mathf.Abs(_rotationZ) <= 180)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GunSequential.instance.Shoot(transform);
    }

    public void Die()
    {
        DestroyPlayer();
        onPlayerDie?.Invoke();
        Player.onPlayerDie = null;
    }

    private void DestroyPlayer()
    {
        SceneController.instance.ChangeScene("Game");
    }

    public void GetMoney(int value)
    {
        onPlayerGetCoint?.Invoke(value);
    }
}
