using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class RangedWeapon : Weapon
{
    [SerializeField]
    private Transform cameraArmTransform;
    [SerializeField]
    private float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, cameraArmTransform.rotation, rotationSpeed * Time.deltaTime);
    }
}
