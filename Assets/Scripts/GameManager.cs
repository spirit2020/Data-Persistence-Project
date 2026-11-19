using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Game References")]
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    [Header("Local UI References")]
    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    public GameObject HighScoresButton; // Drag your View High Scores button object here

    private bool m_Started = false;
    private int m_Points = 0;
    private bool m_GameOver = false;
    private InputAction m_LaunchAction;

    void Awake()
    {
        m_LaunchAction = new InputAction("Launch", InputActionType.Button, "<Keyboard>/space");
    }

    void OnEnable() => m_LaunchAction.Enable();
    void OnDisable() => m_LaunchAction.Disable();

  void Start()
    {
        Debug.Log("GameManager Start() running! Resetting UI.");

        m_Started = false;
        m_GameOver = false;
        m_Points = 0;

        if (GameOverText != null) GameOverText.SetActive(false);
        if (HighScoresButton != null) HighScoresButton.SetActive(false);

        UpdateHighScoreUI();
       
        // Apply Line Count from MainManager Settings
        if (MainManager.Instance != null)
        {
            LineCount = MainManager.Instance.Settings.lineCount;
        }

        SpawnBricks();

        if (ScoreText != null) ScoreText.text = "Score : 0";
    }

    void Update()
    {
        if (!m_Started)
        {
            if (m_LaunchAction.WasPressedThisFrame())
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0).normalized;

                // Apply Ball Speed from MainManager Settings
                float speed = (MainManager.Instance != null) ? MainManager.Instance.Settings.ballSpeed : 2.0f;

                if (Ball != null)
                {
                    Ball.transform.SetParent(null);
                    Ball.AddForce(forceDir * speed, ForceMode.VelocityChange);
                }
            }
        }
        else if (m_GameOver)
        {
            if (m_LaunchAction.WasPressedThisFrame())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            UpdateHighScoreUI();
        }
    }

    void SpawnBricks()
    {
        if (BrickPrefab == null) return;

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        int[] pointCountArray = new [] {1,1,2,2,5,5};

        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
               // Using modulo (%) ensures that if LineCount exceeds the array size, 
                // it safely wraps around and reuses the point values without crashing.
                int pointIndex = i % pointCountArray.Length;
                brick.PointValue = pointCountArray[pointIndex];
                
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

   

    void AddPoint(int point)
    {
        // Prevent adding points if game is already over
        if (m_GameOver) return;

        m_Points += point;
        if (ScoreText != null) ScoreText.text = $"Score : {m_Points}";

        // Update High Score via MainManager using AddNewScore
      //  if (MainManager.Instance != null)
       // {
         //   MainManager.Instance.AddNewScore(MainManager.Instance.PlayerName, m_Points);
        // }
        UpdateHighScoreUI();
    }

    void UpdateHighScoreUI()
    {
        if (HighScoreText != null && MainManager.Instance != null)
        {
            HighScoreText.text = $"Best Score : {MainManager.Instance.HighScorerName} : {MainManager.Instance.HighScore}";
        }
    }

    public void GameOver()
    {
        if (m_GameOver) return; 

        m_GameOver = true;
        
        if (GameOverText != null) GameOverText.SetActive(true);

        // Show the View High Scores button when game is over
        if (HighScoresButton != null)
        {
            HighScoresButton.SetActive(true);
        }

        if (MainManager.Instance != null)
        {
            // Submit final score to the leaderboard list
            MainManager.Instance.AddNewScore(MainManager.Instance.PlayerName, m_Points);
        }
    }

    // Hook this method up to your UI Button's OnClick event in the Inspector
    public void BackToMeno0()
    {
        // Change '2' to match the actual Build Index of your High Score Scene
        SceneManager.LoadScene(0); 
    }
}
