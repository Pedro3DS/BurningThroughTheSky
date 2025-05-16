using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField] private GameObject deathCanvas;
    [SerializeField] private TMP_Text _cointText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text deathsText;
    [SerializeField] private TMP_Text scoreText;
    public GameObject leftCountEffect, rightCountEffect;
    [SerializeField] private TMP_Text pointsText;
    public Animator leftPointEffect, rightPointEffect;

    private Coroutine _pointAnimCoroutine;
    public Slider shieldSlider;
    [SerializeField] private TMP_Text shieldCountText;


    public static UiController Instance = null;
    private bool isShuttingDown = false;

    void OnApplicationQuit() => isShuttingDown = true;
    void OnDestroy() => isShuttingDown = true;
    void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(Instance);
    }
    void Start()
    {
        UpdateScore(0);
        UpdateShieldCount(PlayerPrefs.GetInt("Tiger_Shields"));
        int deaths = PlayerPrefs.GetInt("CurrentDeaths", 0);
        deathsText.text = $"{deaths}X";
    }
    void OnEnable()
    {
        Player.onPlayerGetCoint += UpdateCoin;
        // Player.onPlayerDie += ShowDeath;
        PointManager.onGetPoint += UpdatePoints;
    }

    void OnDisable()
    {
        Player.onPlayerGetCoint -= UpdateCoin;
        // Player.onPlayerDie -= ShowDeath;
        PointManager.onGetPoint -= UpdatePoints;
    }

    public void UpdateScore(int value)
    {
        scoreText.text = $"{value}";
    }

    public void UpdateDeaths(int value)
    {
        deathsText.text = $"{value}X";
    }

    public void UpdateCoin(int value)
    {
        _cointText.text = $"{CoinsManager.Instance.GetCoins()} X";
    }


    public void UpdateShieldBar(float value)
    {
        shieldSlider.value = value;
    }
    public void UpdateShieldCount(int count)
    {
        shieldCountText.text = $"{count}X";
    }

    public void UpdatePoints()
    {
        if (isShuttingDown) return;
        int targetPoints = PointManager.Instance.GetPoints();

        if (leftPointEffect != null && leftPointEffect.gameObject != null)
            leftPointEffect.SetTrigger("GetPoint");

        if (rightPointEffect != null && rightPointEffect.gameObject != null)
            rightPointEffect.SetTrigger("GetPoint");

        if (_pointAnimCoroutine != null)
            StopCoroutine(_pointAnimCoroutine);

        _pointAnimCoroutine = StartCoroutine(AnimatePoints(targetPoints));
    }

    IEnumerator AnimatePoints(int targetValue)
    {
        int currentValue = int.TryParse(pointsText.text, out var result) ? result : 0;
        float duration = 0.5f; // duração da animação
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            int newValue = (int)Mathf.Lerp(currentValue, targetValue, elapsed / duration);
            pointsText.text = newValue.ToString();
            yield return null;
        }

        pointsText.text = targetValue.ToString();
    }

    public void ShowCountdown(string message, Color color, float scale, float textSize)
    {
        countdownText.gameObject.SetActive(true);
        leftCountEffect.SetActive(true);
        rightCountEffect.SetActive(true);
        leftCountEffect.GetComponent<Image>().color = color;
        rightCountEffect.GetComponent<Image>().color = color;
        rightCountEffect.transform.localScale = new Vector3(scale, scale, scale);
        leftCountEffect.transform.localScale = new Vector3(scale, scale, scale);
        countdownText.color = color;
        countdownText.fontSize = textSize;
        countdownText.text = message;

    }

    public void HideCountdown()
    {
        countdownText.gameObject.SetActive(false);
        leftCountEffect.SetActive(false);
        rightCountEffect.SetActive(false);
    }
    // public int GetCoins()
    // {
    //     return coinAmount; // ou como você armazenar as moedas
    // }

}
