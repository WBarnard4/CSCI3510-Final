using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public void handleStartButton()
    {
        SceneManager.LoadScene("SwarmGame");
    }

    public void handleStatsButton()
    {
        SceneManager.LoadScene("SwarmStatsMenu");
    }

    public void handleBackButton()
    {
        SceneManager.LoadScene("SwarmMainMenu");
        Time.timeScale = 1f;
    }

    public void handleResetButton()
    {
        PlayerPrefs.SetInt("WeakKilled",0);
        PlayerPrefs.SetInt("BasicKilled",0);
        PlayerPrefs.SetInt("HeavyKilled",0);
        PlayerPrefs.SetInt("HighestLevel",0);
        PlayerPrefs.SetInt("HighScore",0);
    }
}
