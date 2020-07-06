using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerModel model;

    bool gamePaused;
    bool inputEnabled;
    bool landing;
    bool outOfFuel;

    public int minInitialX;
    public int maxInitialX;
    public int InitialY;
    public int fuelUsagePerSecond;
    int positionZ = 500;

    public float thrustForce;
    public float rotationForce;
    public float maxLandingVelocity;
    public float maxLandingAngle;
    [HideInInspector] public float height;
    [HideInInspector] public float horizontalSpeed;
    [HideInInspector] public float verticalSpeed;
    [HideInInspector] public float fuel;
    float velocity;
    float angle;
    float landingTimer;
    float landingTimerTarget;

    public Vector3 initialRotationEuler;

    public Rigidbody rb;

    public static event Action<bool> onThrustChange;
    public static event Action<bool> onLanding;
    public static event Action onOutOfFuel;

    void OnEnable()
    {
        GameManager.onLevelSetting += SetInitialValues;
        GameManager.onLevelSetting += EnableInput;

        UIManager_Gameplay.onPauseChange += SetPause;
    }

    void Start()
    {
        gamePaused = false;
        inputEnabled = true;
        landing = false;
        outOfFuel = false;

        height = GetHeight();
        landingTimerTarget = 3f;
        fuel = model.fuelBase;

        initialRotationEuler = new Vector3(0f, 180f, 0f);

        if (rb) rb.maxAngularVelocity = 3f;
    }

    void FixedUpdate()
    {
        if (!gamePaused && inputEnabled)
        {
            if (Input.GetButton("Thrust") && fuel > 0)
            {
                if (rb) rb.AddForce(transform.up * Input.GetAxis("Thrust") * thrustForce * Time.fixedDeltaTime, ForceMode.Acceleration);

                if (fuel > 0f)
                {
                    fuel -= (int)(fuelUsagePerSecond * Time.fixedDeltaTime);

                    if (fuel < 0f)
                        fuel = 0f;
                }
            }

            if (Input.GetButton("Rotate"))
                if (rb) rb.AddTorque(transform.forward * Input.GetAxis("Rotate") * rotationForce * Time.fixedDeltaTime, ForceMode.Force);

            if (landing && !(velocity > maxLandingVelocity || angle > maxLandingAngle))
            {
                landingTimer += Time.fixedDeltaTime;

                if (landingTimer >= landingTimerTarget)
                {
                    inputEnabled = false;
                    landing = false;
                    landingTimer = 0f;

                    if (onLanding != null)
                        onLanding(true);
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != null && collision.gameObject.tag == "Terrain")
        {
            if (velocity > maxLandingVelocity || angle > maxLandingAngle)
            {
                inputEnabled = false;
                landing = false;

                if (onLanding != null)
                    onLanding(false);
            }
            else
                landing = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag != null && collision.gameObject.tag == "Terrain")
        {
            landing = false;
            landingTimer = 0f;
        }
    }

    void Update()
    {
        if (gamePaused || !inputEnabled)
            return;

        if (fuel > 0)
        {
            if (Input.GetButtonDown("Thrust") && onThrustChange != null)
                onThrustChange(true);

            if (Input.GetButtonUp("Thrust") && onThrustChange != null)
                onThrustChange(false);
        }
        else if (!outOfFuel)
        {
            outOfFuel = true;
            if (onOutOfFuel != null)
                onOutOfFuel();
        }

        height = GetHeight();
        angle = Vector3.Angle(Vector3.up, transform.up);
        if (rb)
        {
            horizontalSpeed = rb.velocity.x;
            verticalSpeed = rb.velocity.y;
            velocity = rb.velocity.magnitude;
        }
    }

    void OnDisable()
    {
        GameManager.onLevelSetting -= SetInitialValues;
        GameManager.onLevelSetting -= EnableInput;

        UIManager_Gameplay.onPauseChange -= SetPause;
    }

    void SetPause(bool state)
    {
        gamePaused = state;

        if (!rb)
            return;

        if (!rb.IsSleeping())
            rb.Sleep();
        else
        {
            Vector3 velocity = new Vector3(horizontalSpeed, verticalSpeed, 0f);
            rb.velocity = velocity;

            rb.WakeUp();
        }
    }

    void EnableInput()
    {
        inputEnabled = true;
    }

    void SetInitialValues()
    {
        int initialX = UnityEngine.Random.Range(minInitialX, maxInitialX);
        Vector3 initialPosition = new Vector3(initialX, InitialY, positionZ);
        transform.position = initialPosition;

        transform.rotation = Quaternion.Euler(initialRotationEuler);

        if (rb)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    float GetHeight()
    {
        float rayDistance = 100f;

        Ray ray;
        RaycastHit raycastHit;

        ray = new Ray(transform.position, -Vector3.up);
        bool closeToSurface = Physics.Raycast(ray, out raycastHit, rayDistance);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.yellow);

        return closeToSurface ? raycastHit.distance : rayDistance;
    }
}