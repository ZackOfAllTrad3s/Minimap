using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public float accelerationPower = 1500f;
    public float brakingPower = 3000f;
    public float maxSpeed = 20f;

    private Rigidbody rb;
    private float currentAcceleration = 0f;
    private float currentBrakeForce = 0f;
    private bool isBraking = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Get input from the player
        currentAcceleration = Input.GetAxis("Vertical") * accelerationPower;
        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        if (isBraking)
        {
            ApplyBraking();
        }
        else
        {
            ApplyAcceleration();
        }

        // Clamp the velocity of the car to the max speed
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }

    private void ApplyAcceleration()
    {
        // Only apply acceleration force if the car is below max speed
        if (rb.velocity.sqrMagnitude < maxSpeed * maxSpeed)
        {
            rb.AddForce(transform.forward * currentAcceleration * Time.fixedDeltaTime);
        }
    }

    private void ApplyBraking()
    {
        currentBrakeForce = isBraking ? brakingPower : 0f;
        rb.AddForce(-transform.forward * currentBrakeForce * Time.fixedDeltaTime);
    }
}