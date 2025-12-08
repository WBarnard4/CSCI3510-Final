using System.ComponentModel;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //[SerializeField] //commented for now, need public access to health and exp 
    public float health = 100.0f;
    //[SerializeField]
    public float experience = 0.0f;
    private bool isAlive = true;

    [HideInInspector]
    public float LvlThreshold = 250.0f;
    public int playerLevel = 0;

    public DamageFlashEffect damageFlash; // Reference to the DamageFlashEffect script

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float damage)
    {
        if (!isAlive) return;

        health -= damage;
        if (health <= 0)
        {
            health = 0;
            isAlive = false;
            Debug.Log("Player has died.");
        }
        Debug.Log("Player took " + damage + " damage. Current health: " + health);
        if (damageFlash != null)
        {
            damageFlash.TriggerFlash();
        }
    }


    public void gainExperience(float exp)
    {
        if (!isAlive) return;

        experience += exp;
        Debug.Log("Gained " + exp + " experience. Total experience: " + experience);
        if(experience >= LvlThreshold)
        {
            LevelUp();
        }
    }

    public void heal(float amount)
    {
        if (!isAlive) return;

        health += amount;
        if (health > 100.0f)
        {
            health = 100.0f;
        }
        Debug.Log("Healed " + amount + " health. Current health: " + health);
    }

    public bool getAlive()
    {
        return isAlive;
    }

    public void LevelUp()
    {
        experience = 0;
        LvlThreshold += 200;
        playerLevel += 1;
    }

    
}
