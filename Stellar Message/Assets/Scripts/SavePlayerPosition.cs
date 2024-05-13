using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePlayerPosition : MonoBehaviour
{
    public void SavePositionAndLoadOtherScene(string Quiz)
    {
        // Guardar la posición actual del jugadors
        PlayerPrefs.SetFloat("PlayerPosX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", transform.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", transform.position.z);

        // Cargar la otra escena
        SceneManager.LoadScene(Quiz);
    }
}
