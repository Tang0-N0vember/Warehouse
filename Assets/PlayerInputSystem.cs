using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerInputSystem : MonoBehaviour
{
    private PlayerInputSystem playerInputSystem;
    private PlayerInputActions playerInputActions;
    private CharacterController characterController;
    [SerializeField] private Transform cameraFollow;
    private Animator animator;

    private bool lookingAround=false;

    Vector2 inputView;
    Vector2 inputMovement;
    Vector3 horizontalMovement;


    [SerializeField] float viewSpeed = 5f;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float cameraClamp = 85f;
    float xRotation=0f;
    float mouseX, mouseY;
    private void Awake()
    {
        playerInputSystem = GetComponent<PlayerInputSystem>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();


        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Movement.performed += Movement_performed;
        playerInputActions.Player.View.performed += ViewMovement_performed;
        playerInputActions.Player.LookAround.performed += LookAround_performed;
        playerInputActions.Player.Interaction.performed += Interaction_performed;

    }

    private void Interaction_performed(InputAction.CallbackContext context)
    {
        Debug.Log("Interacting Around");
    }

    private void LookAround_performed(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                Debug.Log("Looking Around");
                lookingAround = true;   
                break;
            case InputActionPhase.Canceled:
                Debug.Log("Not Looking Around");
                break;
        }
        

    }

    private void ViewMovement_performed(InputAction.CallbackContext context)
    {
        inputView = context.ReadValue<Vector2>();
        mouseX = inputView.x * viewSpeed;
        mouseY= inputView.y * viewSpeed;
        
    }

    private void Movement_performed(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
        
    }

    

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + (inputView.x * viewSpeed * Time.deltaTime), 0f);

        inputMovement = playerInputActions.Player.Movement.ReadValue<Vector2>();
        horizontalMovement = (transform.right * inputMovement.x + transform.forward * inputMovement.y);
        characterController.Move(horizontalMovement * moveSpeed * Time.deltaTime);

        Vector3 rotationValues = cameraFollow.rotation.eulerAngles;
        xRotation -= inputView.y*viewSpeed*Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -cameraClamp, cameraClamp);
        cameraFollow.rotation= Quaternion.Euler(xRotation,rotationValues.y,rotationValues.z);

        
    }
    private void FixedUpdate()
    {
        animator.SetFloat("VelocityZ", inputMovement.y);
        animator.SetFloat("VelocityX", inputMovement.x);
    }
    private void Moving()
    {
        
    }
    private void StopMoving()
    {
        characterController.Move(Vector3.zero);
    }
    private void LookAround()
    {
        Vector3 rotationValues = cameraFollow.rotation.eulerAngles;
        cameraFollow.rotation = Quaternion.Euler(rotationValues.x, inputView.x * viewSpeed * Time.deltaTime, rotationValues.z);
    }
}
