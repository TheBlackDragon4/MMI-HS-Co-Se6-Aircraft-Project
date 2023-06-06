using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingCounter : MonoBehaviour
{
    // Für Zugriff auf auf Ringcounter
    private AirplaneController airplaneController;
    private int ringCounter;

    // Start is called before the first frame update
    void Start()
    {
        // Für das Aufsummieren des Ring Counters
        ringCounter = 0;
        airplaneController = GetComponent<AirplaneController>();
        airplaneController.SetRingCounter(ringCounter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            ringCounter++;
            airplaneController.SetRingCounter(ringCounter);
            Debug.Log("Ringcounter Anzahl: " + ringCounter);
        }
    }
}
