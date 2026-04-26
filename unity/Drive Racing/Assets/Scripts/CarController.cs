using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class CarController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    [Header("3D Wheel Meshes")]
    public Transform meshFL;
    public Transform meshFR;
    public Transform meshRL;
    public Transform meshRR;

    [Header("Car Settings")]
    public float motorTorque = 2000f;
    public float steeringAngle = 30f;
    public float brakeTorque = 3500f;
    public Vector3 centerOfMassOffset = new Vector3(0, -0.5f, 0);

    private Rigidbody rb;
    private float vInput;
    private float hInput;
    private bool isBraking;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Lowers the center of gravity so the car doesn't flip!
        rb.centerOfMass += centerOfMassOffset;
    }

    void FixedUpdate()
    {
        ReadInput();

        // 2. Drive the Rear Wheels
        rearLeft.motorTorque = vInput * motorTorque;
        rearRight.motorTorque = vInput * motorTorque;

        // 3. Steer the Front Wheels
        frontLeft.steerAngle = hInput * steeringAngle;
        frontRight.steerAngle = hInput * steeringAngle;

        // 4. Handle Braking
        ApplyBrakes(isBraking ? brakeTorque : 0);

        // 5. Update the 3D wheels to match the physics
        UpdateWheelPos(frontLeft, meshFL);
        UpdateWheelPos(frontRight, meshFR);
        UpdateWheelPos(rearLeft, meshRL);
        UpdateWheelPos(rearRight, meshRR);
    }

    void ReadInput()
    {
#if ENABLE_INPUT_SYSTEM
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            vInput = (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed ? 1f : 0f) +
                     (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed ? -1f : 0f);
            hInput = (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed ? 1f : 0f) +
                     (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed ? -1f : 0f);
            isBraking = keyboard.spaceKey.isPressed;
        }
        else
        {
            vInput = 0f;
            hInput = 0f;
            isBraking = false;
        }
#else
        vInput = Input.GetAxis("Vertical");   // W/S or Up/Down
        hInput = Input.GetAxis("Horizontal"); // A/D or Left/Right
        isBraking = Input.GetKey(KeyCode.Space);
#endif
    }

    void ApplyBrakes(float amount)
    {
        frontLeft.brakeTorque = amount;
        frontRight.brakeTorque = amount;
        rearLeft.brakeTorque = amount;
        rearRight.brakeTorque = amount;
    }

    void UpdateWheelPos(WheelCollider col, Transform mesh)
    {
        if (mesh == null) return;

        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);
        mesh.position = pos;
        mesh.rotation = rot;
    }
}