using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowNextRing : MonoBehaviour
{   
    public string ringTag = "Obstacle"; // Tag des Rings, dem das Rechteck folgen soll

    private GameObject targetRing; // Der nächste Ring, dem das Rechteck folgt

    void Start()
    {
        // Finde den nächsten Ring zu Beginn
        FindNextRing();
    }

    void Update()
    {
        // Überprüfe, ob der nächste Ring noch existiert
        if (targetRing != null)
        {
            // Richte das Rechteck in Richtung des Rings aus
            transform.LookAt(targetRing.transform);
        }
        else
        {
            // Wenn kein Ring vorhanden ist, finde den nächsten Ring
            FindNextRing();
        }
    }

    void FindNextRing()
    {
        // Finde alle Objekte mit dem angegebenen Tag
        GameObject[] rings = GameObject.FindGameObjectsWithTag(ringTag);

        if (rings.Length > 0)
        {
            // Bestimme den nächsten Ring basierend auf der Entfernung
            float closestDistance = Mathf.Infinity;
            GameObject closestRing = null;

            foreach (GameObject ring in rings)
            {
                float distance = Vector3.Distance(transform.position, ring.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestRing = ring;
                }
            }

            // Setze den nächsten Ring als das Ziel des Rechtecks
            targetRing = closestRing;
        }
    }  
}

