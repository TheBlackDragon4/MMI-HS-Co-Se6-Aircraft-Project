using System;
using System.Collections.Generic;
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

    //Addes variables for changing mode and add the time
    private int  mode = 1;
    private float startTime = 0f;
    private bool isTriggered = false;
    private string timeText = "00:00"; // Standardwert, falls thrustPercent nicht 1 ist

    // Anzahl der durchfolgener Ringe
    private int ringCounter;

    private void Start()
    {
        aircraftPhysics = GetComponent<AircraftPhysics>();
        rb = GetComponent<Rigidbody>();
        controlsDisplay = GameObject.Find("Controls");
    }

    private void Update()
    {
        ////////////////// MOVEMENT //////////////////
        // Tastatussteuerung
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mode = 1;
        }
        // Controllersteuerung
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mode = 2;
        }
        // Maussteuerung
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            mode = 3;
        }        

        if (mode == 1)
        {
            //Debug.Log("Tastatursteuerung");
            Pitch = Input.GetAxis("Vertical"); // Vertical direction
            Roll = Input.GetAxis("Horizontal"); // Horizontal direction
            Yaw = Input.GetAxis("Yaw");            
        } 
        if (mode == 2)
        {
            //Debug.Log("Controllersteuerung");
            Pitch = Input.GetAxis("Vertical1"); // Vertical direction
            Roll = Input.GetAxis("Horizontal1"); // Horizontal direction
            Yaw = Input.GetAxis("Yaw1");
        }
        if (mode == 3)
        {
            //Debug.Log("Maussteuerung");
            Pitch = Input.GetAxis("Mouse Y"); // Vertical direction
            Roll = Input.GetAxis("Mouse X"); // Horizontal direction
            Yaw = Input.GetAxis("Yaw");
        }

        // joystick button 0 = XBox Controller Taste A
        // Input.GetMouseButtonDown(0) = Linke Maustaste
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("joystick 1 button 9") || Input.GetButtonDown("joystick button 0") || Input.GetMouseButtonDown(0))
        {
            // Schubkraft
            thrustPercent = thrustPercent > 0 ? 0 : 1f;

            if (!isTriggered)
            {
                isTriggered = true;
                // Spielzeitberechnung
                startTime = Time.time;                
            }            
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

        ////////////////// Zeitberechnung für Ausgabe //////////////////
        float elapsedTime = Time.time - startTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timeText = string.Format("{0:0}:{1:00}", minutes, seconds);


        if(transform.position.y < 0)
        {

        }

        ////////////////// Displayausgabe //////////////////
        if (visibleControls)
        {
            if (!controlsDisplay.activeSelf)
            {
                controlsDisplay.SetActive(true);
            }
            displayText.text = "Zeit: " + timeText + " min\n";
            displayText.text += "V: " + ((int)rb.velocity.magnitude).ToString("D3") + " m/s\n";
            displayText.text += "A: " + ((int)transform.position.y).ToString("D4") + " m\n";
            displayText.text += "T: " + (int)(thrustPercent * 100) + "%\n";
            displayText.text += brakesTorque > 0 ? "B: ON \n" : "B: OFF \n";
            displayText.text += Flap > 0 ? "F: ON\n" : "F: OFF \n";
            displayText.text += ringCounter == 0 ? "Ringe: 0 von 9 " : "Ringe: " + ringCounter + " von 9";
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

    public float GetBrakesTorque()
    {
        return brakesTorque;
    }

    public float GetThrustPercent()
    {
        return thrustPercent;
    }

    ////////////////// Funktion herauszufinden ob Ring berührt oder nicht //////////////////
    // Increment Ring Counter    
    public void SetRingCounter(int counter)
    {
        ringCounter = counter;
    }
}
