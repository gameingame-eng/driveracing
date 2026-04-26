using UnityEngine;
using TMPro;

public class Speedometer : MonoBehaviour
{
    [Header("References")]
    public Rigidbody targetVehicle; 
    public TextMeshProUGUI speedText; 

    [Header("Settings")]
    // Changed 3.6f (KM/H) to 2.237f (MPH)
    public float multiplier = 2.237f; 
    public string unitLabel = " MPH";

    void Update()
    {
        if (targetVehicle != null && speedText != null)
        {
            // Calculate speed
            float speed = targetVehicle.linearVelocity.magnitude * multiplier;
            
            // Update display
            speedText.text = Mathf.Round(speed).ToString() + unitLabel;
        }
    }
}