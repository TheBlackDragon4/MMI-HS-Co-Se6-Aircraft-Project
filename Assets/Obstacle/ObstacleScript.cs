using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public float rotationSpeed = 10f;

    [SerializeField]
    private Material seeTrough;

    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Airplane.");
        this.GetComponent<MeshRenderer>().material= seeTrough;
        Destroy(this.GetComponent<MeshCollider>());
    }
}
