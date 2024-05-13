using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSController : MonoBehaviour
{
    //Referencias privadas
    Rigidbody rb;
    Animator anim;

    [Header("Movement & Look Stats")]
    [SerializeField] GameObject camHolder;
    public float speed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float maxForce;
    public float sensitivity;
    bool isSprinting;
    bool isCrouching;

    [Header("Jumping & GroundCheck Configuration")]
    public float jumpForce;
    //GroundCheck
    [SerializeField] GameObject groundCheck;
    [SerializeField] bool isGrounded;    
    [SerializeField] float groundDetectRadius = 0.1f;
    [SerializeField] LayerMask groundLayer;


    //Valores privados
    Vector2 move;
    Vector2 look;
    float lookRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        camHolder = GameObject.Find("CameraHolder");
        groundCheck = GameObject.Find("GroundCheck");
    }

    // Start is called before the first frame update
    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundDetectRadius, groundLayer);
        
    }

    private void FixedUpdate()
    {
        Movement();
        
    }

    private void LateUpdate()
    {
        //Movimiento de la cámara
        CameraMoveLook();

    }

    void Movement()
    {
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(move.x, 0, move.y);
        targetVelocity *= isCrouching ? crouchSpeed : (isSprinting ? sprintSpeed : speed);

        //Alinear la dirección con la orientación correcta
        targetVelocity = transform.TransformDirection(targetVelocity);

        //Calcular las fuerzas que afectan al movimiento
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);
        //Limitar la fuerza máxima
        Vector3.ClampMagnitude(velocityChange, maxForce);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void CameraMoveLook()
    {
        //Girar
        transform.Rotate(Vector3.up * look.x * sensitivity);
        //Mirar
        lookRotation += (-look.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.eulerAngles = new Vector3(lookRotation, camHolder.transform.eulerAngles.y, camHolder.transform.eulerAngles.z);

    }

    void Jump()
    {
        Vector3 jumpForces = rb.velocity;
        if (isGrounded)
        {
            jumpForces.y = jumpForce;
        }

        rb.velocity = jumpForces;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Jump();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        
        isCrouching = context.ReadValueAsButton();
        anim.SetBool("isCrouching", isCrouching);

    }
}
