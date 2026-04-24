using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float moveSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float crouchSpeed;
    [SerializeField] float sensitivity;
    [SerializeField] float cameraHeightNormal = 0.6f;
    [SerializeField] float cameraHeightCrouch = 0.3f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float minLimit = -80f;
    [SerializeField] float maxLimit = 80f;
    [SerializeField] Transform cameraTransform;


    [SerializeField] Movimiento inputAction;
    [SerializeField] CharacterController characterController;
    private Vector2 move;
    private Vector2 look;
    private Vector2 speed;
    private float currentRotationY;
    private bool isRunning = false;
    private bool isCrouching = false;

    private void Awake()
    {
        inputAction = new Movimiento();
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        inputAction.Player.Enable();
        inputAction.Player.Move.performed += SetMovement;
        inputAction.Player.Move.canceled += obj => move = Vector2.zero;
        inputAction.Player.Look.performed += SetLook;
        inputAction.Player.Look.canceled += obj => look = Vector2.zero;
        inputAction.Player.Run.performed += obj => Run();
        inputAction.Player.Run.canceled += obj => StopRunning();
        inputAction.Player.Crouch.performed += obj => Crouch();
        inputAction.Player.Crouch.canceled += obj => StandUp();
        inputAction.Player.Jump.performed += obj => Jump();
    }

    private void SetLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }    
    private void SetMovement(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        Movement();
        Look();
    }

    private void Look()
    {
        Vector2 mouseDelta = look * sensitivity;
        currentRotationY = Mathf.Clamp(currentRotationY - mouseDelta.y, minLimit, maxLimit);
        cameraTransform.localRotation = Quaternion.Euler(currentRotationY, 0f, 0f);
        
        float targetHeight = isCrouching ? cameraHeightCrouch : cameraHeightNormal;
        Vector3 cameraPos = cameraTransform.localPosition;
        cameraPos.y = targetHeight;
        cameraTransform.localPosition = cameraPos;
        
        transform.Rotate(Vector3.up * mouseDelta.x);
    }
    private void Movement()
    {
        Vector3 move = transform.right * this.move.x + transform.forward * this.move.y;
        float currentSpeed = moveSpeed;
        
        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        else if (isRunning)
        {
            currentSpeed = runSpeed;
        }
        
        characterController.Move(move * currentSpeed * Time.deltaTime);

        speed.y += gravity * Time.deltaTime;
        characterController.Move(speed * Time.deltaTime);
    }

    public void Run()
    {
        isRunning = true;
    }

    public void StopRunning()
    {
        isRunning = false;
    }

    public void Crouch()
    {
        isCrouching = true;
        isRunning = false;
    }

    public void StandUp()
    {
        isCrouching = false;
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            speed.y = jumpForce;
        }
    }
}
