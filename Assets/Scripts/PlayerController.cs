using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{

    private Rigidbody playerRb;
    [SerializeField] private GameObject centreOfMass;

    [SerializeField] private TextMeshProUGUI speedometerText;
    [SerializeField] private string speedUnit = "kph";
    private float currentSpeed;
    private float maxKph = 295.0f;
    private float kphConv = 3.6f;
    private float mphConv = 2.237f;

    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;

    [System.Serializable] 
    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor;
        public bool steering;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
        playerRb.centerOfMass = centreOfMass.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeed();
    }

    void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
        }


        MaxSpeed(speedUnit);
        
    }

    private void MaxSpeed(string units = "kph")
    {
        float maxMph = maxKph / 1.609f;

        if (units == "kph")
        {
            if (playerRb.velocity.magnitude * kphConv > maxKph)
            {
                playerRb.velocity = playerRb.velocity.normalized * (maxKph / kphConv);
            }
        } else if (units == "mph")
        {
            if (playerRb.velocity.magnitude * mphConv > maxMph)
            {
                playerRb.velocity = playerRb.velocity.normalized * (maxMph / mphConv);
            }

        }
    }

    private void UpdateSpeed()
    {
        SpeedConversion(speedUnit);
        speedometerText.text = "Speed: " + currentSpeed + speedUnit;
    }

    private void SpeedConversion(string units = "kph")
    {
        float conversion = units == "kph" ? kphConv : units == "mph" ? mphConv : 0.0f;
        currentSpeed = Mathf.Round(playerRb.velocity.magnitude * conversion);

    }
}
