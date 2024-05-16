using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Damage Configuration")]
    [SerializeField] float health;
    [SerializeField] float maxHealth;

    [Header("Feedback System")]
    [SerializeField] Material original;
    [SerializeField] Material damaged;
    [SerializeField] float feedbackTIme;
    private AudioSource audioSource;
    [SerializeField] GameObject deathEffect;
    GameObject model; //Ref al objeto que contiene el mesh del personaje (solo en caso de que el mesh vaya aparte del código)
    MeshRenderer modelRend; //Ref al meshRenderer del objeto con modelado (permite acceder a su material)

    void Awake()
    {
        model = GameObject.Find("Body");
        modelRend = model.GetComponent<MeshRenderer>();
        original = modelRend.material;
        health = maxHealth;
    }

    void Update()
    {
        HealthManagement();
    }

    void HealthManagement()
    {
        if (health <= 0)
        {
            deathEffect.SetActive(true);
            deathEffect.transform.position = transform.position;
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damageToTake)
    {
        //Aquí cabe codear cualquier efecto de recibir daño que se desee
        modelRend.material = damaged; //FEEDBACK DE RECIBIR DAÑO (EN ESTE CASO CAMBIO DE COLOR)
        health -= damageToTake;

        Invoke(nameof(ResetMaterial), feedbackTIme);
    }

    void ResetMaterial()
    {
        modelRend.material = original;
    }
}
