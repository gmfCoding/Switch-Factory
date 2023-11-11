using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed = 1f;
    public float rotationSpeed = 720f;
    private float waitTime;
    public float startWaitTime = 5f;
    public float distanceToDestination = 0.2f;

    public Transform moveSpot;
    public float minX = -9f;
    public float maxX = 9f;
    public float minZ = -9f;
    public float maxZ = 9f;

    private void Start()
    {
        waitTime = startWaitTime;
        moveSpot.position = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));//having the y at 0 insures the move spot doent underground
        //moveSpot.position = new Vector3(Random.Range(minX, maxX), this.transform.position.y, Random.Range(minZ, maxZ));

    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, moveSpot.position) < distanceToDestination)
        {
            if (waitTime <= 0)
            {
                //moveSpot.position = new Vector3(Random.Range(minX, maxX), this.transform.position.y, Random.Range(minZ, maxZ));
                moveSpot.position = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));
                waitTime = startWaitTime;
            }
            else
                waitTime -= Time.deltaTime;
        }
        Vector3 directionToDestination = moveSpot.position - transform.position;
        directionToDestination.y = 0f;
        directionToDestination.Normalize();
        Quaternion toRotation = Quaternion.LookRotation(directionToDestination, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }
}
