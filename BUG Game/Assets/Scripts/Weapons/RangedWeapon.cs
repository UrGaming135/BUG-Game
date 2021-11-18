using System.Collections;
using UnityEngine;

public class RangedWeapon : MonoBehaviour, Weapon
{
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
    //[SerializeField]
    //private float range = 50;
    [SerializeField]
    private Transform barrel;
    [SerializeField]
    private ParticleSystem sparks;
    [SerializeField]
    private LayerMask environment;


    private Transform cameraTransform;
    private Transform particleParent;

    private Coroutine shootCoroutine;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        particleParent = GameObject.Find("ParticleParent").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, cameraArmTransform.rotation, rotationSpeed * Time.deltaTime);
    }

    public void StartAttacking()
    {
        shootCoroutine = StartCoroutine(ShootCoroutine());
    }

    public void StopAttacking()
    {
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
        }
    }

    IEnumerator ShootCoroutine()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(1 / fireRate);
        }
    }

    void Shoot()
    {
        //var debugString = "Shot at ";
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, Mathf.Infinity) && hit.collider != null)
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
        //print(debugString);
    }
}
