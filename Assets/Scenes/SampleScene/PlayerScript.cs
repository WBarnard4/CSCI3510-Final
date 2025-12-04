using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private float health = 100.0f;
    [SerializeField]
    private float experience = 0.0f;
    private bool isAlive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void takeDamage(float damage)
    {
        if (!isAlive) return;

        health -= damage;
        if (health <= 0)
        {
            health = 0;
            isAlive = false;
            Debug.Log("Player has died.");
        }
    }


    void gainExperience(float exp)
    {
        if (!isAlive) return;

        experience += exp;
        Debug.Log("Gained " + exp + " experience. Total experience: " + experience);
    }

    void heal(float amount)
    {
        if (!isAlive) return;

        health += amount;
        if (health > 100.0f)
        {
            health = 100.0f;
        }
        Debug.Log("Healed " + amount + " health. Current health: " + health);
    }

    bool getAlive()
    {
        return isAlive;
    }
}
