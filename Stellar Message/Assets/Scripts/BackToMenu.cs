using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{

    private void Start()
    {
        // Desbloquea y muestra el cursor
        Cursor.lockState = CursorLockMode.None;
    }
    public void Menu()
    {
        
        SceneManager.LoadScene("menu");
    }
}
