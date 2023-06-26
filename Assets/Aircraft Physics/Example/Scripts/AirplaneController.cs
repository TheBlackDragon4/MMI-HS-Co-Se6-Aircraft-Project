using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

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
    [SerializeField]
    private Button[] touchButtons = null;

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
    bool yawingLeft;
    bool yawingRight;

    //Anzahl der durchflogenen Ringe
    private int ringezaehler;

    AircraftPhysics aircraftPhysics;
    Rigidbody rb;

    //Added variables
    public bool visibleControls = true;
    GameObject controlsDisplay;

    //trackpad varriables
    private float swipeHorizontal;
    private float swipeVertical;

    //Addes variables for changing mode and add the time
    private int mode = 0;
    private float startTime = 0f;
    private bool isTriggered = false;
    private int minutes = 0;
    private int seconds = 0;
    private float elapsedTime = 0f;

    private void Start()
    {
        aircraftPhysics = GetComponent<AircraftPhysics>();
        rb = GetComponent<Rigidbody>();
        controlsDisplay = GameObject.Find("Controls");

        // Find the buttons with the specified tag
        touchButtons = GameObject.FindGameObjectsWithTag("TouchButton")
            .Select(go => go.GetComponent<Button>())
            .ToArray();
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mode = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mode = 1;
        } 
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            mode = 2;
        }        

        if (mode == 0)
        {
            SetTouchButtonsState(false);
            Debug.Log("Tastatursteuerung");
            Pitch = Input.GetAxis("Vertical");
            //Debug.Log(Pitch);
            Roll = Input.GetAxis("Horizontal");
            Yaw = Input.GetAxis("Yaw");            
        } 
        if (mode == 1)
        {
            SetTouchButtonsState(false);
            Debug.Log("Controllersteuerung");
            Pitch = Input.GetAxis("Vertical1");
            //Debug.Log(Pitch);
            Roll = Input.GetAxis("Horizontal1");
            //Debug.Log(Roll);
            Yaw = Input.GetAxis("Yaw1");
        }
        if (mode == 2)
        {
            SetTouchButtonsState(true);
            Debug.Log("Touchscreensteuerung");
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    float touchDeltaX = touch.deltaPosition.x;
                    float touchDeltaY = touch.deltaPosition.y;
                    float normalizedDeltaX = Mathf.Clamp(touchDeltaX * 50 / (Screen.width * 0.5f), -1f, 1f);
                    float normalizedDeltaY = Mathf.Clamp(touchDeltaY *50 / (Screen.height * 0.5f), -1f, 1f);


                    swipeHorizontal = Mathf.Clamp(normalizedDeltaX, -1f, 1f);
                    swipeVertical = Mathf.Clamp(normalizedDeltaY, -1f, 1f);
                }
            }

            // Yaw resetting to 0 logic (middle)
            if(yawingLeft)
            {
                Yaw = 1f;
            } else if(yawingRight)
            {
                Yaw = -1f;
            } else
            {
                Yaw = 0;
            }


            Pitch = swipeVertical; // Vertical direction
            // Debug.Log("Y Pitch: " + Pitch);
            Roll = swipeHorizontal; // Horizontal direction
            // Debug.Log("X Roll: " + Roll);
            Debug.Log("X Roll: " + Roll + " || Y Pitch: " + Pitch + " || Z Yaw: " + Yaw);
        }

        // joystick button 0 = XBox Controller Taste A
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("joystick 1 button 9") || Input.GetButtonDown("joystick button 0"))
        {
            thrustPercent = thrustPercent > 0 ? 0 : 1f;

            if (!isTriggered)
            {
                isTriggered = true;
                // Spielzeitberechnung
                startTime = Time.time;
            }
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

        // Zeitausgabe anpassen
        elapsedTime = Time.time - startTime;
        minutes = Mathf.FloorToInt(elapsedTime / 60);
        seconds = Mathf.FloorToInt(elapsedTime % 60);

        if (visibleControls)
        {
            if (!controlsDisplay.activeSelf)
            {
                controlsDisplay.SetActive(true);
            }
            displayText.text = "V: " + ((int)rb.velocity.magnitude).ToString("D3") + " m/s\n";
            displayText.text += "A: " + ((int)transform.position.y).ToString("D4") + " m\n";
            displayText.text += "T: " + (int)(thrustPercent * 100) + "%\n";
            displayText.text += "Zeit: " + (int)minutes + "min " + (int)seconds + "sec\n";
            displayText.text += brakesTorque > 0 ? "B: ON \n" : "B: OFF \n";
            displayText.text += Flap > 0 ? "F: ON \n" : "F: OFF \n";
            displayText.text += ringezaehler < 0 ? "Ringe: 0" : "Ringe: " + ringezaehler;
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

    public void SetRingeZeahler(int zaehler1)
    {
        ringezaehler = zaehler1;
    }


    // TOUCH STEUERUNG
    public void toggleThrust()
    {
        thrustPercent = thrustPercent > 0 ? 0 : 1f;
    }

    public void toggleBrakes()
    {
        brakesTorque = brakesTorque > 0 ? 0 : 100f;
    }

    public void toggleFlaps()
    {
        Flap = Flap > 0 ? 0 : 0.3f;
    }

    public void yawLeftDOWN()
    {
        yawingLeft = true;
    }

    public void yawLeftUP()
    {
        yawingLeft = false;
    }

    public void yawRightDOWN()
    {
        yawingRight = true;
    }

    public void yawRightUP()
    {
        yawingRight = false;
    }

    private void SetTouchButtonsState(bool state)
    {
        foreach (Button button in touchButtons)
        {
            button.interactable = state;
            button.gameObject.SetActive(state);
        }
    }
}
