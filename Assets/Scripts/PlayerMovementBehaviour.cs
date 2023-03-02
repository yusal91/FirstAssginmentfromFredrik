using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementBehaviour : MonoBehaviour
{
    [Header("Component References")]
    public Rigidbody rb;    

    [Header("Movement Settings")]
    public float movementSpeed = 3f;
    public float turnSpeed = 0.1f;

    [Header("Jump Settings")]
    public float jumpHeight = 5f;
    public float dashForce;
    public float dashDuration;
    public float dashSpeed = 10;

    public float buttonTime = 0.5f;    
    public float cancelRate = 100;
    float jumpTime;
    bool jumping;
    bool jumpCancelled;


    [Header("Ground Settings")]
    public Transform groundCheck;
    public LayerMask groundLayer;   

    [Header("Camera Settings")]   //Stored Values 
    public Camera mainCamera;
    private Vector3 movementDirection;
    

    public void SetupBehaviour()
    {
        SetGameplayCamera();
    }

    void SetGameplayCamera()
    {
        //mainCamera = CameraManager.Instance.GetGameplayCamera();    //need to create the camera script
    }

    public void UpdateMovementData(Vector3 newMovementDirection)
    {
        movementDirection = newMovementDirection;
    }

    void FixedUpdate()
    {
        MoveThePlayer();
        TurnThePlayer();
    }

    void MoveThePlayer()
    {
        Vector3 movement = CameraDirection(movementDirection) * movementSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
    }

    void TurnThePlayer()
    {
        if (movementDirection.sqrMagnitude > 0.01f)
        {
            // could change to lerp
            Quaternion rotation = Quaternion.Slerp(rb.rotation,                                              
                                                 Quaternion.LookRotation(CameraDirection(movementDirection)),
                                                 turnSpeed);

            rb.MoveRotation(rotation);
        }
    }

    Vector3 CameraDirection(Vector3 movementDirection)
    {
        var cameraForward = mainCamera.transform.forward;
        var cameraRight = mainCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        return cameraForward * movementDirection.z + cameraRight * movementDirection.x;
    }
    

    public void jump()
    {
        if (movementDirection.y <= 0 && onGround())
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }

    public IEnumerator Dash()
    {
        rb.AddForce(transform.forward * dashForce, ForceMode.VelocityChange);
        yield return new WaitForSeconds(dashDuration);
        rb.velocity = Vector3.zero;
    }   

    public bool onGround()
    {      
        return Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);        
    }     
}
