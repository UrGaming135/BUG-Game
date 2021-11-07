using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100;

    private float currentHealth;
    private bool showHealthBar;

    private void Awake()
    {
        currentHealth = maxHealth;
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
        Destroy(gameObject);
    }

    public void Damage(float hitPoints)
    {
        currentHealth -= hitPoints;
        print($"lost {hitPoints} health.");

        showHealthBar = true;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
