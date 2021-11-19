using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public RangedWeapon currentWeapon;

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
    private float dashSpeed = 2;
    [SerializeField]
    private float dashTime = .5f;
    [SerializeField]
    private float dashCooldown = 5f;
    [SerializeField]
    private Transform cameraArm;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Transform cameraTransform;
    private CameraManager cameraManager;

    private InputAction attackAction;
    private InputAction moveAction;
    private InputAction dashAction;
    private InputAction lookAction;
    private InputAction cameraRightAction;
    private InputAction cameraLeftAction;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Vector3 moveDirection;

    private RangedWeapon[] rangedWeapons;
    private bool dashing;
    private float nextDash;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        cameraManager = GetComponentInChildren<CameraManager>();

        rangedWeapons = GetComponentsInChildren<RangedWeapon>();
        currentWeapon = rangedWeapons[0];

        // Set up actions
        moveAction = playerInput.actions["Move"];
        dashAction = playerInput.actions["Dash"];
        attackAction = playerInput.actions["Attack"];
        lookAction = playerInput.actions["Look"];
        cameraRightAction = playerInput.actions["CameraMoveRight"];
        cameraLeftAction = playerInput.actions["CameraMoveLeft"];
    }

    private void OnEnable()
    {
        if (currentWeapon != null)
        {
            attackAction.started += _ => currentWeapon.StartAttacking();
            attackAction.canceled += _ => currentWeapon.StopAttacking();
        }
        dashAction.performed += _ => Dash();
        cameraRightAction.performed += _ => cameraManager.MoveCameraRight();
        cameraLeftAction.performed += _ => cameraManager.MoveCameraLeft();
    }

    private void OnDisable()
    {
        if (currentWeapon != null)
        {
            attackAction.started -= _ => currentWeapon.StartAttacking();
            attackAction.canceled -= _ => currentWeapon.StopAttacking();
        }
        dashAction.performed -= _ => Dash();
        cameraRightAction.performed -= _ => cameraManager.MoveCameraRight();
        cameraLeftAction.performed -= _ => cameraManager.MoveCameraLeft();
    }

    // Update is called once per frame
    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }

        if (!dashing)
        {
            Vector2 input = moveAction.ReadValue<Vector2>();
            moveDirection = new Vector3(input.x, 0, input.y);
            moveDirection = moveDirection.x * cameraTransform.right.normalized + moveDirection.z * cameraTransform.forward.normalized;
            moveDirection.y = playerVelocity.y;
            controller.Move(moveDirection * Time.deltaTime * playerSpeed);

            playerVelocity.y += gravityValue * Time.deltaTime;

            //print(moveDirection);
        }

        var lookDelta = lookAction.ReadValue<Vector2>();
        var rotationX = lookDelta.x;
        var rotationY = lookDelta.y;

        transform.Rotate(new Vector3(0, rotationX * horizontalRotationSpeed * Time.deltaTime, 0));
        var newRotation = new Vector3(-rotationY * verticalRotationSpeed * Time.deltaTime, 0, 0);
        cameraArm.Rotate(newRotation);
    }

    void Dash()
    {
        if (Time.time >= nextDash)
        {
            nextDash = Time.time + dashCooldown;
            StartCoroutine(DashCoroutine());
        } else
        {
            //print("Dash on cooldown");
        }
    }

    IEnumerator DashCoroutine()
    {
        dashing = true;
        //print("Dash");
        float startTime = Time.time;
        while(Time.time < startTime + dashTime)
        {
            controller.Move(moveDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }
        dashing = false;
    }

    void WeaponSwap()
    {

    }
}
