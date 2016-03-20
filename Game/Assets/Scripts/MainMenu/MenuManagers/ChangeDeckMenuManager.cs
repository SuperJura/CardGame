using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using UnityEngine.UI;

public class ChangeDeckMenuManager : MonoBehaviour {

    public DeckPanelManager deckPanelManager;

    public void RandomBtnClick()
    {
        Deck.DeckType = DeckEnums.Random;
        deckPanelManager.SetDeckType();
        Deck.Cards = Repository.GetCardDatabaseInstance().GetRandomDeck();
    }

    public void CustomBtnClick()
    {
        Deck.DeckType = DeckEnums.Custom;
        Deck.Cards = new List<Card>();
        deckPanelManager.SetDeckType();
    }

    public void SaveDeck()
    {
        if (!Directory.Exists(Application.dataPath + "/Decks"))
        {
            CreateSaveDirectory();
            CreateDeckConfigDat();
        }

        Deck.DeckType = DeckEnums.Saved;
        deckPanelManager.SetDeckType();
        SaveDeckToXML();
    }

    private void SaveDeckToXML()
    {
        int deckID = GetNextDeckID();
        string deckName = transform.Find("DeckName/Text").GetComponent<Text>().text + "_" + deckID;

        XElement deck = new XElement("Deck");
        deck.SetAttributeValue("deckName", deckName);

        foreach (Card card in Deck.Cards)
        {
            XElement cardID = new XElement("Card", card.StaticIDCard);
            cardID.SetAttributeValue("name", card.Name);
            deck.Add(cardID);
        }

        XDocument doc = new XDocument(deck);
        File.WriteAllText(Application.dataPath + "/Decks/" + deckName + ".xml", doc.ToString());
    }

    private int GetNextDeckID()
    {
        XDocument doc = XDocument.Load(Application.dataPath + "/Configuration/Configuration.xml");
        int id = int.Parse(doc.XPathSelectElement("root/DeckID").Value);    //nadi id
        doc.XPathSelectElement("root/DeckID").SetValue(id + 1);  //povecaj ga za jedan

        doc.Save(Application.dataPath + "/Configuration/Configuration.xml");  //spremi novi id
        return id;  //vrati id
    }

    private void CreateDeckConfigDat()
    {
        XDocument doc = new XDocument(
            new XElement("root",
              new XElement("DeckID", 0)
            )
      );

        Directory.CreateDirectory(Application.dataPath + "/Configuration");
        string configPath = Application.dataPath + "/Configuration/Configuration.xml";
        File.WriteAllText(configPath, doc.ToString());
    }

    private void CreateSaveDirectory()
    {
        Directory.CreateDirectory(Application.dataPath + "/Decks");
    }
}