using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public bool isGameStarted { get; private set; } = false;

    [SerializeField] private UiController uiController;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private Color[] countDownColors;
    [SerializeField] private float[] countDownScale;
    [SerializeField] private float[] countDownSize;
    [SerializeField] private GameObject explosionIntro;

    public delegate void OnGameStarted();
    public static event OnGameStarted onGameStarted;


    void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(Instance);
    }

    void Start()
    {
        StartCoroutine(StartGameSequence());
    }

    IEnumerator StartGameSequence()
    {
        uiController.ShowCountdown("3", countDownColors[0],countDownScale[0],countDownSize[0]);
        yield return new WaitForSeconds(1f);
        uiController.ShowCountdown("2",countDownColors[1],countDownScale[1],countDownSize[1]);
        yield return new WaitForSeconds(1f);
        uiController.ShowCountdown("1",countDownColors[2],countDownScale[2],countDownSize[2]);
        yield return new WaitForSeconds(1f);
        uiController.ShowCountdown("VAI!",countDownColors[3],countDownScale[3],countDownSize[3]);
        yield return new WaitForSeconds(1f);
        Instantiate(explosionIntro);
        uiController.HideCountdown();
        
        isGameStarted = true;
        onGameStarted?.Invoke();
        GameManager.onGameStarted = null;
    }

    public void GameOver()
    {
        isGameStarted = false;
        // uiController.ShowDeath();
    }
}
