using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHudScript : MonoBehaviour
{

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI levelText;
    public GameObject player;

    private const string healthPrefix = "Health: ";
    private const string expPrefix = "Experience: ";
    private const string levelPrefix = "Level: ";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        PlayerScript playerInfo = player.GetComponent<PlayerScript>();
        float healthAmt = playerInfo.health;
        int playerLevel = playerInfo.playerLevel;
        float expAmount = playerInfo.experience;
        float expThresh = playerInfo.LvlThreshold;
        //anything that needs to be reset on scene entry
        healthText.text = healthPrefix + healthAmt;
        expText.text = expPrefix + expAmount + "/" + expThresh;
        levelText.text = levelPrefix + playerLevel;
    }

    // Update is called once per frame
    void UpdateHud()
    {
        //Call this function whenever the player either gets hit or gains experience
    }
}
