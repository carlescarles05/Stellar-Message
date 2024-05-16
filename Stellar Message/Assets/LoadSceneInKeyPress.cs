using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneInKeyPress : MonoBehaviour
{
    // Nombre de la escena que quieres cargar
    public string final;

    // Método que se llama cuando otro objeto entra en el trigger
    private void OnTriggerEnter(Collider other)
    {
        // Verifica que el objeto que entra es el jugador (por ejemplo, con el tag "Player")
        if (other.CompareTag("Player"))
        {
            // Desbloquea y muestra el cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Carga la escena especificada
            SceneManager.LoadScene("final");
        }
    }
}
