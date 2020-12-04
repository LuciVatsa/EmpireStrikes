using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovement : MonoBehaviour
{
    public Transform StartPos;
    public Transform EndPos;

    // Movement speed in units per second.
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    void Start()
    {
        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(StartPos.position, EndPos.position);
    }

    // Move to the target end position.
    void Update()
    {
        if (gameObject.activeSelf) 
        {
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - startTime) * speed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(StartPos.position, EndPos.position, fractionOfJourney);
        }

        if(gameObject.transform.position == EndPos.position)
        {
            gameObject.SetActive(false);
        }
    }
}
