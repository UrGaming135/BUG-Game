using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public float currentHealth;
    [SerializeField]
    public float maxHealth = 100;
    
    [SerializeField]
    private GameObject explosionParticles;
    [SerializeField]
    private GameObject hitEffect;
    [SerializeField]
    private Color healthWarningColor = Color.red;
    [SerializeField]
    private Color healthStandardColor = Color.white;

    private Transform particleParent;
    private bool showHealthBar;
    private TextMeshProUGUI healthValue;

    private void Awake()
    {
        currentHealth = maxHealth;
        particleParent = GameObject.Find("ParticleParent").transform;
        var healthValueObject = GameObject.Find("HeathValue");
        healthValue = healthValueObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        var currentHealthPercent = (int)(currentHealth / maxHealth * 100);
        if (gameObject.CompareTag("Player") && healthValue)
        {
            if (currentHealthPercent <= 30)
            {
                healthValue.color = healthWarningColor;
            }
            else
            {
                healthValue.color = healthStandardColor;
            }
            healthValue.text = $"{currentHealth} / {maxHealth}";
        }
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

        Destroy(Instantiate(hitEffect, contactPoint + normal * 0.01f, Quaternion.LookRotation(normal), particleParent), 2f);
    }

    public void AddHealth(float amount)
    {
        currentHealth = currentHealth + amount > maxHealth ? maxHealth : currentHealth + amount;
    }
}
