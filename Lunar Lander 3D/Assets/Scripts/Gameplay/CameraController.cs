using UnityEngine;

public class CameraController : MonoBehaviour
{
    bool pivotEnabled;

    public float offsetY;
    public float zoomMultiplier = 1f;
    public float minZoomDistance = 20f;
    public float maxZoomDistance = 1000f;

    public Vector3 rotationEuler;
    Vector3 offset;

    public Transform pivot;
    public PlayerController pivotController;

    void OnEnable()
    {
        GameManager.onLevelSetting += EnablePivot;

        PlayerController.onLanding += CheckIfLandingFailed;
    }

    void Start()
    {
        pivotEnabled = true;

        transform.rotation = Quaternion.Euler(rotationEuler);

        offset = Vector3.zero;
        offset.y = offsetY;
        if (pivotController) offset.z = Mathf.Clamp(pivotController.height * zoomMultiplier * -1f, -maxZoomDistance, -minZoomDistance);

        if (pivot) transform.position = pivot.position + offset;
    }

    void Update()
    {
        if (!pivotEnabled)
            return;

        if (transform.rotation.eulerAngles != rotationEuler)
            transform.rotation = Quaternion.Euler(rotationEuler);

        if (offset.y != offsetY)
            offset.y = offsetY;

        if (pivotController) offset.z = Mathf.Clamp(pivotController.height * zoomMultiplier * -1f, -maxZoomDistance, -minZoomDistance);
        if (pivot) transform.position = pivot.position + offset;
    }

    void OnDisable()
    {
        GameManager.onLevelSetting -= EnablePivot;

        PlayerController.onLanding -= CheckIfLandingFailed;
    }

    void EnablePivot()
    {
        if (!pivotEnabled)
            pivotEnabled = true;
    }

    void CheckIfLandingFailed(bool landingSuccessful)
    {
        if (!landingSuccessful)
            pivotEnabled = false;
    }
}