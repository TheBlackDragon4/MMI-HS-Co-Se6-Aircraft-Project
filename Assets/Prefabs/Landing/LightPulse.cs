using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LightPulse : MonoBehaviour
{
    private MeshRenderer mR;
    public Material glow;
    public Material turnedOff;

    public bool dayLightMode;

    private bool isOn = true;
    private float switchOffTime = 3f;
    private float switchOnTime = 1f;
    private float timeHolder;


    // Start is called before the first frame update
    void Start()
    {
        mR = this.GetComponent<MeshRenderer>();
        timeHolder = switchOffTime;
        if(dayLightMode)
            mR.material = turnedOff;
    }

    // Update is called once per frame
    void Update()
    {
        if(!dayLightMode)
            lightSwitch();
    }

    private void lightSwitch()
    {
        timeHolder -= Time.deltaTime;

        if(timeHolder< 0 )
        {
            if(isOn)
            {
                mR.material = turnedOff;
                timeHolder = switchOnTime;
            }
            else
            {
                mR.material = glow;
                timeHolder= switchOffTime;
            }

            isOn = !isOn;
        }
    }
}
