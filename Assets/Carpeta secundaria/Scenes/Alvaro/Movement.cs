using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5;
    public float rotationSpeed = 720;

    public Animator animator;

    private float x, y;
    public Rigidbody rb;
    public float jumpForce = 5;

    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;
    private bool isGrounded;

    

    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        transform.Rotate(0, x *Time.deltaTime * rotationSpeed, 0);
        transform.Translate(0, 0, y * speed * Time.deltaTime* speed);

        animator.SetFloat("VelX", x);
        animator.SetFloat("VelY", y);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {   
            animator.Play("Jump");
            Invoke("Jump", 0.1f);
        }
    }
    public void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

    }
}
