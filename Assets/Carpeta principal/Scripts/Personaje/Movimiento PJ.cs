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
    [SerializeField] GameObject diary;


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

    // Actualiza el vector de rotación de cámara según el movimiento del ratón
    private void SetLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }    
    // Actualiza el vector de movimiento según las teclas de dirección presionadas
    private void SetMovement(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;
        
        diaryActions();
        Movement();
        Look();
    }

    // Maneja la rotación de la cámara vertical y horizontal, y ajusta su altura cuando se agacha
    private void Look()
    {
        if (diary.activeInHierarchy == false) 
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
    }
    // Maneja el movimiento del personaje, aplicando gravedad y la velocidad correspondiente (correr, caminar, agacharse)
    private void Movement()
    {
        if(diary.activeInHierarchy == false)
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
    }
    // Controla la apertura y cierre del diario con la tecla Tab, mostrando u ocultando el cursor
    private void diaryActions()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && diary.activeInHierarchy == false)
        {
            diary.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && diary.activeInHierarchy == true)
        {
            diary.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Activa el estado de correr para aumentar la velocidad de movimiento
    public void Run()
    {
        isRunning = true;
    }

    // Desactiva el estado de correr volviendo a la velocidad normal de caminata
    public void StopRunning()
    {
        isRunning = false;
    }

    // Activa el estado de agacharse, reduce la velocidad y la escala vertical del personaje
    public void Crouch()
    {
        isCrouching = true;
        isRunning = false;
        
        // Reducir la escala a la mitad al agacharse
        Vector3 newScale = transform.localScale;
        newScale.y *= 0.5f;
        transform.localScale = newScale;
    }

    // Desactiva el estado de agacharse y restaura la escala vertical original del personaje
    public void StandUp()
    {
        isCrouching = false;
        
        // Restaurar la escala original al levantarse
        Vector3 newScale = transform.localScale;
        newScale.y *= 2f;
        transform.localScale = newScale;
    }

    // Hace saltar al personaje aplicando una fuerza vertical, solo si está en el suelo
    public void Jump()
    {
        if (characterController.isGrounded)
        {
            speed.y = jumpForce;
        }
    }
}
