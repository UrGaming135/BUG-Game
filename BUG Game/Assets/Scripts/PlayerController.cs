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
    private float horizontalRotationSpeed = 5f;
    [SerializeField]
    private float verticalRotationSpeed = 5f;
    [SerializeField]
    private Transform cameraArm;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Transform cameraTransform;

    private InputAction shootAction;
    private InputAction moveAction;
    private InputAction lookAction;

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
        lookAction = playerInput.actions["Look"];
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
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime;

        var lookDelta = lookAction.ReadValue<Vector2>();
        var rotationX = lookDelta.x;
        var rotationY = lookDelta.y;

        transform.Rotate(new Vector3(0, rotationX * horizontalRotationSpeed * Time.deltaTime, 0));
        var newRotation = new Vector3(-rotationY * verticalRotationSpeed * Time.deltaTime, 0, 0);
        print(newRotation);
        cameraArm.Rotate(newRotation);

        
        //var newRotation = new Vector3(cameraArm.rotation.eulerAngles.y - rotationY * verticalRotationSpeed * Time.deltaTime, 0, 0);
        //cameraArm.rotation = Quaternion.Slerp(cameraArm.rotation, Quaternion.Euler(newRotation), verticalRotationSpeed * Time.deltaTime);
    }
}
