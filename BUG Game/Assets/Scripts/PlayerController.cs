using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 5f;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Transform cameraTransform;

    private InputAction shootAction;
    private InputAction moveAction;

    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        // Set up actions
        moveAction = playerInput.actions["Move"];
        shootAction = playerInput.actions["Shoot"];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime;

        var targetAngle = cameraTransform.eulerAngles.y;
        var targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
