using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Input input;
    Rigidbody rb;

    [Header("Player Movement Settings")]
    public float moveSpeed;
    public float cameraSpeed;
    public Transform cameraTransform;
    public float jumpForce;

    [Header("Player Interaction Settings")]
    public InteractionController interactionController;

    Vector2 moveVector;
    Vector2 cameraVector;
    float xRotation;

    private void Awake()
    {
        // Input Actions Setup //
        input = new Input();

        // Map Input Actions To Functions And Variables //
        input.Gameplay.Movement.performed += ctx => moveVector = ctx.ReadValue<Vector2>();
        input.Gameplay.Movement.canceled += ctx => moveVector = Vector2.zero;
        input.Gameplay.Camera.performed += ctx => cameraVector = ctx.ReadValue<Vector2>();
        input.Gameplay.Camera.canceled += ctx => cameraVector = Vector2.zero;
        input.Gameplay.Jump.performed += ctx => Jump();
        input.Gameplay.Inventory.performed += ctx => interactionController.changeSlot(ctx.ReadValue<float>());
        input.Gameplay.Interact.performed += ctx => interactionController.tryInteract(cameraTransform, transform);
        input.Gameplay.Escape.performed += ctx => interactionController.tryEscape();

        // Rigidbody Setup //
        rb = GetComponent<Rigidbody>();

        // Mouse Setup //
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!interactionController.isViewCam)
        { 
            // Move Player Relative To Facing Direction //
            transform.Translate(new Vector3(moveVector.x, 0f, moveVector.y) * moveSpeed * Time.deltaTime);

            // Rotate Player Left And Right //
            transform.Rotate(Vector3.up * cameraVector.x * cameraSpeed * Time.deltaTime);

            // Rotate Camera Up And Down //
            xRotation -= cameraVector.y * cameraSpeed * Time.deltaTime;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce);
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
