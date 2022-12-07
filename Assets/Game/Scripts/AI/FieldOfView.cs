using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0f, 360f)]
    public float angle;

    public Transform targetRef;
    public Transform borderRef;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public LayerMask borderMask;

    public bool canSeeTarget;
    public bool canSeeBorder;

    // Start is called before the first frame update
    void Start()
    {
        //targetRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        targetRef = RangeCheck(targetMask, radius);
        borderRef = RangeCheck(borderMask, radius * 0.6f);
        canSeeTarget = targetRef != null;
        canSeeBorder = borderRef != null;
    }

    private Transform RangeCheck(LayerMask mask, float rad)
    {
        //Vector3 center = new Vector3(transform., 0, rad);
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, rad, mask);
        Transform minTarget = null;
        float minDistance = 10000;

        if (rangeChecks.Length > 0)
        {
            for(int i = 0; i < rangeChecks.Length; i++)
            {
                Transform target = rangeChecks[i].transform;
                if (target != transform)
                {
                    Vector3 directionToTarget = (target.position - transform.position).normalized;

                    if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                    {
                        float distanceToTarget = Vector3.Distance(transform.position, target.position);

                        if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                            target = null;

                        if (target != null && distanceToTarget < minDistance)
                        {
                            minDistance = distanceToTarget;
                            minTarget = target;
                        }
                    }
                }
                
            }
            
        }
        return minTarget;
    }
}
