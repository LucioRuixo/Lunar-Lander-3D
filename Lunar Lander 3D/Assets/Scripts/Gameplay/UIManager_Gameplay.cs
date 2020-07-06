using System;
using TMPro;
using UnityEngine;

public class UIManager_Gameplay : MonoBehaviour
{
    public TextMeshProUGUI fuelText;
    public TextMeshProUGUI heightText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI horizontalSpeedText;
    public TextMeshProUGUI verticalSpeedText;
    public TextMeshProUGUI timeText;
    public GameObject pauseMenu;
    public GameObject landingMenu;
    public TextMeshProUGUI landingText;

    public static event Action<bool> onPauseChange;

    void OnEnable()
    {
        PlayerModel.onStatUpdateI += UpdateStatI;
        PlayerModel.onStatUpdateF += UpdateStatF;
        PlayerModel.onScoreUpdate += UpdateScore;
        PlayerModel.onScoreUpdate += EnableLandingMenu;
        PlayerModel.onStatResetOnNewLevel += ResetStatsOnNewLevel;

    }

    void OnDisable()
    {
        PlayerModel.onStatUpdateI -= UpdateStatI;
        PlayerModel.onStatUpdateF -= UpdateStatF;
        PlayerModel.onScoreUpdate -= UpdateScore;
        PlayerModel.onScoreUpdate -= EnableLandingMenu;
        PlayerModel.onStatResetOnNewLevel -= ResetStatsOnNewLevel;

    }

    void UpdateStatI(string stat, int value)
    {
        if (stat == "Score")
            scoreText.text = "SCORE: " + value;
    }

    void UpdateStatF(string stat, float value)
    {
        switch (stat)
        {
            case "Fuel":
                if (fuelText) fuelText.text = "FUEL: " + (int)value;
                break;
            case "Height":
                if (heightText) heightText.text = "HEIGHT: " + value.ToString("0.0");
                break;
            case "Horizontal speed":
                if (horizontalSpeedText) horizontalSpeedText.text = value >= 0f ? "HORIZONTAL SPEED: " + (int)value + "  >" : "HORIZONTAL SPEED: " + (int)value * -1 + "  <";
                break;
            case "Vertical speed":
                if (verticalSpeedText) verticalSpeedText.text = value >= 0f ? "VERTICAL SPEED: " + (int)value + " /\\" : "VERTICAL SPEED: " + (int)value * -1 + " \\/";
                break;
            case "Time":
                if (timeText) timeText.text = "TIME: " + ((int)(value / 60)).ToString() + ":" + ((int)(value % 60)).ToString("00");
                break;
            default:
                break;
        }
    }

    void UpdateScore(bool landingSuccessful, int value)
    {
        if (scoreText && landingSuccessful)
            scoreText.text = "SCORE: " + value;
    }

    void ResetStatsOnNewLevel(float newTime, float newFuel)
    {
        if (timeText) timeText.text = "TIME: " + ((int)(newTime / 60)).ToString() + ":" + ((int)(newTime % 60)).ToString("00");
        if (fuelText) fuelText.text = "FUEL: " + (int)newFuel;
    }

    void EnableLandingMenu(bool landingSuccessful, int scoreDisplayValue)
    {
        if (!landingMenu) return;

        landingMenu.GetComponent<LandingMenu>().InitializeValues(landingSuccessful, scoreDisplayValue);
        landingMenu.SetActive(true);
    }

    public void SetPauseMenuActive()
    {
        if (!pauseMenu) return;

        pauseMenu.SetActive(!pauseMenu.activeSelf ? true : false);

        if (onPauseChange != null)
            onPauseChange(pauseMenu.activeSelf ? true : false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}