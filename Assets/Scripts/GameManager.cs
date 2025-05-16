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
    [SerializeField] private AudioClip countDown;
    [SerializeField] private AudioClip endCountDown;

    public delegate void OnGameStarted();
    public static event OnGameStarted onGameStarted;


 
    public string gameType = "default";
    private bool _gameWin = false;

    void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(Instance);
    }

    void Start()
    {
        if(gameType == "default"){

            StartCoroutine(StartGameSequence());
        }else if(gameType == "boss"){
            isGameStarted = true;
        }
    }

    IEnumerator StartGameSequence()
    {
        uiController.ShowCountdown("3", countDownColors[0],countDownScale[0],countDownSize[0]);
        yield return new WaitForSeconds(1f);
        AudioController.instance.PlayAudio(countDown);
        uiController.ShowCountdown("2",countDownColors[1],countDownScale[1],countDownSize[1]);
        yield return new WaitForSeconds(1f);
        AudioController.instance.PlayAudio(countDown);
        uiController.ShowCountdown("1",countDownColors[2],countDownScale[2],countDownSize[2]);
        yield return new WaitForSeconds(1f);
        AudioController.instance.PlayAudio(countDown);
        uiController.ShowCountdown("VAI!",countDownColors[3],countDownScale[3],countDownSize[3]);
        yield return new WaitForSeconds(1f);
        AudioController.instance.PlayAudio(endCountDown);
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
    public void GameWin(){
        _gameWin = true;
        isGameStarted = false;
        ScoreManager.Instance.AddScore("AAAAA","AAAAA",PlayerPrefs.GetInt("CurrentPoints"), PlayerPrefs.GetInt("CurrentDeaths"));
    }
    void OnApplicationQuit()
    {
        DeleteKeys();
    }
    private void DeleteKeys(){
        PlayerPrefs.DeleteKey("CurrentP1");
        PlayerPrefs.DeleteKey("CurrentP2");
        PlayerPrefs.DeleteKey("CurrentPoints");
        PlayerPrefs.DeleteKey("CurrentDeaths");
    }
}
