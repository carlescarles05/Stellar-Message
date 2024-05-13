using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickObject : MonoBehaviour
{
    private ControllerFPS playerController; //Declara variable llamada playerController
    public GameObject handPoint; //Declara variable pública handPoint, para representar el punto en el que se cogerán los objetos
    public GameObject handObject; //Objeto mano del jugador
    public GameObject playerGun; // Referencia al objeto de la pistola del jugador

    private GameObject pickedObject = null; //Variable que iniciamos como nula, se utilizará para hacr un seguimiento del objeto que el jugador ha recogido
    private bool canPickupGun = false; //Esta variable se utiliza para determinar si el jugador puede recoger un arma.
    private bool canPickupObject = true; //Esta variable se utiliza para determinar si el jugador puede recoger un objeto genérico (que no sea un arma).
    private bool PickUpPicker = false;

    private void Start()
    {
        playerController = GetComponent<ControllerFPS>(); // Se asigna el componente ControllerFPS del GameObject actual a la variable playerController
    }

    void Update()
    {
        if (pickedObject != null) { playerController.hasWeapon = true; } // Si hay un objeto recogido, se establece hasWeapon de playerController en true
        else { playerController.hasWeapon = false; } // Si no hay un objeto recogido, se establece hasWeapon de playerController en false

        // Si tenemos un objeto recogido y presionamos la tecla "R", lo soltamos
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (pickedObject != null)
            {
                Debug.Log("hola");
                Destroy(playerGun); // Llama a la función ReleaseObject para soltar el objeto
            }
            if (PickUpPicker == true)
            {
                PickUpPicker = false;
                ReleaseObject();
            }
        }

        // Restablecer la capacidad de recoger objetos si no tenemos ningún objeto recogido
        if (pickedObject == null)
        {
            canPickupObject = true; // Establece canPickupObject en true
        }       
    }

    private void OnTriggerStay(Collider other)
    {
        // Si podemos recoger un objeto y presionamos la tecla "E" y no tenemos ningún objeto recogido
        if (canPickupObject && other.gameObject.CompareTag("Object") && Input.GetKeyDown(KeyCode.E) && pickedObject == null)
        {
            canPickupObject = true; // Establece canPickupObject en true

            // Desactivar la gravedad y cinemática del objeto para recogerlo
            other.GetComponent<Rigidbody>().useGravity = false; // Desactiva la gravedad del Rigidbody del objeto colisionado
            other.GetComponent<Rigidbody>().isKinematic = true; // Establece isKinematic del Rigidbody del objeto colisionado en true
                                                                // Mover el objeto a la posición de la mano
            other.transform.position = handPoint.transform.position; // Establece la posición del objeto colisionado igual a la posición de handPoint
            other.transform.SetParent(handPoint.transform); // Establece el padre del objeto colisionado como handPoint
                                                            // Establecer el objeto recogido
            pickedObject = other.gameObject; // Asigna el objeto colisionado a pickedObject

            PickUpPicker = true;
        }

        if (other.gameObject.CompareTag("Weapon") && Input.GetKeyDown(KeyCode.E) && pickedObject == null)
        {
            // Restablecer la capacidad de recoger objetos
            canPickupObject = true; // Establece canPickupObject en true

            // Desactivar la gravedad y cinemática del objeto para recogerlo
            other.GetComponent<Rigidbody>().useGravity = false; // Desactiva la gravedad del Rigidbody del objeto colisionado
            other.GetComponent<Rigidbody>().isKinematic = true; // Establece isKinematic del Rigidbody del objeto colisionado en true

            // Desactivar el objeto "Hand"
            handObject.SetActive(false); // Desactiva el GameObject handObject
                                         // Desactivar el objeto "Weapon"
            other.gameObject.SetActive(false); // Desactiva el GameObject asociado al objeto colisionado
                                               // Establecer la variable haveGUN en true en el script SystemGun
            SystemGun gun = GetComponent<SystemGun>(); // Obtiene el componente SystemGun del GameObject actual
            gun.haveGUN = true; // Establece la variable haveGUN del componente SystemGun en true
                                // Activar la pistola del jugador
            playerGun.SetActive(true); // Activa el GameObject playerGun
        }
    }

    void ReleaseObject()
    {
        // Desactivar el collider trigger del objeto
        pickedObject.GetComponent<Collider>().isTrigger = true; // Establece isTrigger del Collider del objeto recogido en true
        pickedObject.GetComponent<Rigidbody>().useGravity = true; // Activa la gravedad del Rigidbody del objeto recogido
        pickedObject.GetComponent<Rigidbody>().isKinematic = false; // Establece isKinematic del Rigidbody del objeto recogido en false
        pickedObject.transform.SetParent(null); // Establece el padre del objeto recogido como null
        pickedObject = null; // Establecer el objeto recogido como nulo     
    }

    
}
