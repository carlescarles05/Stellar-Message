using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{


    public void Play()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("SampleScene");
    }
    public void Exit()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Application.Quit();
    }
}
