using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goal : MonoBehaviour
{
    private AirplaneController airplaneController;
    private int zaehler1 ;
    // Start is called before the first frame update
    void Start()
    {
        zaehler1 = 0;
        airplaneController = GetComponent<AirplaneController>();
        airplaneController.SetRingeZeahler(zaehler1);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("test");
        if (other.CompareTag("Obstacle")) {
            Debug.Log("test2");
            //other.GetComponent<MeshCollider>().enabled = false;
            zaehler1++;
            airplaneController.SetRingeZeahler(zaehler1);
        }
        
        if ((other.CompareTag("Finish"))
            && (zaehler1 == 9)
            && (0 == airplaneController.GetBrakesTorque())
            && (0 == airplaneController.GetThrustPercent())) 
         {
                Debug.Log("Aircraft reached the finish line!");
                
         }
    } 
}
