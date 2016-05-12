using UnityEngine;
using UnityEngine.SceneManagement;

public class GamesManager : MonoBehaviour
{
    public void LoadCoopGame()
    {
        Deck.CheckCards();
        SceneManager.LoadScene("CoopGame");
    }

    public void LoadBotGame()
    {
        SceneManager.LoadScene("BotGame");
    }

    public void LoadOnlineGame()
    {
        Deck.CheckCards();
        SceneManager.LoadScene("OnlineGame");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}