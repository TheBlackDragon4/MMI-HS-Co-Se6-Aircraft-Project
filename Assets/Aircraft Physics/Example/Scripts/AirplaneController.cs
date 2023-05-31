<<<<<<< Updated upstream
﻿using System.Collections.Generic;
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

    private void Start()
    {
        aircraftPhysics = GetComponent<AircraftPhysics>();
        rb = GetComponent<Rigidbody>();
        controlsDisplay = GameObject.Find("Controls");
    }

    private void Update()
    {
        Pitch = Input.GetAxis("Vertical");
        Roll = Input.GetAxis("Horizontal");
        Yaw = Input.GetAxis("Yaw");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            thrustPercent = thrustPercent > 0 ? 0 : 1f;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Flap = Flap > 0 ? 0 : 0.3f;
        }

        if (Input.GetKeyDown(KeyCode.B))
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
            displayText.text += brakesTorque > 0 ? "B: ON" : "B: OFF";
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
=======
﻿using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    private int  mode = 0;

    private void Start()
    {
        aircraftPhysics = GetComponent<AircraftPhysics>();
        rb = GetComponent<Rigidbody>();
        controlsDisplay = GameObject.Find("Controls");
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            mode = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mode = 1;
        } 
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mode = 2;
        }        

        if (mode == 0)
        {
            Debug.Log("Tastatursteuerung");
            Pitch = Input.GetAxis("Vertical");
            Debug.Log(Pitch);
            Roll = Input.GetAxis("Horizontal");
            Yaw = Input.GetAxis("Yaw");            
        } 
        if (mode == 1)
        {
            Debug.Log("Controllersteuerung");
            Pitch = Input.GetAxis("Vertical1");
            Debug.Log(Pitch);
            Roll = Input.GetAxis("Horizontal1");
            Debug.Log(Roll);
            Yaw = Input.GetAxis("Yaw1");
        }
        if (mode == 2)
        {
            Debug.Log("Maussteuerung");
            Pitch = Input.GetAxis("Mouse Y"); // Vertical direction
            Debug.Log("Mous Y Pitch: " + Pitch);
            Roll = Input.GetAxis("Mouse X"); // Horizontal direction
            Debug.Log("Mous X Roll: " + Roll);
            Yaw = Input.GetAxis("Yaw");
        }


        // joystick button 0 = XBox Controller Taste A
        // Input.GetMouseButtonDown(0) = Linke Maustaste
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("joystick 1 button 9") || Input.GetButtonDown("joystick button 0") || Input.GetMouseButtonDown(0))
        {
            // Schubkraft
            thrustPercent = thrustPercent > 0 ? 0 : 1f;
        }
        // joystick button 2 = XBox Controller Taste X
        // Input.GetMouseButtonDown(1) = Rechte Maustaste
        if (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("joystick 1 button 10") || Input.GetButtonDown("joystick button 2") || Input.GetMouseButtonDown(2))
        {
            // Klappen
            Flap = Flap > 0 ? 0 : 0.3f;
        }
        // joystick button 1 = XBox Controller Taste B
        if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("joystick 1 button 11") || Input.GetButtonDown("joystick button 1") || Input.GetMouseButtonDown(1))
        {
            // Bremsen
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
>>>>>>> Stashed changes
