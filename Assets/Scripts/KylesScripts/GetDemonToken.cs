using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetDemonToken : MonoBehaviour
{
    public RectTransform scaleRectTransform;
    public float stationaryIncreaseRate = 0.1f; // Adjust this rate as needed
    public float scaleMaxValue = 1.0f; // Adjust the maximum scale value
    public float scaleFillSpeed = 1.0f; // Adjust the speed of the scale fill
    public float distanceInFront = 2.0f; // Adjust the distance for object instantiation
    public GameObject yourObjectPrefab; // Assign your object prefab in the Inspector

    private Vector3 previousPlayerPosition;

    void Start()
    {
        previousPlayerPosition = transform.position;
    }

    void Update()
    {
        // Calculate player movement
        float playerMovement = (transform.position - previousPlayerPosition).magnitude;

        // Increase the scale when the player is not moving
        if (playerMovement <= 0.001f) // You can adjust this threshold as needed
        {
            float newScale = scaleRectTransform.localScale.x + stationaryIncreaseRate * scaleFillSpeed * Time.deltaTime;

            // Clamp the scale to the defined maximum value
            newScale = Mathf.Clamp(newScale, 0f, scaleMaxValue);

            // Apply the new scale to the RectTransform
            scaleRectTransform.localScale = new Vector3(newScale, newScale, 1f);
        }

        // Check if the scale has reached its maximum value
        if (scaleRectTransform.localScale.x >= scaleMaxValue)
        {
            // Instantiate an object in front of the player
            Instantiate(yourObjectPrefab, transform.position + transform.forward * distanceInFront, Quaternion.identity);

            // Reset the scale to zero
            scaleRectTransform.localScale = Vector3.zero;
        }

        // Store the current player position for the next frame
        previousPlayerPosition = transform.position;
    }
}