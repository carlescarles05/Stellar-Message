using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 10f;
    public AudioSource steps;
    private bool HActive;
    private bool VActive;

    private void Start()
    {
        
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Horizontal"))
        {
            HActive = true;
            steps.Play();
        }

        if (Input.GetButtonDown("Vertical"))
        {
            VActive = true;
            steps.Play();
        }

        if (Input.GetButtonUp("Horizontal"))
        {
            HActive = false;
            if (VActive == false)
            {
                steps.Pause();
            }
        }

        if (Input.GetButtonUp("Vertical"))
        {
            VActive = false;
            if (HActive == false)
            {
                steps.Pause();
            }
        }
    }

}
