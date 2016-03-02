using UnityEngine;
using UnityEngine.SceneManagement;

public class GamesManager : MonoBehaviour {

    public void LoadCoopGame()
    {
        SceneManager.LoadScene("CoopGame");
    }

    public void LoadBotGame()
    {
        SceneManager.LoadScene("BotGame");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadOnlineGame()
    {
        SceneManager.LoadScene("OnlineGame");
    }
}