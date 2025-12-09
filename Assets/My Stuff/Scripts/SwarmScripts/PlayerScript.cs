using System.ComponentModel;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //[SerializeField] //commented for now, need public access to health and exp 
    public float maxHealth = 100.0f;
    public float maxSpeed = 10;
    public float damageModifer = 0;
    //[SerializeField]
    public float experience = 0.0f;
    public PlayerHudScript hud;
    private bool isAlive = true;
    public float LvlThreshold = 50.0f;
    public LevelUpMenu menu;
    
    [HideInInspector]
    public int playerLevel = 0;

    public DamageFlashEffect damageFlash; // Reference to the DamageFlashEffect script

    [HideInInspector]
    public float health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
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
        hud.UpdateHud();
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
        hud.UpdateHud();
    }

    public void heal(float amount)
    {
        if (!isAlive) return;

        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        Debug.Log("Healed " + amount + " health. Current health: " + health);
        hud.UpdateHud();
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
        menu.showLevelUpMenu();
    }

    
}
