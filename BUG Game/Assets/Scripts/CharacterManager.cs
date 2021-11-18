using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public float currentHealth;
    
    [SerializeField]
    private float maxHealth = 100;
    [SerializeField]
    private GameObject explosionParticles;
    [SerializeField]
    private GameObject hitEffect;

    private Transform particleParent;
    private bool showHealthBar;

    private void Awake()
    {
        currentHealth = maxHealth;
        particleParent = GameObject.Find("ParticleParent").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Die()
    {
        if (gameObject == GameManager.instance.player)
        {
            GameManager.instance.SwapActiveCams();
        }
        if (explosionParticles)
        {
            Destroy(Instantiate(explosionParticles, transform.position, Quaternion.identity, particleParent), 3f);
        }
        Destroy(gameObject);
    }

    public void Damage(float hitPoints, Vector3 contactPoint, Vector3 normal)
    {
        currentHealth -= hitPoints;
        //print($"lost {hitPoints} health.");

        showHealthBar = true;

        if (currentHealth <= 0)
        {
            Die();
        }

        Destroy(Instantiate(hitEffect, contactPoint + normal * 0.005f, Quaternion.LookRotation(normal), particleParent), 2f);
    }
}
