using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirplaneController : MonoBehaviour
{
    [SerializeField]
    List<AeroSurface> controlSurfaces = null;
    [SerializeField]
    List<WheelCollider> wheels = null;
    [SerializeField]
    float rollControlSensitivity = 0.2f;
    [SerializeField]
    float pitchControlSensitivity = 0.2f;
    [SerializeField]
    float yawControlSensitivity = 0.2f;

    [Range(-1, 1)]
    public float Pitch;
    [Range(-1, 1)]
    public float Yaw;
    [Range(-1, 1)]
    public float Roll;
    [Range(0, 1)]
    public float Flap;
    [SerializeField]
    Text displayText = null;

    float thrustPercent;
    float brakesTorque;

    AircraftPhysics aircraftPhysics;
    Rigidbody rb;

    //Added variables
    public bool visibleControls = true;
    GameObject controlsDisplay;

    private bool mode = true;

    private void Start()
    {
        aircraftPhysics = GetComponent<AircraftPhysics>();
        rb = GetComponent<Rigidbody>();
        controlsDisplay = GameObject.Find("Controls");
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            mode = !mode;
        }

        if (mode == true)
        {
            Debug.Log("Tastatursteuerung");
            Pitch = Input.GetAxis("Vertical");
            Debug.Log(Pitch);
            Roll = Input.GetAxis("Horizontal");
            Yaw = Input.GetAxis("Yaw");
        } 
        else
        {
            Debug.Log("Controllersteuerung");
            Pitch = Input.GetAxis("Vertical1");
            Debug.Log(Pitch);
            Roll = Input.GetAxis("Horizontal1");
            Debug.Log(Roll);
            Yaw = Input.GetAxis("Yaw1");
        }

        // joystick button 0 = XBox Controller Taste A
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("joystick 1 button 9") || Input.GetButtonDown("joystick button 0"))
        {
            thrustPercent = thrustPercent > 0 ? 0 : 1f;
        }
        // joystick button 2 = XBox Controller Taste X
        if (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("joystick 1 button 10") || Input.GetButtonDown("joystick button 2"))
        {
            Flap = Flap > 0 ? 0 : 0.3f;
        }
        // joystick button 1 = XBox Controller Taste B
        if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("joystick 1 button 11") || Input.GetButtonDown("joystick button 1"))
        {
            brakesTorque = brakesTorque > 0 ? 0 : 100f;
        }

        if (visibleControls)
        {
            if (!controlsDisplay.activeSelf)
            {
                controlsDisplay.SetActive(true);
            }
            displayText.text = "V: " + ((int)rb.velocity.magnitude).ToString("D3") + " m/s\n";
            displayText.text += "A: " + ((int)transform.position.y).ToString("D4") + " m\n";
            displayText.text += "T: " + (int)(thrustPercent * 100) + "%\n";
            displayText.text += brakesTorque > 0 ? "B: ON \n" : "B: OFF \n";
            displayText.text += Flap > 0 ? "F: ON" : "F: OFF";
        }
        else
        {
            controlsDisplay.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        SetControlSurfecesAngles(Pitch, Roll, Yaw, Flap);
        aircraftPhysics.SetThrustPercent(thrustPercent);
        foreach (var wheel in wheels)
        {
            wheel.brakeTorque = brakesTorque;
            // small torque to wake up wheel collider
            wheel.motorTorque = 0.01f;
        }
    }

    public void SetControlSurfecesAngles(float pitch, float roll, float yaw, float flap)
    {
        foreach (var surface in controlSurfaces)
        {
            if (surface == null || !surface.IsControlSurface) continue;
            switch (surface.InputType)
            {
                case ControlInputType.Pitch:
                    surface.SetFlapAngle(pitch * pitchControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Roll:
                    surface.SetFlapAngle(roll * rollControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Yaw:
                    surface.SetFlapAngle(yaw * yawControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Flap:
                    surface.SetFlapAngle(Flap * surface.InputMultiplyer);
                    break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            SetControlSurfecesAngles(Pitch, Roll, Yaw, Flap);
    }

    public float getVelocity()
    {
        return rb.velocity.magnitude;
    }

    public float getAltitude()
    {
        return transform.position.y;
    }
}
