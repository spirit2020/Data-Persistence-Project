using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }

    [System.Serializable]
    public class HighScoreEntry
    {
        public string playerName;
        public int score;
    }

    [System.Serializable]
    public class GameSettings
    {
        public int lineCount = 6;           // Default brick rows
        public float ballSpeed = 2.0f;      // Default ball launch force
        public bool soundEnabled = true;    // Sound toggle
    }

    [System.Serializable]
    class SaveData
    {
        public List<HighScoreEntry> highScores = new List<HighScoreEntry>();
        public GameSettings settings = new GameSettings();
    }

    // Public properties for high scores
    public int HighScore => (m_SaveData.highScores.Count > 0) ? m_SaveData.highScores[0].score : 0;
    public string HighScorerName => (m_SaveData.highScores.Count > 0) ? m_SaveData.highScores[0].playerName : "";
    public List<HighScoreEntry> HighScores => m_SaveData.highScores;

    // Public properties/variables for Settings
    public GameSettings Settings => m_SaveData.settings;
    public string PlayerName = "Player";

    private SaveData m_SaveData = new SaveData();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        LoadGameData();
    }

    public void AddNewScore(string playerName, int score)
    {
        if (score <= 0) return;

        bool exactMatchExists = m_SaveData.highScores.Exists(x => x.playerName == playerName && x.score == score);
        
        if (!exactMatchExists)
        {
            m_SaveData.highScores.Add(new HighScoreEntry { playerName = playerName, score = score });
        }

        m_SaveData.highScores.Sort((x, y) => y.score.CompareTo(x.score));

        if (m_SaveData.highScores.Count > 10)
        {
            m_SaveData.highScores.RemoveRange(10, m_SaveData.highScores.Count - 10);
        }

        SaveGameData();
    }

    public void ResetHighScores()
    {
        m_SaveData.highScores.Clear();
        SaveGameData();
    }

    // Save both high scores and settings to the same JSON file
    public void SaveGameData()
    {
        string json = JsonUtility.ToJson(m_SaveData, true);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadGameData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            m_SaveData = JsonUtility.FromJson<SaveData>(json) ?? new SaveData();
        }
    }
}