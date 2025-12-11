using TMPro;
using UnityEngine;

public class StatsScreenScript : MonoBehaviour
{
    public TextMeshProUGUI WeakEnemyAmtText;
    public TextMeshProUGUI BasicEnemyAmtText;
    public TextMeshProUGUI HeavyEnemyAmtText;
    public TextMeshProUGUI HighestPlayerLevelText;
    public TextMeshProUGUI HighScoreText;

    void Start()
    {
        WeakEnemyAmtText.text = "" + PlayerPrefs.GetInt("WeakKilled",0);
        BasicEnemyAmtText.text = "" + PlayerPrefs.GetInt("BasicKilled",0);
        HeavyEnemyAmtText.text = "" + PlayerPrefs.GetInt("HeavyKilled",0);
        HighestPlayerLevelText.text = "" +  PlayerPrefs.GetInt("HighestLevel",0);
        HighScoreText.text = "" + PlayerPrefs.GetInt("HighScore",0);
    }

    public void updateStats()
    {
        WeakEnemyAmtText.text = "" + PlayerPrefs.GetInt("WeakKilled",0);
        BasicEnemyAmtText.text = "" + PlayerPrefs.GetInt("BasicKilled",0);
        HeavyEnemyAmtText.text = "" + PlayerPrefs.GetInt("HeavyKilled",0);
        HighestPlayerLevelText.text = "" +  PlayerPrefs.GetInt("HighestLevel",0);
        HighScoreText.text = "" + PlayerPrefs.GetInt("HighScore",0);
    }

}
