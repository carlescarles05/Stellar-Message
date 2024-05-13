using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public GameObject quizPanel; //Ref al objeto Panel del quiz
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (other.gameObject.CompareTag("Player")) // Verifica si se presiona la tecla "E"
            {
                quizPanel.SetActive(true);
            }
        }
    }
}

