using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToSampleScene : MonoBehaviour
{
    public void ReturntoSampleScene()
    {
        // Cargar la escena "SampleScene"
        SceneManager.LoadScene("SampleScene");
    }
}
