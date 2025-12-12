
using System.Collections;
using Cinemachine;
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
    public GameObject HighScoreAlertText;
    public PlayerScript P;




    private int finalScore;


    //Const text values

    private const string WeakPrefix = "Weak Zombies Killed: ";
    private const string BasicPrefix = "Basic Zombies Killed: ";
    private const string HeavyPrefix = "Heavy Zombies Killed: ";
    private const string LevelPreix = "Player Level Reached: ";



    public void showGameOverMenu()
    {
        
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        //Make the text reflect ingame scores
        WeakScore.text = WeakPrefix + P.WeakKilled;
        BasicScore.text = BasicPrefix + P.BasicKilled;
        HeavyScore.text = HeavyPrefix + P.HeavyKilled;
        PlayerLevel.text = LevelPreix + P.playerLevel;
        finalScore = P.WeakKilled + P.BasicKilled + P.HeavyKilled + P.playerLevel;

        FinalScoreAmt.text = ""+ finalScore;
        saveData();
        
        Debug.Log("Got Into ShowGameoverMenu()");
        //freeze time, and make the cursor usable for interaction
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        //set the menu active
        GameOverUI.SetActive(true);

    }


    public void saveData()
    {
        //obtain kill data and add it to the global totals
        PlayerPrefs.SetInt("WeakKilled", P.WeakKilled + PlayerPrefs.GetInt("WeakKilled",0));
        PlayerPrefs.SetInt("BasicKilled", P.BasicKilled + PlayerPrefs.GetInt("BasicKilled",0));
        PlayerPrefs.SetInt("HeavyKilled", P.HeavyKilled + PlayerPrefs.GetInt("BasicKilled",0));

        //check for a new highest level and or highest score
        if(P.playerLevel > PlayerPrefs.GetInt("HighestLevel",0))
        {
            PlayerPrefs.SetInt("HighestLevel", P.playerLevel);
        }

        if(finalScore > PlayerPrefs.GetInt("HighScore",0))
        {
            PlayerPrefs.SetInt("HighScore", finalScore);
            HighScoreAlertText.SetActive(true);
        }

        PlayerPrefs.Save();
        
    }

    

    

    
}
