using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpControl : MonoBehaviour
{
    private Transform pickUpPoint;
    private Transform player;

    public float pickUpDistance;
    public float forceMulti;

    public bool readyToThrow;
    public bool itemIsPicked;

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").transform;
        pickUpPoint = GameObject.Find("PickUpPoint").transform;
    }

    void Update()
    {
        if (Input.GetButton("Jump") && itemIsPicked == true && readyToThrow)
        {
            forceMulti += 300 * Time.deltaTime;
        }
        //if (Input.GetKey(KeyCode.Space) && itemIsPicked == true && readyToThrow)
        //{
        //    forceMulti += 300 * Time.deltaTime;
        //}
        pickUpDistance = Vector3.Distance(player.position, transform.position);

        if(pickUpDistance <= 2)
        {
            if (Input.GetButtonDown("Jump") && itemIsPicked == false && pickUpPoint.childCount < 1)
            {
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<BoxCollider>().enabled = false;
                this.transform.position = pickUpPoint.position;
                this.transform.parent = GameObject.Find("PickUpPoint").transform;

                itemIsPicked = true;
                forceMulti = 0;
            }
        }

        if (Input.GetButtonUp("Jump") && itemIsPicked == true)
        {
            readyToThrow = true;

            if (forceMulti > 10)
            {
                rb.AddForce(player.transform.forward * forceMulti);
                this.transform.parent = null;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<BoxCollider>().enabled = true;
                itemIsPicked = false;
                forceMulti = 0;
                readyToThrow = false;
            }
            forceMulti = 0;
        }
    }
}
