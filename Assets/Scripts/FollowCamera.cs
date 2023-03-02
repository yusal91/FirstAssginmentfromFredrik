using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offSet;
   

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offSet;   // storing the target position + offset to local veriable
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);                                                       
        transform.position = smoothedPosition;                // follows the player

        transform.LookAt(target);                            // always looks at the target
    }
}
