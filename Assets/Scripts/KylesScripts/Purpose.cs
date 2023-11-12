using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purpose : MonoBehaviour
{
    public Patrol patrolScript;
    public PutToWork putToWorkScript;
    public FollowPlayer followPlayerScript;

    public int switchCount = 0;
    public int switchesNeededToFollow  = 1;
    private bool takeCommandFromMaster = false;
    private int clickCounter = 0;

    private void Update()
    {
        if (takeCommandFromMaster == true)
            HandleMouseInput();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Token"))
        {
            Destroy(other.gameObject);
            switchCount++;
            if (switchCount >= switchesNeededToFollow)
            {
                patrolScript.enabled = false;
                putToWorkScript.enabled = false;
                followPlayerScript.enabled = true;
                takeCommandFromMaster = true;
            }
        }
        if (other.CompareTag("Grabable"))
        {


        }
    }

    private static Plane xzPlane = new Plane(Vector3.up, Vector3.zero);

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (xzPlane.Raycast(ray, out float enter))
            {
                if (clickCounter == 0)
                {
                    putToWorkScript.pointA.position = ray.GetPoint(enter);
                    clickCounter++;
                }
                else if (clickCounter == 1)
                {
                    putToWorkScript.pointB.position = ray.GetPoint(enter);
                    clickCounter = 0;
                    patrolScript.enabled = false;
                    followPlayerScript.enabled = false;
                    putToWorkScript.enabled = true;
                    takeCommandFromMaster = false;
                }
            }
        }
    }
}
