using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private Transform barrelTransform;
    [SerializeField]
    private GameObject bulletPrefab;
    //[SerializeField]
    //private AudioSource bulletSound;

    private Transform bulletParent;

    private void Start()
    {
        bulletParent = GameObject.FindGameObjectWithTag("BulletParent")?.transform;
    }

    public void ShootGun(Transform target)
    {
        print("Ranged attack");
        //Destroy(Instantiate(bulletSound, transform), 2f);
        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.target = target.transform.position;
        bullet.transform.LookAt(target.transform.position + Vector3.up * 1.25f);
    }
}
