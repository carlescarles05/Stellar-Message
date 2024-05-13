using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestorePlayerPosition : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerPosX") && PlayerPrefs.HasKey("PlayerPosY") && PlayerPrefs.HasKey("PlayerPosZ"))
        {
            Vector3 playerPos = new Vector3(PlayerPrefs.GetFloat("PlayerPosX"), PlayerPrefs.GetFloat("PlayerPosY"), PlayerPrefs.GetFloat("PlayerPosZ"));
            transform.position = playerPos;
        }
    }
}
