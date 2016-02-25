using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

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
}