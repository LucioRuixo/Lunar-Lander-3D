using System;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public PlayerController controller;

    bool gamePaused;

    public int scoreIncreaseBase;
    public float fuelBase;
    public float fuelMultiplier;

    [Header("Base stats: ")]
    public int score;

    public float fuel;
    public float height;
    public float horizontalSpeed;
    public float verticalSpeed;
    public float time;

    public static event Action<string, int> onStatUpdateI;
    public static event Action<string, float> onStatUpdateF;
    public static event Action<bool, int> onScoreUpdate;
    public static event Action<float, float> onStatResetOnNewLevel;

    void OnEnable()
    {
        GameManager.onLevelSetting += ResetStatsOnNewLevel;
        UIManager_Gameplay.onPauseChange += SetPause;

        PlayerController.onLanding += UpdateScore;
    }

    void Start()
    {
        fuel = fuelBase;
        height = controller.height;

        InitializeStatUI();
    }

    void Update()
    {
        if (!gamePaused)
        {
            fuel = controller.fuel;
            height = controller.height;
            horizontalSpeed = controller.horizontalSpeed;
            verticalSpeed = controller.verticalSpeed;
            time += Time.deltaTime;

            onStatUpdateF("Fuel", fuel);
            onStatUpdateF("Height", height);
            onStatUpdateF("Horizontal speed", horizontalSpeed);
            onStatUpdateF("Vertical speed", verticalSpeed);
            onStatUpdateF("Time", time);
        }
    }

    void OnDisable()
    {
        GameManager.onLevelSetting -= ResetStatsOnNewLevel;
        UIManager_Gameplay.onPauseChange -= SetPause;

        PlayerController.onLanding -= UpdateScore;
    }

    void SetPause(bool state)
    {
        gamePaused = state;
    }

    void InitializeStatUI()
    {
        if (onStatUpdateI != null)
            onStatUpdateI("Score", score);

        if (onStatUpdateF != null)
        {
            onStatUpdateF("Fuel", fuel);
            onStatUpdateF("Height", (int)height);
            onStatUpdateF("Horizontal speed", horizontalSpeed);
            onStatUpdateF("Vertical speed", verticalSpeed);
            onStatUpdateF("Time", time);
        }
    }

    void UpdateScore(bool landingSuccessful)
    {
        if (landingSuccessful)
            score += scoreIncreaseBase / (int)time;

        if (onScoreUpdate != null)
            onScoreUpdate(landingSuccessful, score);
    }

    void ResetStatsOnNewLevel()
    {
        time = 0f;
        fuelBase *= fuelMultiplier;
        fuel = fuelBase;
        controller.fuel = fuel;

        if (onStatResetOnNewLevel != null)
            onStatResetOnNewLevel(time, fuel);
    }
}