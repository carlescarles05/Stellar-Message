using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_AlienBase : MonoBehaviour
{
    [Header("AI Configuration")]
    [SerializeField] NavMeshAgent agent; //Ref al componente Agente, que permite que el objeto tenga IA
    [SerializeField] Transform target; //Ref al transform del objeto que la IA va a perseguir
    [SerializeField] LayerMask targetLayer; //Determina cual es la capa de detecci�n del target
    [SerializeField] LayerMask groundLayer; //Determina cual es la capa de detecci�n del suelo

    [Header("Patroling Stats")]
    public Vector3 walkPoint; //Direcci�n a la que la IA se va a mover si no detecta target
    [SerializeField] float walkPointRange; //Rango m�ximo de direcci�n de movimiento si la IA no detecta target
    bool walkPointSet; //Bool que determina si la IA ha llegado al objetivo y entonces cambia de objetivo

    [Header("Attack Configuration")]
    public float timeBetweenAttacks; //Tiempo de espera entre ataque y ataque
    bool alreadyAttacked; //Bool para determinar si se ha atacado
    //Variables necesarias si el ataque es un disparo F�SICO
    [SerializeField] GameObject projectile; //Ref al prefab del proyectil
    [SerializeField] Transform shootPoint; //Ref a la posici�n desde donde se disparan los proyectiles
    [SerializeField] float shootSpeedZ; //Velocidad de disparo hacia delante
    [SerializeField] float shootSpeedY; //Velocidad de disparo hacia arriba (solo si el disparo es catapulta/bolea)
    [SerializeField] float rotationSpeed = 10f;

    [Header("States & Detection")]
    [SerializeField] float sightRange; //Rango de detecci�n de persecuci�n de la IA
    [SerializeField] float attackRange; //Rango a partir del cual la IA ataca
    [SerializeField] bool targetInSightRange; //Bool que determina si el target est� a distancia de detecci�n
    [SerializeField] bool targetInAttackRange; //Bool que determina si el target est� a distancia de ataque

    private void Awake()
    {
        target = GameObject.Find("Player").transform; //Al inicio referencia el transform del Player, para poder perseguirlo cuando toca
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Chequear si el target est� en los rangos de detecci�n y de ataque
        targetInSightRange = Physics.CheckSphere(transform.position, sightRange, targetLayer);
        targetInAttackRange = Physics.CheckSphere(transform.position, attackRange, targetLayer);

        //Cambios din�micos de estado de la IA
        //Si no detecta el target ni est� en rango de ataque: PATRULLA
        if (!targetInSightRange && !targetInAttackRange) Patroling();
        //Si detecta el target pero no est� en rango de ataque: PERSIGUE
        if (targetInSightRange && !targetInAttackRange) ChaseTarget();
        //Si detecta el target y est� en rango de ataque: ATACA
        if (targetInSightRange && targetInAttackRange) AttackTarget();

    }

    void Patroling()
    {
        if (!walkPointSet)
        {
            //Si no existe punto al que dirigirse, inicia el m�todo de crearlo
            SearchWalkPoint();
        }
        else
        {
            //Si existe punto, el personaje mueve la IA hacia ese punto
            agent.SetDestination(walkPoint);
        }

        //Sistema para que la IA busque un nuevo destino de patrullaje una vez ha llegado al destino actual
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1) { walkPointSet = false; }

    }

    void SearchWalkPoint()
    {
        //Crear el sistema de puntos "random" a patrullar

        //Sistema de creaci�n de puntos Random
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        //Direcci�n a la que se mueve la IA
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //Detecci�n del suelo por debajo del personaje, para evitar ca�das
        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            walkPointSet = true; //Comienza el movimiento, porque existe SUELO en el DESTINO
        }
    }

    void ChaseTarget()
    {
        // Calculamos la direcci�n hacia el objetivo desde la posici�n del objeto actual
        Vector3 direction = target.position - transform.position;

        // Calculamos el Quaternion de rotaci�n para que el objeto actual mire hacia el objetivo
         transform.LookAt(new Vector3(0,direction.y,0));
         shootPoint.transform.LookAt(new Vector3(0, direction.y, 0));

        // Aplicamos la rotaci�n al objeto actual
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        // Establecemos el destino del agente para que se mueva hacia el objetivo
        agent.SetDestination(target.position);
    }

    void AttackTarget()
    {
        //Cuando comienza a atacr, no se mueve (se persigue a s� mismo)
        agent.SetDestination(transform.position);
        //La IA mira directamente al target
        transform.LookAt(target);

        if (!alreadyAttacked)
        {
            //Si no hemos atacado ya, atacamos
            //AQU� IR�A EL C�DIGO DEL ATAQUE A PERSONALIZAR
            //En este ejemplo, vamos a generar una bala que se empuja hacia el player
            
            Debug.Log("Enemigo est� atacando");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks); //Vuelve a atacar en el intervalo de tiempo indicado por la variable
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    /*void GenerateProjectile()
    {
        Rigidbody rb = Instantiate(projectile, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * shootSpeedZ, ForceMode.Impulse);
        //rb.AddForce(transform.up * shootSpeedY, ForceMode.Impulse);
    }*/

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }
}
