using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Cinemachine;

public class PlayerInputSystem : MonoBehaviour
{
    private PlayerInputSystem playerInputSystem;
    private PlayerInputActions playerInputActions;
    private CharacterController characterController;
    [SerializeField] private GameObject Weapon;
    [SerializeField] private Transform cameraFollow;
    [SerializeField] private GameObject vCam3rdPerson;
    [SerializeField] private GameObject vCamAim3rdPerson;
    [SerializeField] private GameObject CrossHair;


    private Animator animator;

    //private bool lookingAround=false;

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
        Cursor.visible = false;


        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Movement.performed += Movement_performed;
        playerInputActions.Player.View.performed += ViewMovement_performed;
        playerInputActions.Player.LookAround.performed += LookAround_performed;
        playerInputActions.Player.LookAround.canceled += LookAround_canceled;
        playerInputActions.Player.Aim.performed += Aim_performed;
        playerInputActions.Player.Aim.canceled += Aim_canceled;
        playerInputActions.Player.Interaction.performed += Interaction_performed;

    }

    

    private void Aim_performed(InputAction.CallbackContext context)
    {
        Debug.Log("Aiming");
        vCam3rdPerson.SetActive(false);
        vCamAim3rdPerson.SetActive(true);
        SwitchWeaponState();
        CrossHair.SetActive(true);
        animator.SetBool("Aim", true);

        playerInputActions.Player.LookAround.Disable();
    }
    private void Aim_canceled(InputAction.CallbackContext context)
    {
        Debug.Log("Stoped Aiming");
        vCam3rdPerson.SetActive(true);
        vCamAim3rdPerson.SetActive(false);
        SwitchWeaponState();
        CrossHair.SetActive(false);
        animator.SetBool("Aim", false);

        playerInputActions.Player.LookAround.Enable();
    }
    private void Interaction_performed(InputAction.CallbackContext context)
    {
        Debug.Log("Interacting Around");
    }

    private void LookAround_performed(InputAction.CallbackContext context)
    {
        Debug.Log("Looking Around");
        playerInputActions.Player.Movement.Disable();

    }
    private void LookAround_canceled(InputAction.CallbackContext context)
    {
        Debug.Log("Not Looking Around");
        playerInputActions.Player.Movement.Enable();
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
        if (!playerInputActions.Player.Movement.enabled) LookAround(); else MovingAround();


        inputMovement = playerInputActions.Player.Movement.ReadValue<Vector2>();
        horizontalMovement = (transform.right * inputMovement.x + transform.forward * inputMovement.y);
        characterController.Move(horizontalMovement * moveSpeed * Time.deltaTime);
    }
    private void FixedUpdate()
    {
        animator.SetFloat("VelocityZ", inputMovement.y);
        animator.SetFloat("VelocityX", inputMovement.x);
    }
    public void MovingAround()
    {
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + (inputView.x * viewSpeed * Time.deltaTime), 0f);

        Vector3 rotationValues = cameraFollow.rotation.eulerAngles;
        xRotation -= inputView.y * viewSpeed * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -cameraClamp, cameraClamp);
        cameraFollow.rotation = Quaternion.Euler(xRotation, rotationValues.y, rotationValues.z);
    }
    public void LookAround()
    {
        Vector3 rotationValues = cameraFollow.rotation.eulerAngles;
        xRotation -= inputView.y * viewSpeed * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -cameraClamp, cameraClamp);
        cameraFollow.rotation = Quaternion.Euler(xRotation, cameraFollow.rotation.eulerAngles.y+ (inputView.x * viewSpeed * Time.deltaTime), 0f);
    }
    public void SwitchWeaponState()
    {
        Weapon.SetActive(!Weapon.activeSelf);
    }
}
