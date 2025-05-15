using UnityEngine;
using System;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;

    public static Action onGetPoint;

    private int score = 0;
    private int deaths = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetPoints(int value)
    {
        score += value;
        onGetPoint?.Invoke();
    }

    public int GetPoints() => score;

    public void ResetPoints()
    {
        score = 0;
        onGetPoint?.Invoke();
    }

    public void AddDeath()
    {
        deaths++;
        UiController.Instance.UpdateDeaths(deaths); // Chama UI diretamente
    }

    public int GetDeaths() => deaths;
}
