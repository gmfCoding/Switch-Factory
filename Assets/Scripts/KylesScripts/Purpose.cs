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
        if (other.CompareTag("Switch"))
        {
            Destroy(other.gameObject);
            switchCount++;
            Debug.Log("ARE WE HERERERE");
            if (switchCount >= switchesNeededToFollow)
            {
                patrolScript.enabled = false;
                followPlayerScript.enabled = true;
                takeCommandFromMaster = true;
            }
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Ground"))
            {
                if (clickCounter == 0)
                {
                    putToWorkScript.pointA.position = hit.point;
                    clickCounter++;
                }
                else if (clickCounter == 1)
                {
                    putToWorkScript.pointB.position = hit.point;
                    clickCounter = 0;
                    followPlayerScript.enabled = false;
                    putToWorkScript.enabled = true;
                    takeCommandFromMaster = false;
                }
            }
        }
    }
}
