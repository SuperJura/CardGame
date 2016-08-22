using UnityEngine;
using UnityEngine.SceneManagement;

public static class GamesManager
{
    public static void LoadCoopGame()
    {
        Deck.CheckCards();
        SceneManager.LoadScene("CoopGame");
    }

    public static void LoadBotGame()
    {
        SceneManager.LoadScene("BotGame");
    }

    public static void LoadOnlineGame()
    {
        Deck.CheckCards();
        SceneManager.LoadScene("OnlineGame");
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static string GetApplicationPath()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)    return Application.persistentDataPath;
        return Application.dataPath;
    }

    public static Enumerations.GameModes GetGameMode()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Bot")) return Enumerations.GameModes.Bot;
        if (sceneName.StartsWith("Coop")) return Enumerations.GameModes.Coop;
        if (sceneName.StartsWith("Online")) return Enumerations.GameModes.Online;
        return Enumerations.GameModes.MainMenu;
    }

    public static void ExitApplication()
    {
        Application.Quit();
    }
}