using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float speed = 1f;
    public float rotationSpeed = 720f;
    public float distanceToPlayer = 3f;

    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > distanceToPlayer)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0f; // Set Y component to 0 to prevent changes in height
            direction.Normalize();
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);

            float targetY = transform.position.y; // Store the current Y position
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, targetY, transform.position.z); // Keep the same Y position
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
