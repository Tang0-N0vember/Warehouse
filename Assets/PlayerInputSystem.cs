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


        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Movement.performed += Movement_performed;
        playerInputActions.Player.View.performed += ViewMovement_performed;

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
        //characterController.Move(new Vector3(inputMovement.x, 0, inputMovement.y) * speed*Time.deltaTime);
    }

    // Start is called before the first frame update
    private void Update()
    {
        //transform.Rotate(Vector3.up * inputView.x * viewSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y+(inputView.x*viewSpeed*Time.deltaTime), 0f);

        Vector3 rotationValues = cameraFollow.rotation.eulerAngles;
        xRotation -= inputView.y*viewSpeed*Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -cameraClamp, cameraClamp);
        //rotationValues.x = xRotation;
        cameraFollow.rotation= Quaternion.Euler(xRotation,rotationValues.y,rotationValues.z);

        inputMovement = playerInputActions.Player.Movement.ReadValue<Vector2>();
        horizontalMovement = (transform.right * inputMovement.x + transform.forward * inputMovement.y);
        characterController.Move(horizontalMovement * moveSpeed * Time.deltaTime);
        /*inputView = playerInputActions.Player.View.ReadValue<Vector2>();
        characterController.transform.Rotate(new Vector3(inputView.x, 0, 0) * speed * Time.deltaTime);*/
    }
}
