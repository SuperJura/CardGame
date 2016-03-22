using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using UnityEngine.UI;
using System;

public class CurrentDeckMenuManager : MonoBehaviour {

    public DeckPanelManager deckPanelManager;
    public MenuManager menuManager;
    public GameObject changeDeckMenu;

    private string applicationPath;


    Text errorTextSaving;

    void Start()
    {
        errorTextSaving = transform.Find("ErrorText").GetComponent<Text>();
        applicationPath = Application.dataPath;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            applicationPath = Application.persistentDataPath;
        }
    }

    public void SaveDeckBtnClick()
    {
        string deckName = transform.Find("DeckName/Text").GetComponent<Text>().text;
        if (CheckDeckName(deckName))
        {
            StartCoroutine(ShowError("Deck name cannot contain '.'"));
            return;
        }
        if (!Directory.Exists(applicationPath + "/Decks"))
        {
            CreateSaveDirectory();
            CreateDeckConfigDat();
        }

        Deck.DeckType = DeckEnums.Saved;
        deckPanelManager.SetDeckType();
        SaveDeckToXML(deckName);

        menuManager.LoadMenu(changeDeckMenu);
    }

    private bool CheckDeckName(string deckName)
    {
        if (deckName.Contains("."))
        {
            return true;
        }
        return false;
    }

    private IEnumerator ShowError(string error)
    {
        errorTextSaving.enabled = true;
        errorTextSaving.text = error;
        yield return new WaitForSeconds(5);
        errorTextSaving.text = "";
        errorTextSaving.enabled = false;
    }

    private void SaveDeckToXML(string deckName)
    {
        int deckID = GetNextDeckID();

        XElement deck = new XElement("Deck");
        deck.SetAttributeValue("deckName", deckName);
        deck.SetAttributeValue("deckID", deckID);

        foreach (Card card in Deck.Cards)
        {
            XElement cardID = new XElement("Card", card.StaticIDCard);
            cardID.SetAttributeValue("name", card.Name);
            deck.Add(cardID);
        }

        XDocument doc = new XDocument(deck);
        string path = applicationPath + "/Decks/" + deckName + ".xml";
        File.WriteAllText(path, doc.ToString());
    }

    private int GetNextDeckID()
    {
        XDocument doc = XDocument.Load(applicationPath + "/Configuration/Configuration.xml");
        int id = int.Parse(doc.XPathSelectElement("root/DeckID").Value);    //nadi id
        doc.XPathSelectElement("root/DeckID").SetValue(id + 1);  //povecaj ga za jedan

        doc.Save(applicationPath + "/Configuration/Configuration.xml");  //spremi novi id
        return id;  //vrati id
    }

    private void CreateDeckConfigDat()
    {
        XDocument doc = new XDocument(
            new XElement("root",
              new XElement("DeckID", 0)
            )
      );

        Directory.CreateDirectory(applicationPath + "/Configuration");
        string configPath = applicationPath + "/Configuration/Configuration.xml";
        File.WriteAllText(configPath, doc.ToString());
    }

    private void CreateSaveDirectory()
    {
        Directory.CreateDirectory(applicationPath + "/Decks");
    }
}