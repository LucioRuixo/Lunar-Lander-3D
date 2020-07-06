using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public GameObject rocketModel;
    public ParticleSystem thrustPS;
    public ParticleSystem explosionPS;
    public ParticleSystem debrisPS;

    void OnEnable()
    {
        GameManager.onLevelSetting += ResetRocket;

        PlayerController.onThrustChange += SetParticleSystem;
        PlayerController.onLanding += CheckIfLandingFailed;
        PlayerController.onOutOfFuel += StopThrust;
    }

    void SetParticleSystem(bool play)
    {
        if (play)
            thrustPS.Play();
        else
            thrustPS.Stop();
    }

    void StopThrust()
    {
        if (thrustPS && thrustPS.isPlaying)
            thrustPS.Stop();
    }

    void ResetRocket()
    {
        if (explosionPS && explosionPS.isPlaying)
            explosionPS.Stop();

        if (debrisPS && debrisPS.isPlaying)
            debrisPS.Stop();

        if (rocketModel && !rocketModel.activeSelf)
            rocketModel.SetActive(true);
    }

    void CheckIfLandingFailed(bool landingSuccessful)
    {
        if (!landingSuccessful)
        {
            if (thrustPS.isPlaying) thrustPS.Stop();

            Explode();
        }
    }

    void Explode()
    {
        if (rocketModel && rocketModel.activeSelf)
        {
            rocketModel.SetActive(false);

            if (explosionPS) explosionPS.Play();
            if (debrisPS) debrisPS.Play();
        }
    }

    void OnDisable()
    {
        GameManager.onLevelSetting -= ResetRocket;

        PlayerController.onThrustChange -= SetParticleSystem;
        PlayerController.onLanding -= CheckIfLandingFailed;
        PlayerController.onOutOfFuel -= StopThrust;
    }
}