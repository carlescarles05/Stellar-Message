using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGun : MonoBehaviour
{
    public GameObject handObject; // Referencia al objeto "Hand" del jugador
    public GameObject playerGun; // Referencia al objeto de la pistola del jugador

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            // Desactivar el objeto "Hand"
            handObject.SetActive(false);

            // Activar la pistola del jugador
            playerGun.SetActive(true);
        }
    }
}
