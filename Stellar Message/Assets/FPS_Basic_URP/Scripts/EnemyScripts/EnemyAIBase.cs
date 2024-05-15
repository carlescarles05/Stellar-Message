
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //Libreria para referenciar clases de NavMesh

public class EnemyAIBase : MonoBehaviour
{
    [Header("AI Configuration")]
    [SerializeField] NavMeshAgent agent; //Ref al componente Agente, que permite que el objeto tenga IA
    [SerializeField] Transform target; //Ref al transform del objeto que la IA va a perseguir
    [SerializeField] LayerMask targetLayer; //Determina cual es la capa de detección del target
    [SerializeField] LayerMask groundLayer; //Determina cual es la capa de detección del suelo

    [Header("Patroling Stats")]
    [SerializeField] Vector3 walkPoint; //Dirección a la que la IA se va a mover si no detecta target
    [SerializeField] float walkPointRange; //Rango máximo de dirección de movimiento si la IA no detecta target
    bool walkPointSet; //Bool que determina si la IA ha llegado al objetivo y entonces cambia de objetivo

    [Header("Attack Configuration")]
    public float timeBetweenAttacks; //Tiempo de espera entre ataque y ataque
    bool alreadyAttacked; //Bool para determinar si se ha atacado
    //Variables necesarias si el ataque es un disparo FÍSICO
    [SerializeField] GameObject projectile; //Ref al prefab del proyectil
    [SerializeField] Transform shootPoint; //Ref a la posición desde donde se disparan los proyectiles
    [SerializeField] float shootSpeedZ; //Velocidad de disparo hacia adelante
    [SerializeField] float shootSpeedY; //Velocidad de disparo hacia arriba (solo si el disparo es catapulta/bolea)

    [Header("States & Detection")]
    [SerializeField] float sightRange; //Rango de detección de persecución de la IA
    [SerializeField] float attackRange; //Rango a partir del cual la IA ataca
    [SerializeField] bool targetInSightRange; //Bool que determina si el target está a distancia de detección
    [SerializeField] bool targetInAttackRange; //Bool que determina si el target está a distancia de ataque
    public bool boost;
    public Animator anim;
    public float rotation;


    private void Awake()
    {
        target = GameObject.Find("Player").transform; //Al inicio referencia el transform del Player, para poder perseguirlo cuando toca
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();   
    }


    void Update()
    {
        // Chequear si el target está en los rangos de detección y de ataque
        targetInSightRange = Physics.CheckSphere(transform.position, sightRange, targetLayer);
        targetInAttackRange = Physics.CheckSphere(transform.position, attackRange, targetLayer);



        // Cambios dinámicos de estado de la IA
        // Si detecta el target y está fuera del rango de ataque: PERSIGUE
        if (targetInSightRange && !targetInAttackRange)
        {
            ChaseTarget(); 
            anim.SetBool("Run", true);
        }
        // Si detecta el target y está en rango de ataque: ATACA
        else if (targetInSightRange && targetInAttackRange) AttackTarget();
        // Si no detecta el target: PATRULLA
        

    }

    void Patroling()
    {
        if (!walkPointSet)
        {
            //Si no existe punto al que dirigirse, inicia el método de crearlo
            SearchWalkPoint();
        }
        else
        {
            //Si existe punto, el personaje mueve la IA hacia ese punto
            agent.SetDestination(walkPoint);
        }

        //Sistema para que la IA busque un nuevo  destino de patrullaje una vez ha llegado al destino actual
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1) { walkPointSet = false; }
    }

    void SearchWalkPoint()
    {
        //Crear el sistema de puntos "random" a patrullar

        //Sistema de creación de puntos Random
        float RandomZ = Random.Range(-walkPointRange, walkPointRange);
        float RandomX = Random.Range(-walkPointRange, walkPointRange);

        //Dirección a la que se mueve la IA
        walkPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z + RandomZ);

        //Detección del suelo por debajo del personaje, para evitar caídas
        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            walkPointSet = true; //Comienza el movimiento , porque existe SUELO en el DESTINO
        }
    }

    void ChaseTarget()
    {
        // Calculamos la dirección hacia el objetivo desde la posición del objeto actual
        Vector3 direccion = target.position - transform.position;

        // Calculamos el Quaternion de rotación para que el objeto actual mire hacia el objetivo
        Quaternion rotacion = Quaternion.LookRotation(direccion);

        // Aplicamos la rotación al objeto actual solo en el eje Y
        //transform.rotation = Quaternion.Euler(0, rotacion.eulerAngles.y, 0);
        float anguloY = rotacion.eulerAngles.y;

        // Invertimos el ángulo de rotación en el eje Y
        float anguloYInvertido = anguloY > rotation ? anguloY - rotation : anguloY + rotation;

        // Aplicamos la rotación al objeto actual solo en el eje Y, invertida
        transform.rotation = Quaternion.Euler(0, anguloYInvertido, 0);
        // Establece el destino del agente para que se mueva hacia el objetivo
        agent.SetDestination(target.position);

    }

    void AttackTarget()
    {
        //Cuando comienza a atacar, no se mueve (se persigue a sí mismo)
        agent.SetDestination(transform.position);
        //La IA mira directamente al target
        LookAtTarget();

        if (!alreadyAttacked)
        {
            //Si no hemos atacado ya, atacamos
            //AQUÍ IRÍA EL CÓDIGO DEL ATAQUE A PERSONALIZAR
            //En este ejemplo, vamos a generar una bala que se empuja hacia el player
            GeneratedProjectile();
            Debug.Log("Enemigo está atacando");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks); //Vuelve a atacar en el intervalo de tiempo indicado por la variable
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void GeneratedProjectile()
    {
        Rigidbody rb = Instantiate(projectile, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * shootSpeedZ, ForceMode.Impulse);
        //rb.AddForce(transform.up * shootSpeedY, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    void LookAtTarget()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed);
    }
}
