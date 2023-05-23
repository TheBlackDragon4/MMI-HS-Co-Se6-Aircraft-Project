using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public float rotationSpeed = 10f;

    private GameObject landingstrip;

    [SerializeField]
    private Material seeTrough;

    void Start()
    {
        landingstrip = GameObject.FindGameObjectWithTag("Landing");
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Obstacle hit!");
//        landingstrip.GetComponent<EvaluateFlight>().obstacleWasHit();
        this.GetComponent<MeshRenderer>().material= seeTrough;
        Destroy(this.GetComponent<MeshCollider>());
    }
}
