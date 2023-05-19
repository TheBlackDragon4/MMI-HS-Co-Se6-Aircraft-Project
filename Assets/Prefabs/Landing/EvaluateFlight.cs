using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaluateFlight : MonoBehaviour
{
    public GameObject endScreen;
    public Text endText;

    private int hitObstacles;
    private int numObstacles;
    private int numBumps;

    private bool collisionAllowed;

    // Start is called before the first frame update
    void Start()
    {
        hitObstacles= 0;
        numObstacles = GameObject.FindGameObjectsWithTag("Obstacle").Length;
        numBumps = 0;
        endScreen.SetActive(false);
        collisionAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void obstacleWasHit()
    {
        hitObstacles++;
    }

    public void showEndscreen()
    {
        endScreen.SetActive(true);
        Time.timeScale = 0;
        endText.text = "Results:\n";
        endText.text += "You've hit " + hitObstacles + "/" + numObstacles + " Obstacles\n";
        endText.text += "Your landing was " + landingQuality() + "\n\n";
        endText.text += "Your final rank:";
    }

    public string landingQuality()
    {
        return "";
    }

    //public void OnCollisionStay(Collision collision)
    //{
    //        if(GameObject.Find("Aircraft").GetComponent<AirplaneController>().getVelocity() <= 0.1)
    //            showEndscreen();
    //}

    //public void OnCollisionEnter(Collision collider)
    //{
    //    if (collider.gameObject.tag.Equals("Landing"))
    //    {
    //        if (collisionAllowed)
    //        {
    //            numBumps++;
    //            Debug.Log(numBumps);
    //            collisionAllowed = false;
    //        }
    //    }
    //}

    //public void OnCollisionExit(Collision collider)
    //{
    //    if (collider.gameObject.tag.Equals("Landing"))
    //    {
    //        if (!collisionAllowed)
    //            collisionAllowed = true;
    //    }
    //}
}
