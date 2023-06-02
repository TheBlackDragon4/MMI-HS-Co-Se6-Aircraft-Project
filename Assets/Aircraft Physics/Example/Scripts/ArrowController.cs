using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public Transform[] targets;
    private int currentTargetIndex = 0;

    void Update()
    {
        if (targets.Length > 0)
        {
            // Rotate the arrow to point towards the current target object
            transform.LookAt(targets[currentTargetIndex]);

            // Rotate the arrow on the X-axis by 180 degrees
            transform.Rotate(180f, 0f, 0f);

            // Check if the arrow has reached the current target
            if (Vector3.Distance(transform.position, targets[currentTargetIndex].position) < 15f)
            {
                // Move to the next target in the list
                currentTargetIndex = (currentTargetIndex + 1) % targets.Length;
            }
        }
    }
}