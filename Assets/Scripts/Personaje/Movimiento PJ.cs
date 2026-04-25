using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float moveSpeed;
    [SerializeField] float sensitivity;
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
        diaryActions();
        Movement();
        Look();
    }

    private void Look()
    {
        if (diary.activeInHierarchy == false) 
        {
        Vector2 mouseDelta = look * sensitivity;
        currentRotationY = Mathf.Clamp(currentRotationY - mouseDelta.y, minLimit, maxLimit);
        cameraTransform.localRotation = Quaternion.Euler(currentRotationY, 0f, 0f); 
        transform.Rotate(Vector3.up * mouseDelta.x);
        }
    }
    private void Movement()
    {
        if(diary.activeInHierarchy == false)
        {
        Vector3 move = transform.right * this.move.x + transform.forward * this.move.y;
        characterController.Move(move * moveSpeed * Time.deltaTime);

        speed.y += gravity * Time.deltaTime;
        characterController.Move(speed * Time.deltaTime);
        }
    }
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
}
