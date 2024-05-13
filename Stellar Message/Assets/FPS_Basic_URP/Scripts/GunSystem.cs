using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //Necesario para trabajar con el New Input System

public class GunSyste : MonoBehaviour
{

    #region General Variables
    [Header("General References")]
    [SerializeField] Camera fpsCam; //Referencia a la c�mara desde cuyo centro se dispara (Raycast desde centro c�mara)
    [SerializeField] Transform shootPoint; //Referencia a la posici�n del objeto desde donde se dispara (Raycast desde posici�n concreta)
    [SerializeField] RaycastHit hit; //Referencia a la info de impacto de los disparos (informaci�n de impacto Raycast)
    [SerializeField] LayerMask enemyLayer; //Referencia a la Layer que puede impactar el disparo
    [SerializeField] AudioSource weaponSound; //Referencia al AudioSource del arma


    [Header("Weapon Stats")]
    public int damage; //Da�o base del arma por bala
    public float range; //Alcance de disparo (Longitud del Raycast)
    public float spread; //Dispersi�n de los disparos
    public float shootingCooldown; //Tiempo de enfriamento del arma
    public float timeBetweenShoots; //Tiempo real entre disparo y disparo (Impacto e impacto)
    public float reloadTime; //Tiempo que tardas en recargar (suele igualarse a la duraci�n de la animaci�n de recarga)
    public bool allowButtonHold; //Permite disparar por pulsaci�n (false) o manteniendo (true)


    [Header("Bullet Management")]
    public int ammoSize; //N�mero de balas por cargador
    public int bulletsPerTap; //N�mero de balas que se disparan por cada disparo �nico
    [SerializeField] int bulletsLeft; //N�mero de balas dentro del cargador ACTUAL
    [SerializeField] int bulletsShot; //N�mero de balas YA DISPARADAS dentro del cargador

    [Header("State Bools")]
    [SerializeField] bool shooting; //Verdadero cuando ESTAMOS DISPARANDO
    [SerializeField] bool canShoot; //Verdadero cuando PODEMOS DISPARAR
    [SerializeField] bool reloading; //Verdadero cuando ESTAMOS RECARGANDO

    [Header("Feedback & Graphics")]
    [SerializeField] GameObject muzzleFlash; //Objeto feedback del fogonazo
    [SerializeField] bool attackIsSounding; //Si es verdadero, el sonido de disparo ya suena, por lo que no hay que repetirlo
    //[SerializeField] GameObject hitGraphic; //Elemento feedback de impacto

    #endregion

    private void Awake()
    {
        weaponSound = GetComponent<AudioSource>();
        attackIsSounding = false;
        bulletsLeft = ammoSize;
        canShoot = true;
    }

    void Update()
    {
        Inputs();
        VisualEffects();
    }

    void Inputs()
    {
        //Lectura constante del disparo si se re�nen las condiciones
        // Si (podemos dispara + el input de disparo se lee + no estamos recargando + nos quedan balas en el cargador)
        if (canShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    void VisualEffects()
    {

    }

    void Shoot()
    {
        canShoot = false; //Estamos en ek proceso de disparo, por tanto YA NO PODEMOS DISPARAR hasta que acabe
        //Al inicio del disparo, si hay dispersi�n, se genera la randomizaci�n de dicha dispersi�n (cada disparo tiene una dispersi�n diferente)
        float spreadX = Random.Range(-spread, spread);
        float spreadY = Random.Range(-spread, spread);
        float spreadZ = Random.Range(-spread, spread);
        Vector3 direction = fpsCam.transform.forward + new Vector3(-spreadX, spreadY, spreadZ);

        //Raycast del disparo
        //Generar un Raycast: Physics.Raycast(Origen, Direcci�n, Variable Almac�n del impacto, longitud del rayo, a qu� layer golpea el rayo)
        //Si no declaramos layer en un Raycast, golpea a todo lo que tenga collider
        if (Physics.Raycast(fpsCam.transform.position, direction, out hit, range, enemyLayer))
        {
            Debug.DrawRay(fpsCam.transform.position, direction, Color.red);
            Debug.Log(hit.collider.name);
            //A PARTIR DE AQUI SE CODEAN LOS EFECTOS DEL RAYCAST. En este caso es UN DISPARO
            //EN ESTE CASO SE CODEA HACER DA�O
            if (hit.collider.CompareTag("Enemy"))
            {

                //Hacer da�o concreto
                EnemyDamage enemyScript = hit.collider.GetComponent<EnemyDamage>(); //ACCESI DIRECTO AL SCRIPT DEL ENEMIGO HITEADO
                enemyScript.TakeDamage(damage);
            }
        }

        //Instanciar o visualizar los efectos del disparo (hitGraphics)
        bulletsLeft--; //Restamos una bala al cargador actual
        bulletsShot--; //Le indicamos al ordenador que hemos disparado X cantidad de balas

        if(!IsInvoking(nameof(ResetShoot)) && !canShoot)
        {
            Invoke(nameof(ResetShoot), shootingCooldown);
        }

        //Disparar m�s balas de una
        if(bulletsShot > 0 && bulletsLeft > 0) 
        {
            Invoke(nameof(Shoot), timeBetweenShoots);
        }
    }

    void ResetShoot()
    {
        canShoot = true; //La acci�n de disparo ha acabado y por tanto (si se reunen las condiciones) podemos volver a disparars
    }

    void Reload()
    {
        reloading = true; //Entrar en estado recarga (No se pueden hacer otras opciones con el arma)
        Invoke(nameof(ReloadFinished), reloadTime); //Intentar hacer coincidir el valor de reloadTime con la duraci�n de anim de recarga.
    }

    void ReloadFinished()
    {
        bulletsLeft = ammoSize; //Balas actuales pasan a ser el m�ximo por cargador actual 
        reloading = false; //Salir del estado de recarga (Se pueden hacer otras cosas con el arma)
    }

    #region New Input Methods

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started && !allowButtonHold)
        {
            muzzleFlash.SetActive(true);
            shooting = true;
            if (!attackIsSounding)
            {
                weaponSound.Play();
                attackIsSounding = true;
            }
        }
        if (context.canceled)
        {
            shooting = false;
            attackIsSounding = false;
            muzzleFlash.SetActive(false);
            weaponSound.Stop();
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (bulletsLeft < ammoSize && !reloading)
            {
                Reload();
            }
        }
    }

    #endregion
}
