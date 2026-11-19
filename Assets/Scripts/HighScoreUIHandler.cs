using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HighScoreUIHandler : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI HighScoreListText; // Ensure this is assigned in the Inspector!

    void Start()
    {
        DisplayHighScores();
    }

    public void DisplayHighScores()
    {
        if (MainManager.Instance == null)
        {
            Debug.LogError("MainManager.Instance is missing!");
            return;
        }

        if (HighScoreListText == null)
        {
            Debug.LogError("HighScoreListText UI reference is missing in the Inspector!");
            return;
        }

        string scoresText = "<b>--- HIGH SCORES ---</b>\n\n";
        var scores = MainManager.Instance.HighScores;

        if (scores == null || scores.Count == 0)
        {
            scoresText += "No scores recorded yet!";
        }
        else
        {
            for (int i = 0; i < scores.Count; i++)
            {
                scoresText += $"{i + 1}. {scores[i].playerName} : {scores[i].score}\n";
            }
        }

        // Force update the text component immediately
        HighScoreListText.text = scoresText;
        HighScoreListText.SetAllDirty(); // Forces TextMeshPro to redraw immediately
    }

    // Hook this up to your "Reset" UI Button's OnClick event in the Inspector
    public void ResetScores()
    {
        if (MainManager.Instance != null)
        {
            Debug.Log("Resetting high scores...");
            MainManager.Instance.ResetHighScores();
            DisplayHighScores(); // Instantly refresh the UI text
        }
    }

    // Hook this up to your "Back" UI Button's OnClick event in the Inspector
    public void BackToMenu()
    {
        SceneManager.LoadScene(0); // Assumes Menu Scene is Build Index 0
    }
}