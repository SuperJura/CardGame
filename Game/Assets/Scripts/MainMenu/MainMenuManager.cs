using System.Collections;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    public GameObject currentMenu;

    [HideInInspector]
    public GameObject MainMenu;
    [HideInInspector]
    public GameObject OnlineGameMenu;
    [HideInInspector]
    public GameObject BotGameMenu;
    [HideInInspector]
    public GameObject CoopGameMenu;
    [HideInInspector]
    public GameObject ChangeDeckMenu;
    [HideInInspector]
    public GameObject CurrentDeckMenu;
    [HideInInspector]
    public GameObject SavedDeckMenu;

    //--- MAIN MENU VARIJABLE ---
    private const string pathToMainMenuDeckType = "MainMenu/MainPanel/DeckPanel/DeckType";

    private Text mainMenuDeckType;

    //--- BOT MENU VARIABLE ---
    private const string pathToBotNickname = "BotGameMenu/MainPanel/Nickname/Text";

    private Text botNickname;

    //--- COOP MENU VARIJABLE ---
    private const string pathToCoopNicknameA = "CoopGameMenu/MainPanel/NicknameA/Text";
    private const string pathToCoopNicknameB = "CoopGameMenu/MainPanel/NicknameB/Text";

    private Text coopNicknameA;
    private Text coopNicknameB;

    //--- ONLINE MENU VARIJABLE ---
    private const string pathToOnlineBtnStartGame = "OnlineGameMenu/MainPanel/StartOnlineGame";
    private const string pathToOnlineBtnConnectToServer = "OnlineGameMenu/MainPanel/ConnectToServer";
    private const string pathToOnlinePlayerList = "OnlineGameMenu/MainPanel/CurrentPlayers/ListOfPlayers";
    private const string pathToOnlineErrorText = "OnlineGameMenu/MainPanel/ErrorText";
    private const string pathToOnlineWaitingBar = "OnlineGameMenu/MainPanel/WaitingBar";
    private const string pathToOnlineOnlineNickname = "OnlineGameMenu/MainPanel/Nickname/Text";
    private const string pathToOnlineIPAddress = "OnlineGameMenu/MainPanel/ServerIP/Text";

    private Button      onlineStartGameBtn;
    private Button      onlineConnectToServerBtn;
    private Transform   onlineListOfPlayers;
    private Text        onlineErrorText;
    private GameObject  onlineWaitingBar;
    private Text        onlineNickname;
    private Text        onlineIPAddress;

    //--- CURRENT DECK MENU VARIJABLE ---
    private const string pathToCurrentDeckCardList = "CurrentDeckMenu/MainPanel/CardList/Cards";
    private const string pathToCurrentDeckName = "CurrentDeckMenu/MainPanel/DeckName";
    private const string pathtToCurrentDeckError = "CurrentDeckMenu/MainPanel/ErrorText";

    private Transform currentDeckCardList;
    private InputField currentDeckName;
    private Text currentDeckError;

    //--- SAVED DECK MENU VARIJABLE ---
    private const string pathToSavedDeckDeckList = "SavedDeckMenu/MainPanel/DeckList/Decks";

    private Transform savedDeckDeckList;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(InitializeMenus());

        MainMenu = transform.Find("MainMenu").gameObject;
        OnlineGameMenu = transform.Find("OnlineGameMenu").gameObject;
        BotGameMenu = transform.Find("BotGameMenu").gameObject;
        CoopGameMenu = transform.Find("CoopGameMenu").gameObject;
        ChangeDeckMenu = transform.Find("ChangeDeckMenu").gameObject;
        CurrentDeckMenu = transform.Find("CurrentDeckMenu").gameObject;
        SavedDeckMenu = transform.Find("SavedDeckMenu").gameObject;

        mainMenuDeckType = transform.Find(pathToMainMenuDeckType).GetComponent<Text>();
        mainMenuDeckType.text = Enumerations.DeckEnums.Random.ToString();

        botNickname = transform.Find(pathToBotNickname).GetComponent<Text>();

        coopNicknameA = transform.Find(pathToCoopNicknameA).GetComponent<Text>();
        coopNicknameB = transform.Find(pathToCoopNicknameB).GetComponent<Text>();

        onlineStartGameBtn = transform.Find(pathToOnlineBtnStartGame).GetComponent<Button>();
        onlineConnectToServerBtn = transform.Find(pathToOnlineBtnConnectToServer).GetComponent<Button>();
        onlineListOfPlayers = transform.Find(pathToOnlinePlayerList);
        onlineErrorText = transform.Find(pathToOnlineErrorText).GetComponent<Text>();
        onlineWaitingBar = transform.Find(pathToOnlineWaitingBar).gameObject;
        onlineNickname = transform.Find(pathToOnlineOnlineNickname).GetComponent<Text>();
        onlineIPAddress = transform.Find(pathToOnlineIPAddress).GetComponent<Text>();

        currentDeckCardList = transform.Find(pathToCurrentDeckCardList);
        currentDeckName = transform.Find(pathToCurrentDeckName).GetComponent<InputField>();
        currentDeckError = transform.Find(pathtToCurrentDeckError).GetComponent<Text>();

        savedDeckDeckList = transform.Find(pathToSavedDeckDeckList);
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

    //--- MAIN MENU ----
    public void MainMenu_CoopGameBtn()
    {
        LoadMenu(CoopGameMenu);
    }

    public void MainMenu_BotGameBtn()
    {
        LoadMenu(BotGameMenu);
    }

    public void MainMenu_OnlineGameBtn()
    {
        LoadMenu(OnlineGameMenu);
    }

    public void MainMenu_DeckBtn()
    {
        LoadMenu(ChangeDeckMenu);
    }

    public void MainMenu_ExitBtn()
    {
        GamesManager.ExitApplication();
    }

    public void MainMenu_ReturnToBtn()
    {
        LoadMenu(MainMenu);
    }

    //--- BOT MENU ---
    public void BotMenu_StartGameBtn()
    {
        string nickname = botNickname.text;
        if (!string.IsNullOrEmpty(nickname)) PlayerNamesForGame.NicknameForBotGame = nickname;

        GamesManager.LoadBotGame();
    }

    //--- COOP MENU ---
    public void CoopMenu_StartGameBtn()
    {
        string nicknameA = coopNicknameA.text;
        string nicknameB = coopNicknameB.text;

        if (!string.IsNullOrEmpty(nicknameA)) PlayerNamesForGame.NicknameForCoopGameA = nicknameA;
        if (!string.IsNullOrEmpty(nicknameB)) PlayerNamesForGame.NicknameForCoopGameB = nicknameB;

        GamesManager.LoadCoopGame();
    }

    //--- ONLINE MENU ---
    public void OnlineMenu_ConnectToServerBtn()
    {
        string nick = onlineNickname.text;
        string ipAddress = onlineIPAddress.text;
        ServerLobbyBehavior.instance.ConnectToServer(nick, ipAddress);
    }

    public void OnlineMenu_StartOnlineGameBtn()
    {
        ServerLobbyBehavior.instance.SendWantToPlay();
        OnlineMenu_StartWaiting();
    }

    public void OnlineMenu_StartWaiting()
    {
        onlineWaitingBar.SetActive(true);
        onlineStartGameBtn.enabled = false;
        onlineStartGameBtn.GetComponentInChildren<Text>().text = "Waiting for opponent";
    }

    public void OnlineMenu_DisplayPlayerList(string[] playerList)
    {
        foreach (Transform child in onlineListOfPlayers)
        {
            Destroy(child.gameObject);
        }

        foreach (string nick in playerList)
        {
            GameObject go = (GameObject)Resources.Load("MainMenuResources/PlayerListItem");
            RectTransform prefab = (RectTransform)Instantiate(go.transform);
            prefab.GetComponent<Text>().text = nick;

            prefab.SetParent(onlineListOfPlayers);
            prefab.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void OnlineMenu_DisplayPlayerJoined(string nick)
    {
        onlineStartGameBtn.interactable = true;
        onlineConnectToServerBtn.interactable = false;
        onlineNickname.enabled = false;
    }

    public void OnlineMenu_DisplayError(int errorCode)
    {
        switch (errorCode)
        {
            case 1:
                StartCoroutine(OnlineMenu_DisplayError("Name is taken"));
                break;
            case 2:
                StartCoroutine(OnlineMenu_DisplayError("Name cannot be empty"));
                break;
            case 3:
                StartCoroutine(OnlineMenu_DisplayError("Name cannot contain ;"));
                break;
            case 4:
                StartCoroutine(OnlineMenu_DisplayError("Error while connecting"));
                break;
        }
    }

    private IEnumerator OnlineMenu_DisplayError(string error)
    {
        onlineErrorText.text = error;
        onlineErrorText.enabled = true;
        onlineConnectToServerBtn.interactable = true;
        onlineStartGameBtn.interactable = false;
        onlineNickname.enabled = true;
        yield return new WaitForSeconds(5);
        onlineErrorText.enabled = false;
    }

    public void OnlineMenu_StartOnlineGame(string opponent)
    {
        string nick = onlineNickname.text;
        PlayerNamesForGame.NicknameForOnlineGame = nick;
        PlayerNamesForGame.OpponentForOnlineGame = opponent;
        GamesManager.LoadOnlineGame();
    }

    //--- CHANGE DECK MENU ---
    public void ChangeDeckMenu_RandomBtn()
    {
        Deck.DeckType = Enumerations.DeckEnums.Random;
        Deck.DeckName = Deck.DeckType.ToString();
        mainMenuDeckType.text = Deck.DeckName;
        Deck.Cards = Repository.GetCardDatabaseInstance().GetRandomDeck();

        LoadMenu(MainMenu);
    }

    public void ChangeDeckMenu_CustomBtn()
    {
        Deck.DeckType = Enumerations.DeckEnums.Custom;
        Deck.DeckName = Deck.DeckType.ToString();
        Deck.Cards.Clear();
        CurrentDeckMenu_FillList();

        LoadMenu(CurrentDeckMenu);
    }

    public void ChangeDeckMenu_SavedBtn()
    {
        SavedDeckMenu_FillList();
        LoadMenu(SavedDeckMenu);
    }

    public void ChangeDeckMenu_ReturnToBtn()
    {
        LoadMenu(ChangeDeckMenu);
    }

    //--- CURRENT DECK MENU ---
    public void CurrentDeckMenu_SaveAndUseBtn()
    {
        string deckName = currentDeckName.text;
        if (CurrentDeckMenu_CheckDeckName(deckName))
        {
            StartCoroutine(CurrentDeckMenu_ShowError("Deck name cannot contain '.'"));
            return;
        }
        if (!Directory.Exists(GamesManager.GetApplicationPath() + "/Decks"))
        {
            CurrentDeckMenu_CreateSaveDirectory();
            CurrentDeckMenu_CreateDeckConfigDat();
        }

        Deck.DeckType = Enumerations.DeckEnums.Saved;
        Deck.DeckName = deckName;
        mainMenuDeckType.text = Deck.DeckName;
        CurrentDeckMenu_SaveDeckToXml(deckName);

        LoadMenu(MainMenu);
    }

    public void CurrentDeckMenu_UseBtn()
    {
        Deck.DeckType = Enumerations.DeckEnums.Custom;
        Deck.DeckName = Deck.DeckType.ToString();
        mainMenuDeckType.text = Deck.DeckName;

        LoadMenu(MainMenu);
    }

    private bool CurrentDeckMenu_CheckDeckName(string deckName)
    {
        if (deckName.Contains(".")) return true;
        return false;
    }

    private IEnumerator CurrentDeckMenu_ShowError(string error)
    {
        currentDeckError.enabled = true;
        currentDeckError.text = error;
        yield return new WaitForSeconds(5);
        currentDeckError.text = "";
        currentDeckError.enabled = false;
    }

    private void CurrentDeckMenu_SaveDeckToXml(string deckName)
    {
        int deckId = CurrentDeckMenu_GetNextDeckId();

        XElement deck = new XElement("Deck");
        deck.SetAttributeValue("deckName", deckName);
        deck.SetAttributeValue("deckID", deckId);

        foreach (Card card in Deck.Cards)
        {
            XElement cardId = new XElement("Card", card.StaticIdCard);
            cardId.SetAttributeValue("name", card.Name);
            deck.Add(cardId);
        }

        XDocument doc = new XDocument(deck);
        string path = GamesManager.GetApplicationPath() + "/Decks/" + deckName + ".xml";
        File.WriteAllText(path, doc.ToString());
    }

    private int CurrentDeckMenu_GetNextDeckId()
    {
        XDocument doc = XDocument.Load(GamesManager.GetApplicationPath() + "/Configuration/Configuration.xml");
        int id = int.Parse(doc.XPathSelectElement("root/DeckID").Value); //nadi id
        doc.XPathSelectElement("root/DeckID").SetValue(id + 1); //povecaj ga za jedan

        doc.Save(GamesManager.GetApplicationPath() + "/Configuration/Configuration.xml"); //spremi novi id
        return id; //vrati id
    }

    private void CurrentDeckMenu_CreateDeckConfigDat()
    {
        XDocument doc = new XDocument(
            new XElement("root",
                new XElement("DeckID", 0)
                )
            );

        Directory.CreateDirectory(GamesManager.GetApplicationPath() + "/Configuration");
        string configPath = GamesManager.GetApplicationPath() + "/Configuration/Configuration.xml";
        File.WriteAllText(configPath, doc.ToString());
    }

    private void CurrentDeckMenu_CreateSaveDirectory()
    {
        Directory.CreateDirectory(GamesManager.GetApplicationPath() + "/Decks");
    }

    public void CurrentDeckMenu_FillList()
    {
        ICardDatabase database = Repository.GetCardDatabaseInstance();

        foreach (Transform child in currentDeckCardList)
        {
            Destroy(child.gameObject);
        }

        foreach (Card card in database.AllCards)
        {
            GameObject go = (GameObject)Resources.Load("MainMenuResources/CardItem");
            RectTransform prefab = Instantiate((RectTransform)go.transform);

            RectTransform cardRectTransform = (RectTransform)prefab.Find("Card");
            int numberInDeck = CurrentDeckMenu_GetNumberOfCardInDeck(card.StaticIdCard);
            CurrentDeckMenu_FillCardInfo(cardRectTransform, card, numberInDeck);

            prefab.SetParent(currentDeckCardList);
            prefab.localScale = new Vector3(1, 1, 1); //neznam zasto sam mjenja pa moram ja vratiti na default
        }
        currentDeckName.text = Deck.DeckName;

    }

    private int CurrentDeckMenu_GetNumberOfCardInDeck(string staticIdCard)
    {
        int counter = 0;
        foreach (Card card in Deck.Cards)
        {
            if (card.StaticIdCard == staticIdCard)
            {
                counter++;
            }
        }
        return counter;
    }

    private void CurrentDeckMenu_FillCardInfo(RectTransform cardRectTransform, Card card, int numberInDeck)
    {
        cardRectTransform.Find("CardName").GetComponentInChildren<Text>().text = card.Name;
        cardRectTransform.Find("CardStaticID").GetComponentInChildren<Text>().text = card.StaticIdCard;
        cardRectTransform.Find(Card.cardCooldownPath).GetComponentInChildren<Text>().text =
            card.DefaultCooldown.ToString();
        cardRectTransform.Find(Card.cardHealthPath).GetComponentInChildren<Text>().text =
            card.Health.ToString();
        cardRectTransform.Find(Card.cardAttackPath).GetComponentInChildren<Text>().text =
            card.Attack.ToString();
        cardRectTransform.parent.Find("ControlPanel/Counter").GetComponent<Text>().text = numberInDeck.ToString();

        if (card.SpecialAttackId != "")
        {
            cardRectTransform.Find("SpecialAttackSign").GetComponent<Image>().enabled = true;
        }

        //kasnije dodaj sliku
        switch (card.Quality)
        {
            case Enumerations.EquipmentQuality.Common:
                cardRectTransform.GetComponent<Image>().color = Color.white;
                break;
            case Enumerations.EquipmentQuality.Rare:
                cardRectTransform.GetComponent<Image>().color = new Color(0 / 255f, 107 / 255f, 255 / 255f); //blue
                break;
            case Enumerations.EquipmentQuality.Legendary:
                cardRectTransform.GetComponent<Image>().color = new Color(212 / 255f, 199 / 255f, 48 / 255f); //yellow
                break;
        }
    }

    //--- SAVED DECK MENU ---
    public void SavedDeckMenu_ChangeDeckBtn()
    {
        SavedDeckMenu_FillSavedDeck();
        CurrentDeckMenu_FillList();
        LoadMenu(CurrentDeckMenu);
    }

    public void SavedDeckMenu_UseDeckBtn()
    {
        SavedDeckMenu_FillSavedDeck();
        LoadMenu(MainMenu);
    }

    private void SavedDeckMenu_FillSavedDeck()
    {
        ToggleGroup tg = savedDeckDeckList.GetComponent<ToggleGroup>();
        tg.ActiveToggles().GetEnumerator().MoveNext();
        Toggle selectedDeck = tg.ActiveToggles().FirstOrDefault();
        Transform panel = selectedDeck.transform;

        string deckPath = panel.Find("DeckPath").GetComponent<Text>().text;
        string deckName = panel.Find("DeckName").GetComponent<Text>().text;
        deckName = deckName.Split('.')[0];
        SavedDeckMenu_LoadDeckFromXml(deckPath, deckName);
    }

    private void SavedDeckMenu_LoadDeckFromXml(string deckPath, string deckName)
    {
        XDocument doc = XDocument.Load(deckPath);

        Deck.Cards.Clear();
        ICardDatabase database = Repository.GetCardDatabaseInstance();
        foreach (XElement cardElement in doc.XPathSelectElements("Deck/Card"))
        {
            Deck.Cards.Add(database.GetNewCard(cardElement.Value));
        }

        Deck.DeckType = Enumerations.DeckEnums.Saved;
        Deck.DeckName = deckName;
        mainMenuDeckType.text = Deck.DeckName;
        currentDeckName.text = Deck.DeckName;
    }

    public void SavedDeckMenu_FillList()
    {
        foreach (Transform child in savedDeckDeckList)
        {
            Destroy(child.gameObject);
        }

        string[] savedDecks = SavedDeckMenu_GetAllSavedDecks();
        foreach (string path in savedDecks)
        {
            if (path.EndsWith(".xml"))
            {
                GameObject go = (GameObject)Resources.Load("MainMenuResources/DeckItem");
                RectTransform prefab = Instantiate((RectTransform)go.transform);
                string[] deckNameDetails = path.Split('/');
                string deckName = deckNameDetails[deckNameDetails.Length - 1];
                prefab.Find("DeckName").GetComponent<Text>().text = deckName;
                prefab.Find("DeckPath").GetComponent<Text>().text = path;

                prefab.GetComponent<Toggle>().group = savedDeckDeckList.GetComponent<ToggleGroup>();

                prefab.SetParent(savedDeckDeckList);
                prefab.localScale = new Vector3(1, 1, 1); //neznam zasto sam mjenja pa moram ja vratiti na default
            }
        }

        if (savedDeckDeckList.childCount > 0)
        {
            savedDeckDeckList.GetChild(0).GetComponent<Toggle>().isOn = true;
        }
    }

    private string[] SavedDeckMenu_GetAllSavedDecks()
    {
        return Directory.GetFiles(GamesManager.GetApplicationPath() + "/Decks/");
    }
}