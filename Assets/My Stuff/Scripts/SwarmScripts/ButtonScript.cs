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
    }
}
