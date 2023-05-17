using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockHorizon : MonoBehaviour
{
    private Transform planeTransform;

    // Start is called before the first frame update
    void Start()
    {
        planeTransform = GameObject.Find("Aircraft").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //Quaternion rot = new Quaternion(0f, 0f, planeTransform.localRotation.z, 1);
        //GetComponent<RectTransform>().localRotation = rot;

        Vector3 targetRotation = transform.rotation.eulerAngles;
        targetRotation.z = planeTransform.rotation.eulerAngles.z;
        transform.rotation = Quaternion.Euler(targetRotation);
    }
}
