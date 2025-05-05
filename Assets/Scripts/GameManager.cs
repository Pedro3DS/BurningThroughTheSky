using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public bool isGameStarted { get; private set; } = false;

    [SerializeField] private UiController uiController;
    [SerializeField] private CameraFollow cameraFollow;

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
        uiController.ShowCountdown("3");
        yield return new WaitForSeconds(1f);
        uiController.ShowCountdown("2");
        yield return new WaitForSeconds(1f);
        uiController.ShowCountdown("1");
        yield return new WaitForSeconds(1f);
        uiController.ShowCountdown("VAI!");
        yield return new WaitForSeconds(1f);
        uiController.HideCountdown();

        yield return cameraFollow.IntroMove();

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
