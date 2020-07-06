using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LandingMenu : MonoBehaviour
{
    bool landingSuccessful;

    int score;

    public Button button;
    public TextMeshProUGUI landingText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI buttonText;

    public static event Action onLevelSetting;

    void OnEnable()
    {
        if (landingSuccessful)
        {
            if (landingText) landingText.text = "YOUR SHIP HAS LANDED!";
            if (scoreText) scoreText.text = "SCORE: " + score;
            if (buttonText) buttonText.text = "NEXT LEVEL";

            if (button) button.onClick.AddListener(SetLevel);
        }
        else
        {
            if (landingText) landingText.text = "YOUR SHIP CRASHED!";
            if (scoreText) scoreText.text = "SCORE: " + score;
            if (buttonText) buttonText.text = "RETURN TO MAIN MENU";

            if (button) button.onClick.AddListener(ReturnToMainMenu);
        }
    }

    void SetLevel()
    {
        if (onLevelSetting != null)
            onLevelSetting();

        gameObject.SetActive(false);
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void InitializeValues(bool newLandingSuccessful, int newScore)
    {
        landingSuccessful = newLandingSuccessful;
        score = newScore;
    }
}