using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public GameObject pinkCylinder;
    public GameObject yellowSquare;
    public GameObject greenSphere;
    private AudioSource audioSource;
    private bool puzzleCompleted = false;

    public Transform pinkSlot;
    public Transform yellowSlot;
    public Transform greenSlot;

    private bool pinkPlaced = false;
    private bool yellowPlaced = false;
    private bool greenPlaced = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Verificar si los objetos están encima de los slots correspondientes
        pinkPlaced = IsObjectAboveSlot(pinkCylinder, pinkSlot);
        yellowPlaced = IsObjectAboveSlot(yellowSquare, yellowSlot);
        greenPlaced = IsObjectAboveSlot(greenSphere, greenSlot);

        if (pinkPlaced && yellowPlaced && greenPlaced && !puzzleCompleted && ( !IsObjectBeingHeld(pinkCylinder) && !IsObjectBeingHeld(yellowSquare) && !IsObjectBeingHeld(greenSphere)))
        {
            // Marcar el puzzle como completo
            puzzleCompleted = true;

            Debug.Log("¡Enhorabuena! Has completado el puzzle.");

            // Reproducir el efecto de sonido
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
            Debug.Log("Estado del puzzle: " + puzzleCompleted);

        }
    }

    private bool IsObjectAboveSlot(GameObject puzzleObject, Transform slot)
    {
        // Obtener la posición del objeto y del slot en el mismo plano Y
        Vector3 objectPosition = new Vector3(puzzleObject.transform.position.x, 0, puzzleObject.transform.position.z);
        Vector3 slotPosition = new Vector3(slot.position.x, 0, slot.position.z);

        // Calcular la distancia entre el objeto y el slot en el mismo plano Y
        float distance = Vector3.Distance(objectPosition, slotPosition);

        // Si la distancia es menor que un cierto umbral, consideramos que el objeto está encima del slot
        float threshold = 1f; // Puedes ajustar este valor según tus necesidades
        return distance < threshold;
    }

    private bool IsObjectBeingHeld(GameObject puzzleObject)
    {
        // Verificar si el objeto tiene un componente Rigidbody y está siendo sujetado por el jugador
        if (puzzleObject.GetComponent<Rigidbody>().isKinematic == true)
        {
            return true; // El objeto está siendo sostenido por el jugador
        }

        return false; // El objeto no está siendo sostenido por el jugador
    }
}
