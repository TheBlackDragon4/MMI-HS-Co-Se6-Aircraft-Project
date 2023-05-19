using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaluateFlight : MonoBehaviour
{
    private GameObject airplane;

    public GameObject endScreen;
    public Text endText;

    private int hitObstacles;
    private int numObstacles;
 //   private int numBumps;

    private bool landingAllowed;

    // Start is called before the first frame update
    void Start()
    {
        hitObstacles= 0;
        numObstacles = GameObject.FindGameObjectsWithTag("Obstacle").Length;
 //       numBumps = 0;
        endScreen.SetActive(false);
        landingAllowed = false;
        airplane = GameObject.Find("Aircraft");
    }

    // Update is called once per frame
    void Update()
    {
        if(!landingAllowed)
        {
            if (airplane.GetComponent<AirplaneController>().getAltitude() > 10f)
                enableLanding();
        }
        else
        {
            if(airplane.GetComponent<AirplaneController>().getVelocity() <= 0.05f)
            {
                showEndscreen();
            }
        }
    }

    public void obstacleWasHit()
    {
        hitObstacles++;
    }

    public void showEndscreen()
    {
        endScreen.SetActive(true);
        Time.timeScale = 0;
        endText.text = "Results:\n\n";
        endText.text += "You've hit " + hitObstacles + "/" + numObstacles + " Obstacles\n";
        endText.text += "Your landing was " + landingQuality() + "\n\n";
        endText.text += "Your final rank: " + calculateRank();
    }

    public string landingQuality()
    {
        return "[landingQuality]";
    }

    public string calculateRank()
    {
        return "[calculateRank]";
    }

    public void enableLanding()
    {
        landingAllowed= true;
    }
}
