using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateControl : MonoBehaviour
{
    public Transform targetTransform;
    public Animator animator;
    public Patrol patrolScript;
    public PutToWork putToWorkScript;
    public FollowPlayer followPlayerScript;
    private Vector3 previousPosition;
    public float movementState = 0.1f;
    private BoxCollider boxCollider;
    private bool brawlTriggered = false;
    public int timeUntilNextFight = 50;

    public bool brawlAllowed = true;

    void Start()
    {
        previousPosition = targetTransform.position;
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        Vector3 movement = targetTransform.position - previousPosition;
        if (movement.magnitude > movementState)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
        previousPosition = transform.position;
    }
    void OnTriggerEnter(Collider other)
    {
        if (brawlAllowed && brawlTriggered == false)
        {
            if (other.CompareTag("Demon"))
            {
                boxCollider.enabled = false;
                brawlTriggered = true;
                patrolScript.enabled = false;
                putToWorkScript.enabled = false;
                followPlayerScript.enabled = false;

                if (this.transform.position.x > other.transform.position.x)
                {
                    animator.SetTrigger("BrawlWin");
                }
                else
                {
                    animator.SetTrigger("BrawlLose");
                }
                transform.LookAt(other.transform);
                other.transform.LookAt(transform);
                StartCoroutine(EnableBrawl());
            }
        }
}
    public void EnableScripts()
    {
        patrolScript.enabled = true;
        animator.SetBool("IsMoving", true);
        StartCoroutine(EnableBrawl());
    }
    IEnumerator EnableBrawl()
    {
        yield return new WaitForSeconds(timeUntilNextFight);
        boxCollider.enabled = true;
        brawlTriggered = false;
    }
}