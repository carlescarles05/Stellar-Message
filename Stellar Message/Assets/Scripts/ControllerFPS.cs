using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControllerFPS : MonoBehaviour
{
    // Referencias privadas
    Rigidbody rb;
    Animator anim;
    

    [Header("Movement & Look Stats")]
    [SerializeField] GameObject camHolder;
    public float speed;
    public float maxForce;
    public float sensitivity;
    [SerializeField] GameObject crosshair;

    [Header("Player Status")]
    public bool hasWeapon;

    // Valores privados
    Vector2 move;
    Vector2 look;
    float lookRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        camHolder = GameObject.Find("CameraHolder");
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (hasWeapon){ Cursor.lockState = CursorLockMode.Locked;}
        else { Cursor.lockState = CursorLockMode.None; }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            crosshair.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        Movement();
    }

    void LateUpdate()
    {
        CameraMoveLook();
    }


    void Movement()
    {
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(move.x, 0, move.y) * speed;

        // Alinear la dirección con la orientación correcta
        targetVelocity = transform.TransformDirection(targetVelocity);

        // Calcular las fuerzas que afectan al movimiento
        Vector3 velocityChange = targetVelocity - currentVelocity;
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);
        // Limitar la fuerza máxima
        Vector3.ClampMagnitude(velocityChange, maxForce);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void CameraMoveLook()
    {
        // Girar
        transform.Rotate(Vector3.up * look.x * sensitivity);
        // Mirar
        lookRotation += (-look.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.eulerAngles = new Vector3(lookRotation, camHolder.transform.eulerAngles.y, camHolder.transform.eulerAngles.z);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }
}
