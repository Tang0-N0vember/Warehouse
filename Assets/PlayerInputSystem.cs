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
    [SerializeField]private GameObject playerCamera;
    private CinemachineComposer composer;

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

        //composer = playerCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineComposer>();

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
        transform.Rotate(Vector3.up * inputView.x * viewSpeed * Time.deltaTime);

        
        xRotation -= inputView.y;
        //playerCamera.GetComponent<CinemachineComposer>().m_TrackedObjectOffset.y += xRotation;
        //playerCamera.GetComponent<CinemachineComposer>().m_TrackedObjectOffset.y = Mathf.Clamp(composer.m_TrackedObjectOffset.y, -cameraClamp, cameraClamp);
        //composer.m_TrackedObjectOffset.y += xRotation;
        //composer.m_TrackedObjectOffset.y = Mathf.Clamp(composer.m_TrackedObjectOffset.y, -cameraClamp, cameraClamp);

        /*
        xRotation = Mathf.Clamp(xRotation, -cameraClamp, cameraClamp);
        Vector3 targetRoation = transform.eulerAngles;
        targetRoation.x = xRotation;
        playerCamera.transform.Rotate(Vector3.right*xRotation*Time.deltaTime);*/

        inputMovement = playerInputActions.Player.Movement.ReadValue<Vector2>();
        horizontalMovement = (transform.right * inputMovement.x + transform.forward * inputMovement.y);
        characterController.Move(horizontalMovement * moveSpeed * Time.deltaTime);
        /*inputView = playerInputActions.Player.View.ReadValue<Vector2>();
        characterController.transform.Rotate(new Vector3(inputView.x, 0, 0) * speed * Time.deltaTime);*/
    }
}
