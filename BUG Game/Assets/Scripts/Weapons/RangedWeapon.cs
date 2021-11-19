using System.Collections;
using UnityEngine;

public class RangedWeapon : MonoBehaviour, Weapon
{
    [SerializeField]
    public int remainingAmmo;
    [SerializeField]
    public int ammoInMag;
    [SerializeField]
    public bool isReloading;
    [SerializeField]
    public int maxAmmo;

    [SerializeField]
    private Transform cameraArmTransform;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private float damage = 10;
    //[SerializeField]
    //private float hitForce = 10;
    [SerializeField]
    private float range = 50;
    [SerializeField]
    private Transform barrel;
    [SerializeField]
    private ParticleSystem sparks;
    [SerializeField]
    private LayerMask environment;
    [SerializeField]
    private int magSize;
    [SerializeField]
    private float reloadDelay = 2f;

    private Transform cameraTransform;
    private Transform particleParent;

    private Coroutine shootCoroutine;
    private float nextFireTime;

    private void Awake()
    {
        remainingAmmo = maxAmmo;
        ammoInMag = magSize;
        cameraTransform = Camera.main.transform;
        particleParent = GameObject.Find("ParticleParent").transform;
        nextFireTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, cameraArmTransform.rotation, rotationSpeed * Time.deltaTime);

        if (isReloading && Time.time >= nextFireTime)
        {
            isReloading = false;
        }
    }

    IEnumerator ShootCoroutine()
    {
        while (true)
        {
            var nextFireDelay = 1 / fireRate;
            if (Time.time >= nextFireTime)
            {
                nextFireDelay = Shoot();
            }
            yield return new WaitForSeconds(nextFireDelay);
        }
    }

    float Shoot()
    {
        //var debugString = "Shot at ";
        //Debug.DrawRay(cameraTransform.position, cameraTransform.forward);
        var nextFireDelay = 1 / fireRate;
        if (ammoInMag > 0)
        {
            ammoInMag--;
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, range) && hit.collider != null)
            {
                var characterManager = hit.collider.gameObject.GetComponent<CharacterManager>();
                if (hit.collider.gameObject != this)
                {
                    if (characterManager)
                    {
                        characterManager.Damage(damage, hit.point, hit.normal);
                    
                    } else if (1 << hit.collider.gameObject.layer == environment.value)
                    {
                        Destroy(Instantiate(sparks, hit.point + hit.normal * 0.005f, Quaternion.LookRotation(hit.normal), particleParent), sparks.main.duration);
                    }
                }
            }
        }
        else
        {
            if (remainingAmmo > 0)
            {
                Reload();
                nextFireTime = Time.time + reloadDelay;
                nextFireDelay = reloadDelay;
            }
        }

        return nextFireDelay;
    }

    void Reload()
    {
        isReloading = true;
        ammoInMag += remainingAmmo > magSize ? magSize : remainingAmmo - ammoInMag;
        remainingAmmo -= ammoInMag;
    }

    public void StartAttacking()
    {
        //if (Time.time >= nextFireTime)
        //{
        //    shootCoroutine = StartCoroutine(ShootCoroutine());
        //}
        shootCoroutine = StartCoroutine(ShootCoroutine());
    }

    public void StopAttacking()
    {
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
        }
    }

    public void AddAmmo(int amount)
    {
        remainingAmmo = remainingAmmo + amount > maxAmmo ? maxAmmo : remainingAmmo + amount;
        if (remainingAmmo == amount && ammoInMag == 0)
        {
            Reload();
        }
    }
}
 