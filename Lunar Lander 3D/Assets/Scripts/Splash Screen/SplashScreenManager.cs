using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreenManager : MonoBehaviour
{
    public float fadeWaitTime;
    public float fadeTime;
    public float displayTime;
    float alphaStep;

    Color logoColor;
    public Image logo;
    public TextMeshProUGUI developerName;

    void Start()
    {
        alphaStep = fadeWaitTime / fadeTime;

        logoColor = Color.white;
        logoColor.a = developerName.alpha = 0f;

        StartCoroutine(DisplayGameTitle(fadeWaitTime));
    }

    IEnumerator DisplayGameTitle(float waitTime)
    {
        for (float t = 0f; t < fadeTime; t += waitTime)
        {
            logoColor.a += alphaStep;
            if (logo) logo.color = logoColor;
            if (developerName) developerName.alpha += alphaStep;

            yield return new WaitForSeconds(waitTime);
        }

        yield return new WaitForSeconds(displayTime);

        for (float t = 0f; t < fadeTime; t += waitTime)
        {
            logoColor.a -= alphaStep;
            if (logo) logo.color = logoColor;
            if (developerName) developerName.alpha -= alphaStep;

            yield return new WaitForSeconds(waitTime);
        }

        SceneManager.LoadScene("Main Menu");
    }
}