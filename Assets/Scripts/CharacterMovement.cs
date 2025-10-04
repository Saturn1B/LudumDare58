using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Camera Control")]
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    [SerializeField] private float lookSensitivity;

    [Header("Player Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float jumpHeight;

    [Header("Crouch")]
    [SerializeField] private float standingHeight;
    [SerializeField] private float crouchingHeight;

    private CharacterController characterController;
    private Camera playerCamera;

    private float yaw;
    private float pitch;

    private Vector3 velocity = Vector3.zero;
    private float gravity = 9.81f;
    private bool isCrouching;
    private float crouchTransitionSpeed = .1f;

    private bool canMove = true;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMouseLook();

        if (!canMove)
        {
            velocity.y -= gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);

            return;
        }

        HandleMovement();
        HandleCrouch();
    }

    private void HandleMovement()
    {
        float currentSpeed = isCrouching ? crouchSpeed : Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        float horizontal = Input.GetAxis("Horizontal") * currentSpeed;
        float vertical = Input.GetAxis("Vertical") * currentSpeed;

        Vector3 moveDirection = new Vector3(horizontal, 0.0f, vertical);
        moveDirection = transform.rotation * moveDirection;

        HandleJump();

        velocity.x = moveDirection.x;
        velocity.z = moveDirection.z;

        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleMouseLook()
    {
        yaw += Input.GetAxisRaw("Mouse X") * lookSensitivity;
        pitch -= Input.GetAxisRaw("Mouse Y") * lookSensitivity;

        pitch = ClampAngle(pitch, minPitch, maxPitch);

        transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
        playerCamera.transform.localEulerAngles = new Vector3(pitch, 0.0f, 0.0f);
    }

    private void HandleJump()
    {
        if (characterController.isGrounded)
        {
            velocity.y = -.5f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = jumpHeight;
            }
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCoroutine(CrouchStand());
        }
    }

    private IEnumerator CrouchStand()
    {
        float targetHeight = isCrouching ? standingHeight : crouchingHeight;
        float initialHeight = characterController.height;

        float timeElapsed = 0f;

        while (timeElapsed < crouchTransitionSpeed)
        {
            characterController.height = Mathf.Lerp(initialHeight, targetHeight, timeElapsed / crouchTransitionSpeed);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterController.height = targetHeight;
        isCrouching = !isCrouching;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}
