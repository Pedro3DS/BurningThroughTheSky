using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField] private GameObject deathCanvas;
    [SerializeField] private TMP_Text _cointText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text pointsText;
    public Animator leftPointEffect, rightPointEffect;

    private Coroutine _pointAnimCoroutine;
    public Slider shieldSlider;

    public static UiController Instance = null;
    void Awake()
    {
        if(!Instance) Instance = this;
        else Destroy(Instance);
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

    // public void ShowDeath()
    // {
    //     deathCanvas.SetActive(true);
    // }

    public void UpdateCoin(int value)
    {
        _cointText.text = $"{CoinsManager.Instance.GetCoins()} X";
    }


    public void UpdateShieldBar(float value)
    {
        shieldSlider.value = value;
    }

    public void UpdatePoints()
    {
        int targetPoints = PointManager.Instance.GetPoints();
        leftPointEffect.SetTrigger("GetPoint");
        rightPointEffect.SetTrigger("GetPoint");
        if (_pointAnimCoroutine != null) StopCoroutine(_pointAnimCoroutine);
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

    public void ShowCountdown(string message)
    {
        countdownText.gameObject.SetActive(true);
        countdownText.text = message;
    }

    public void HideCountdown()
    {
        countdownText.gameObject.SetActive(false);
    }
    
}
