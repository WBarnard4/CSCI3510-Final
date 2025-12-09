using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
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
        UpdateHud();
    }
    public void UpdateHud()
    {
        //Call this function whenever the player either gets hit or gains experience
        PlayerScript p = player.GetComponent<PlayerScript>();
        healthText.text = healthPrefix + p.health;
        expText.text = expPrefix + p.experience + "/" + p.LvlThreshold;
        levelText.text = levelPrefix + p.playerLevel;
    }
}
