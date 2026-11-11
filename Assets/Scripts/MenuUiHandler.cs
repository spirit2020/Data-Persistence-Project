using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    public TMP_InputField NameInputField;       
    public TextMeshProUGUI MenuHighScoreText;   

    private void Start()
    {
        if (MainManager.Instance != null && MenuHighScoreText != null)
        {
            MenuHighScoreText.text = $"Best Score : {MainManager.Instance.HighScorerName} : {MainManager.Instance.HighScore}";
        }
    }

    public void StartNew()
    {
        if (MainManager.Instance != null && NameInputField != null)
        {
            if (!string.IsNullOrEmpty(NameInputField.text))
            {
                MainManager.Instance.PlayerName = NameInputField.text;
            }
        }

        SceneManager.LoadScene(1); // Load Game Scene
    }

    public void Exit()
    {
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #else
        Application.Quit();
        #endif
    }
}




