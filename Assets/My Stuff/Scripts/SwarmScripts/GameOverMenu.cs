using TMPro;
using UnityEngine;


public class GameOverMenu : MonoBehaviour
{

    public GameObject GameOverUI;
    public TextMeshProUGUI WeakScore;
    public TextMeshProUGUI BasicScore;
    public TextMeshProUGUI HeavyScore;
    public TextMeshProUGUI PlayerLevel;
    public TextMeshProUGUI FinalScoreAmt;
    public PlayerScript P;
    public ZombieTarget Z;


    //Const text values

    private const string WeakPrefix = "Weak Zombies Killed: ";
    private const string BasicPrefix = "Basic Zombies Killed: ";
    private const string HeavyPrefix = "Heavy Zombies Killed: ";
    private const string LevelPreix = "Player Level Reached: ";



    public void showGameOverMenu()
    {
        //freeze time, and make the cursor usable for interaction
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        //Make the text reflect ingame scores
        WeakScore.text = WeakPrefix + Z.WeakKilled;
        BasicScore.text = BasicPrefix + Z.BasicKilled;
        HeavyScore.text = HeavyPrefix + Z.HeavyKilled;
        PlayerLevel.text = LevelPreix + P.playerLevel;

        FinalScoreAmt.text = ""+ Z.WeakKilled + Z.BasicKilled + Z.HeavyKilled + P.playerLevel;

        //set the menu active
        GameOverUI.SetActive(true);

    }

    

    
}
