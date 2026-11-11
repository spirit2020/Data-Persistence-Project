using System.IO;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }

    // Public properties for high score and player name
    public int HighScore => m_HighScore;
    public string HighScorerName => m_HighScorerName;

    public string PlayerName = "Player";
    
    private int m_HighScore = 0;
    private string m_HighScorerName = "";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        LoadHighScoreData();
    }

    public void CheckAndUpdateHighScore(int currentScore)
    {
       if (currentScore > m_HighScore)
        {
            m_HighScore = currentScore;
            m_HighScorerName = PlayerName;
            SaveHighScoreData();
        }
    }

    [System.Serializable]
    class SaveData
    {
        public int highscore;
        public string playerName;
    }

    public void SaveHighScoreData()
    {
        SaveData data = new SaveData();
        data.highscore = m_HighScore;
        data.playerName = m_HighScorerName;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScoreData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            m_HighScore = data.highscore;
            m_HighScorerName = data.playerName;
        }
    }
}