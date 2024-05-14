using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    // M�todo llamado cuando se produce una colisi�n
    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si la colisi�n es con un enemigo
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Cargar la escena de Game Over
            //SceneManager.LoadScene("GameOver");
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }
}
