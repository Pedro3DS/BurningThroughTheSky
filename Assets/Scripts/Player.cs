using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb2d;
    private float _rotationZ;
    public delegate void OnPlayerDie();
    public static event OnPlayerDie onPlayerDie;
    public delegate void OnPlayerGetCoint(int value);
    public static event OnPlayerGetCoint onPlayerGetCoint;
    [SerializeField] private bool _inBoss = false;

    [SerializeField] private float shootCadence = 0.5f;
    private float _nextShoot = 0f;

    [SerializeField]
    private ControllersData controllData;
    [SerializeField]
    private GameObject dieTransition;

    [SerializeField] private AudioClip[] dieAudios;
    private bool _playerDied = false;
    [SerializeField] private GameObject explosion;

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
        // float rotationValue = -ControllersManager.Instance.HorizontalMovement(0);

        float rotationValue = -ControllersManager.Instance.HorizontalMovement(0);

        if (rotationValue != 0)
        {
            transform.Rotate(0, 0, rotationValue * 400f * Time.deltaTime);
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
        if (_playerDied) return;

        Instantiate(explosion);
        _playerDied = true;

        AudioController.instance.PlayAudio(dieAudios[UnityEngine.Random.Range(0,dieAudios.Length-1)]);

        PointManager.Instance.AddDeath();
        SetDeath();
        PointManager.Instance.ResetPoints();

        DestroyPlayer();
        onPlayerDie?.Invoke();
        Player.onPlayerDie = null;
    }

    private void SetDeath()
    {
        if (PlayerPrefs.HasKey("CurrentDeaths"))
        {
            int deaths = PlayerPrefs.GetInt("CurrentDeaths");
            PlayerPrefs.SetInt("CurrentDeaths", deaths + 1);
        }
        else
        {
            PlayerPrefs.SetInt("CurrentDeaths", 1);

        }
        PlayerPrefs.Save();
    }

    private void DestroyPlayer()
    {

        TransitionController.Instance.LoadTransition(dieTransition, _inBoss?"Boss": "Game");
        // SceneController.instance.ChangeScene("Game");
    }

    public void GetMoney(int value)
    {
        onPlayerGetCoint?.Invoke(value);
    }
}
