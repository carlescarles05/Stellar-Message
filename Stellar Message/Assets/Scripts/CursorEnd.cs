using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorEnd : MonoBehaviour
{
    // Método que se llama al iniciar la escena
    void Start()
    {
        // Desbloquea y muestra el cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
