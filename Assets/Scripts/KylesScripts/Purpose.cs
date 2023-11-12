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

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Ground"))
            {
                if (clickCounter == 0)
                {
                    Vector3 newPointA = hit.point;
                    newPointA.y = 0;
                    putToWorkScript.pointA.position = newPointA;
                    clickCounter++;
                }
                else if (clickCounter == 1)
                {
                    Vector3 newPointB = hit.point;
                    newPointB.y = 0;
                    putToWorkScript.pointB.position = newPointB;
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
