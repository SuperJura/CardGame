using UnityEngine;
using System.Collections;
using System.Reflection.Emit;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    public static GameMenuManager instance;

    public GameObject currentMenu;

    [HideInInspector]
    public GameObject Gameboard;
    [HideInInspector]
    public GameObject PauseMenu;
    [HideInInspector]
    public GameObject EndGameMenu;

    //--- END GAME MENU VARIJABLE ---
    private const string pathToEndGameOriginalMessagesSource = "Gameboard/MainPanel/NotificationPanel/MessagePanel/Messages";
    private const string pathToEndGameMatchHistory = "EndGameMenu/MainPanel/Panel/MatchHistory/Messages";
    private const string pathToEndGameTitle = "EndGameMenu/MainPanel/Title";
    private const string pathToEndGameWinner = "EndGameMenu/MainPanel/Panel/Winner/Text";
    private const string pathToEndGameLoser = "EndGameMenu/MainPanel/Panel/Loser/Text";

    private Transform endGameOriginalMessagesSource;
    private Transform endGameMatchHistory;
    private Text endGameTitle;
    private Text endGameWinner;
    private Text endGameLoser;

    void Awake()
    {
        instance = this;
    }

    void Start ()
    {
        StartCoroutine(InitializeMenus());

        Gameboard = transform.Find("Gameboard").gameObject;
        PauseMenu = transform.Find("PauseMenu").gameObject;
        EndGameMenu = transform.Find("EndGameMenu").gameObject;

        endGameOriginalMessagesSource = transform.Find(pathToEndGameOriginalMessagesSource);
        endGameMatchHistory = transform.Find(pathToEndGameMatchHistory);
        endGameTitle = transform.Find(pathToEndGameTitle).GetComponent<Text>();
        endGameWinner = transform.Find(pathToEndGameWinner).GetComponent<Text>();
        endGameLoser = transform.Find(pathToEndGameLoser).GetComponent<Text>();
    }

    //--- API ---
    public void LoadMenu(GameObject menu)
    {
        if (currentMenu != null)
        {
            CloseCurrentMenu();
        }
        currentMenu = menu;
        OpenCurrentMenu();
    }

    //--- POMOCNE METODE ---
    private IEnumerator InitializeMenus()
    {
        foreach (Transform menuChild in transform)
        {
            if (menuChild.name != "Background")
            {
                Animation initializeAnimation = menuChild.GetComponent<Animation>();
                initializeAnimation.Play();
            }
        } //postavi sve menue u pocetnu poziciju

        yield return new WaitForSeconds(0.1f); //pricekaj da se sve animacije zavrse
        OpenCurrentMenu(); //otvori prvi menu
    }

    private void OpenCurrentMenu()
    {
        CanvasGroup canvasGroup = currentMenu.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void CloseCurrentMenu()
    {
        CanvasGroup canvasGroup = currentMenu.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    //--- GAMEBOARD ---
    public void Gameboard_PauseBtn()
    {
        LoadMenu(PauseMenu);
    }

    //--- PAUSE MENU ---
    public void PauseMenu_ReturnToGameBtn()
    {
        LoadMenu(Gameboard);
    }

    public void PauseMenu_ExitToMainMenuBtn()
    {
        GamesManager.LoadMainMenu();
    }

    public void PauseMenu_ExitGameBtn()
    {
        GamesManager.ExitApplication();
    }

    //--- END GAME MENU ---
    public void EndGameMenu_ExitToMainMenuBtn()
    {
        GamesManager.LoadMainMenu();
    }

    public void EndGameMenu_RematchBtn()
    {
        GamesManager.LoadBotGame();
    }

    public void EndGame(BasePlayer winner, BasePlayer loser)
    {
        Debug.Log(winner.playerName + " won the game");
        EndGameMenu_DisplayDetails(winner.playerName, loser.playerName);
        EndGameMenu_SetMatchHistory();
        LoadMenu(EndGameMenu);
    }

    public void EndGame(char winner, char loser)
    {
        BasePlayer aPlayer = TurnsManager.instance.aPlayer;
        BasePlayer bPlayer = TurnsManager.instance.bPlayer;

        if (char.ToLower(winner) == 'a')
        {
            EndGame(aPlayer, bPlayer);
        }
        else
        {
            EndGame(bPlayer, aPlayer);
        }
        if (GamesManager.GetGameMode() == Enumerations.GameModes.Online) Destroy(TurnsManager.instance);
    }

    private void EndGameMenu_SetMatchHistory()
    {
        while (endGameOriginalMessagesSource.childCount != 0)
        {
            endGameOriginalMessagesSource.GetChild(endGameOriginalMessagesSource.childCount - 1).SetParent(endGameMatchHistory);
            //poruke idu obrnutim redosljedom
            endGameMatchHistory.GetChild(endGameMatchHistory.childCount - 1).localScale = new Vector3(1, 1, 1);
        }
    }

    private void EndGameMenu_DisplayDetails(string winner, string loser)
    {
        endGameTitle.text = winner + " won the game!";
        endGameWinner.text = winner;
        endGameLoser.text = loser;
    }
}
