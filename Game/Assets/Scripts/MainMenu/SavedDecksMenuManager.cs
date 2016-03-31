using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Xml.Linq;
using System.Xml.XPath;

public class SavedDecksMenuManager : MonoBehaviour {

    public DeckPanelManager deckPanelManager;

    public void LoadDeck()
    {
        ToggleGroup tg = transform.Find("DeckList/Decks").GetComponent<ToggleGroup>();
        tg.ActiveToggles().GetEnumerator().MoveNext();
        Toggle selectedDeck = tg.ActiveToggles().FirstOrDefault();
        Transform panel = selectedDeck.transform;

        string deckPath = panel.Find("DeckPath").GetComponent<Text>().text;
        string deckName = panel.Find("DeckName").GetComponent<Text>().text;
        deckName = deckName.Split('.')[0];
        LoadDeckFromXML(deckPath, deckName);
    }

    private void LoadDeckFromXML(string DeckPath, string deckName)
    {
        XDocument doc = XDocument.Load(DeckPath);

        Deck.Cards.Clear();
        ICardDatabase database = Repository.GetCardDatabaseInstance();
        foreach (XElement cardElement in doc.XPathSelectElements("Deck/Card"))
        {
            Deck.Cards.Add(database.GetNewCard(cardElement.Value));
        }

        Deck.DeckType = DeckEnums.Saved;
        Deck.DeckName = deckName;
        deckPanelManager.SetDeckType();
    }
}
