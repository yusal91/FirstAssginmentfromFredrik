using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{

    [Header("Sub Behaviours")]
    public PlayerMovementBehaviour playerMovementBehaviour;    
    //public PlayerAnimationBehaviour playerAnimationBehaviour;   

    [Header("Input Movement Settings")]    
    public float movementSmoothingSpeed = 1f;
    public Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;
    [Header("Bullet")]
    public GameObject bullet;
    public Transform shootPotion;


    public void SetupPlayer()             //(int newPlayerID)
    {
        playerMovementBehaviour.SetupBehaviour();
    }

    //Update Loop - Used for calculating frame-based data
    void Update()
    {
        OnMovement(smoothInputMovement);
        CalculateMovementInputSmoothing();
        UpdatePlayerMovement();        
        onJump();
        OnDash();
        AttackKeyOnKeyboard();
    }  

    void UpdatePlayerMovement()
    {
        playerMovementBehaviour.UpdateMovementData(smoothInputMovement);
    }

    void CalculateMovementInputSmoothing()
    {
        smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
    }

    void OnMovement(Vector3 inputMovement)    
    {
        //inputMovement.x = Input.GetAxis("Horizontal");        
        //inputMovement.z = Input.GetAxis("Vertical");
        inputMovement.x = managerJoystick.instance.InputHorizontal();
        inputMovement.z = managerJoystick.instance.InputVertical();

        rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.z);
        rawInputMovement = PlayerSlop();      // testing if dash can be used only on ground       
    }

    private Vector3 PlayerSlop()
    {
        RaycastHit groundcheckHit = new RaycastHit();
        Vector3 calculatedPlayermovement = rawInputMovement;
        if(playerMovementBehaviour.onGround())
        {
            Vector3 localGroundeCheckHitNormal = playerMovementBehaviour.rb.transform.InverseTransformDirection
                                                  (groundcheckHit.normal);
            float groundSlopeAngel = Vector3.Angle(localGroundeCheckHitNormal, playerMovementBehaviour.rb.transform.up);
            if(!(groundSlopeAngel == 0f))
            {
                Quaternion slopeAngleRotation = Quaternion.FromToRotation(playerMovementBehaviour.rb.transform.up, 
                                                localGroundeCheckHitNormal);
                calculatedPlayermovement = slopeAngleRotation * calculatedPlayermovement;
            }
#if UNITY_EDITOR
            Debug.DrawRay(playerMovementBehaviour.rb.position, 
                          playerMovementBehaviour.rb.transform.TransformDirection(calculatedPlayermovement), 
                          Color.red, 0.5f);
#endif
        }

        return calculatedPlayermovement;
    }
 

    public void onJump()
    {       
        if (Input.GetButtonDown("Jump"))
        {
            playerMovementBehaviour.jump();            
        }
    }

    public void UsingAttackButton()
    {
        Instantiate(bullet, shootPotion.position,  Quaternion.identity);        
    }

    void AttackKeyOnKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(bullet, shootPotion.position, Quaternion.identity);            
        }
    }


    public void OnDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(playerMovementBehaviour.Dash());
            Debug.Log("Dashing");
        }
    }
}
