using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SettingsUIHandler : MonoBehaviour
{
    [Header("UI References")]
    public Slider LineCountSlider;         // Slider for brick rows (e.g., 4 to 8)
    public TextMeshProUGUI LineCountText;  // Text displaying current line count value
    
    public Slider BallSpeedSlider;         // Slider for ball speed (e.g., 1.0 to 5.0)
    public TextMeshProUGUI BallSpeedText;  // Text displaying current ball speed value
    
    public Toggle SoundToggle;             // Toggle for sound

    void Start()
    {
        LoadSettingsIntoUI();
    }

    void LoadSettingsIntoUI()
    {
        if (MainManager.Instance == null) return;

        var settings = MainManager.Instance.Settings;

        if (LineCountSlider != null)
        {
            LineCountSlider.value = settings.lineCount;
            UpdateLineCountText(settings.lineCount);
        }

        if (BallSpeedSlider != null)
        {
            BallSpeedSlider.value = settings.ballSpeed;
            UpdateBallSpeedText(settings.ballSpeed);
        }

        if (SoundToggle != null)
        {
            SoundToggle.isOn = settings.soundEnabled;
        }
    }

    // Hook this up to the Line Count Slider's OnValueChanged event in the Inspector
    public void OnLineCountChanged(float value)
    {
        Debug.Log("Slider moved! Value is: " + value); // Add this line
        int lines = Mathf.RoundToInt(value);
        MainManager.Instance.Settings.lineCount = lines;
        UpdateLineCountText(lines);
        MainManager.Instance.SaveGameData();
    }

    // Hook this up to the Ball Speed Slider's OnValueChanged event in the Inspector
    public void OnBallSpeedChanged(float value)
    {
        MainManager.Instance.Settings.ballSpeed = value;
        UpdateBallSpeedText(value);
        MainManager.Instance.SaveGameData();
    }

    // Hook this up to the Sound Toggle's OnValueChanged event in the Inspector
    public void OnSoundToggled(bool isEnabled)
    {
        MainManager.Instance.Settings.soundEnabled = isEnabled;
        MainManager.Instance.SaveGameData();
    }

    void UpdateLineCountText(int value)
    {
        if (LineCountText != null)
            LineCountText.text = $"Brick Rows: {value}";
    }

    void UpdateBallSpeedText(float value)
    {
        if (BallSpeedText != null)
            BallSpeedText.text = $"Ball Speed: {value:F1}";
    }

    // Hook this up to your "Back" Button's OnClick event
    public void BackToMenu()
    {
        SceneManager.LoadScene(0); // Index 0 = Menu Scene
    }
}
