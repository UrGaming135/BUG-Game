using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public float currentHealth;
    
    [SerializeField]
    private float maxHealth = 100;

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
        if (gameObject == GameManager.instance.player)
        {
            GameManager.instance.SwapActiveCams();
        }
        Destroy(gameObject);
    }

    public void Damage(float hitPoints)
    {
        currentHealth -= hitPoints;
        //print($"lost {hitPoints} health.");

        showHealthBar = true;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
