using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    private const string KEY = "ScoreBoard";
    public ScoreList scoreList = new ScoreList();
    public static ScoreManager Instance = null;

    void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);

        LoadScores();
    }

    public void AddScore(string name1, string name2, int score, int deaths)
    {
        ScoreEntry entry = new ScoreEntry
        {
            player1Name = name1,
            player2Name = name2,
            score = score,
            deaths = deaths
        };

        scoreList.entries.Add(entry);
        scoreList.entries = scoreList.entries
            .OrderByDescending(e => e.score)
            .ThenBy(e => e.deaths)
            .ToList();

        SaveScores();
    }

    public void SaveScores()
    {
        string json = JsonUtility.ToJson(scoreList);
        PlayerPrefs.SetString(KEY, json);
        PlayerPrefs.Save();
    }

    public void LoadScores()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string json = PlayerPrefs.GetString(KEY);
            scoreList = JsonUtility.FromJson<ScoreList>(json);
        }
    }

    public void ClearScores()
    {
        PlayerPrefs.DeleteKey(KEY);
        scoreList.entries.Clear();
    }

    public List<ScoreEntry> GetTopScores(int max = 10)
    {
        return scoreList.entries.Take(max).ToList();
    }

    [System.Serializable]
    public class ScoreEntry
    {
        public string player1Name;
        public string player2Name;
        public int score;
        public int deaths;
    }

    [System.Serializable]
    public class ScoreList
    {
        public List<ScoreEntry> entries = new List<ScoreEntry>();
    }
}
