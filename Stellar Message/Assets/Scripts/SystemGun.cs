using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SystemGun : MonoBehaviour
{
    /*[Header("General References")]
    [SerializeField] Camera fpsCam;
    [SerializeField] Transform shootPoint;
    [SerializeField] RaycastHit hit;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] AudioSource weaponSound;
    [SerializeField] GameObject muzzleFlash;


    [Header("Weapon Stats")]
    public int damage;
    public float range;
    public float spread;
    public float shootingCooldown;
    public float timeBetweenShoots;
    public bool allowButtonHold;

    [Header("Bullet Management")]
    public int ammoSize;
    public int bulletsPerTap;
    [SerializeField] int bulletsLeft;
    [SerializeField] int bulletsShot;

    [Header("State Bools")]
    [SerializeField] bool shooting;
    [SerializeField] bool canShoot;
    public bool haveGUN;

    [Header("Feedback & Graphics")]
    [SerializeField] bool attackIsSounding;

    private void Awake()
    {
        weaponSound = GetComponent<AudioSource>();
        attackIsSounding = false;
        bulletsLeft = ammoSize;
        haveGUN = false;
        
    }

    void Update()
    {
        if (haveGUN) { Inputs(); }
        VisualEffects();
    }

    void Inputs()
    {
        if (canShoot && shooting && bulletsLeft > 0)
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
        canShoot = false;

        float spreadX = Random.Range(-spread, spread);
        float spreadY = Random.Range(-spread, spread);
        float spreadZ = Random.Range(-spread, spread);
        Vector3 direction = fpsCam.transform.forward + new Vector3(-spreadX, spreadY, spreadZ);

        if (Physics.Raycast(fpsCam.transform.position, direction, out hit, range, enemyLayer))
        {
            Debug.DrawRay(fpsCam.transform.position, direction, Color.red);
            Debug.Log(hit.collider.name);

            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Play");
                EnemyDamage enemyScript = hit.collider.GetComponent<EnemyDamage>();
                enemyScript.TakeDamage(damage);
            }
        }

        bulletsLeft--;
        bulletsShot--;

        if (!IsInvoking(nameof(ResetShoot)) && !canShoot)
        {
            Invoke(nameof(ResetShoot), shootingCooldown);
        }

        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke(nameof(Shoot), timeBetweenShoots);
        }
    }

    void ResetShoot()
    {
        canShoot = true;
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

    #endregion*/
    [Header("General References")]
    [SerializeField] Camera fpsCam;
    [SerializeField] Transform shootPoint;
    [SerializeField] RaycastHit hit;
    [SerializeField] LayerMask enemyLayer = -1; // Usar -1 como un valor que representa todas las capas
    [SerializeField] AudioSource weaponSound;
    [SerializeField] GameObject muzzleFlash;
    ControllerFPS controllerFPS;

    [Header("Weapon Stats")]
    public int damage;
    public float range;
    public float spread;
    public float shootingCooldown;
    public float timeBetweenShoots;
    public bool allowButtonHold;

    [Header("Bullet Management")]
    public int ammoSize;
    public int bulletsPerTap;
    [SerializeField] int bulletsLeft;
    [SerializeField] int bulletsShot;

    [Header("State Bools")]
    [SerializeField] bool shooting;
    [SerializeField] bool canShoot;
    public bool haveGUN;
    

    [Header("Feedback & Graphics")]
    [SerializeField] bool attackIsSounding;

    private void Awake()
    {
        weaponSound = GetComponent<AudioSource>();
        controllerFPS = GetComponent<ControllerFPS>();
        attackIsSounding = false;
        bulletsLeft = ammoSize;
        haveGUN = false;
        canShoot = true;
    }

    void ReleaseWeapon()
    {
        // Reinicia todas las referencias y estados relacionados con el arma
        haveGUN = false;
        // Aquí puedes reiniciar cualquier otro estado relacionado con el arma, como la munición restante, etc.
    }
    void Update()
    {
        if (haveGUN) { Inputs();
        }
        VisualEffects();
    }

    void Inputs()
    {
        if (canShoot && shooting && bulletsLeft > 0)
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
        canShoot = false;
        Debug.Log("kk");

        float spreadX = Random.Range(-spread, spread);
        float spreadY = Random.Range(-spread, spread);
        float spreadZ = Random.Range(-spread, spread);
        Vector3 direction = fpsCam.transform.forward + new Vector3(-spreadX, spreadY, spreadZ);

        if (Physics.Raycast(fpsCam.transform.position, direction, out hit, range, enemyLayer))
        {
            Debug.DrawRay(fpsCam.transform.position, direction, Color.red);
            Debug.Log(hit.collider.name);

            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyDamage enemyScript = hit.collider.GetComponent<EnemyDamage>(); //ACCES0 DIRECTO AL SCRIPT DEL ENEMIGO HITEADO
                enemyScript.TakeDamage(damage);
            }
        }

        bulletsLeft--;
        bulletsShot--;

        if (!IsInvoking(nameof(ResetShoot)) && !canShoot)
        {
            Invoke(nameof(ResetShoot), shootingCooldown);
        }

        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke(nameof(Shoot), timeBetweenShoots);
        }
    }

    void ResetShoot()
    {
        canShoot = true;
    }

    #region New Input Methods

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started && !allowButtonHold && haveGUN)
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

    #endregion
}
