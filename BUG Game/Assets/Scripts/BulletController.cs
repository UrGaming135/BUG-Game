using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletDecal;
    [SerializeField]
    private float timeToDestroyBulletDecal = 10f;
    [SerializeField]
    private LayerMask environmentMask;
    [SerializeField]
    private LayerMask enemyMask;
    [SerializeField]
    private ParticleSystem sparks;

    public float damage = 25f;
    public float speed = 50f;
    public float timeToDestroy = 3f;

    private Transform decalParent;
    private Transform sparkParent;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    private void OnEnable()
    {
        decalParent = GameObject.FindGameObjectWithTag("DecalParent")?.transform;
        sparkParent = GameObject.FindGameObjectWithTag("SparkParent")?.transform;
        Destroy(gameObject, timeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        Debug.DrawLine(transform.position, target, Color.green);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var collisionObject = collision.gameObject;
        if (1 << collisionObject.layer == environmentMask.value)
        {
            ContactPoint contact = collision.GetContact(0);
            var decal = GameObject.Instantiate(bulletDecal, contact.point + contact.normal * 0.005f, Quaternion.LookRotation(contact.normal), decalParent);
            var sparkEffect = GameObject.Instantiate(sparks, contact.point + contact.normal * 0.005f, Quaternion.LookRotation(contact.normal), sparkParent);
            Destroy(sparkEffect, sparks.main.duration);
            Destroy(decal, timeToDestroyBulletDecal);
        }
        else
        {
            Destroy(gameObject);
            var isPlayer = collisionObject.tag == "Player";

            if (isPlayer)
            {
                print("hit player");
                var manager = collisionObject.GetComponentInParent<CharacterManager>();
                var damageDone = damage;
                manager.Damage(damageDone);
            }
        }
    }
}
