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

        // Explicitly reset game states on start/restart
        m_Started = false;
        m_GameOver = false;
        m_Points = 0;

        // 1. Force hide Game Over text on start/restart
        if (GameOverText != null) 
        {
            GameOverText.SetActive(false);
        }
        else
        {
            Debug.LogWarning("GameOverText reference is missing in the Inspector!");
        }
       

        UpdateHighScoreUI();
       
        // Spawn Bricks
        SpawnBricks();

        if (ScoreText != null) ScoreText.text = "Score : 0";
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
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
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

                if (Ball != null)
                {
                    Ball.transform.SetParent(null);
                    Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
                }
            }
        }
        else if (m_GameOver)
        {
            if (m_LaunchAction.WasPressedThisFrame())
            {
                Debug.Log("Space pressed on Game Over. Reloading scene...");
                // Reload game scene to restart
                
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
             UpdateHighScoreUI();
        }
    }

    void AddPoint(int point)
    {
        // Prevent adding points if game is already over
        if (m_GameOver) return;

        m_Points += point;
        if (ScoreText != null) ScoreText.text = $"Score : {m_Points}";

        // Update High Score via MainManager
        if (MainManager.Instance != null)
        {
            MainManager.Instance.CheckAndUpdateHighScore(m_Points);
        }
       
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
        if (m_GameOver) return; // Prevent multiple triggers

        m_GameOver = true;
        if (GameOverText != null) GameOverText.SetActive(true);

        if (MainManager.Instance != null)
        {
            MainManager.Instance.CheckAndUpdateHighScore(m_Points);
        }
    }
}
