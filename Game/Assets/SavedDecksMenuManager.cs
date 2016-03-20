using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Xml.Linq;
using System.Xml.XPath;

public class SavedDecksMenuManager : MonoBehaviour {

    public DeckPanelManager deckPanelManager;

    public void UseDeckBtnClick()
    {
        ToggleGroup tg = transform.Find("DeckList/Decks").GetComponent<ToggleGroup>();
        tg.ActiveToggles().GetEnumerator().MoveNext();
        Toggle selectedDeck = tg.ActiveToggles().FirstOrDefault();
        Transform panel = selectedDeck.transform;

        string DeckPath = panel.Find("DeckPath").GetComponent<Text>().text;
        LoadDeck(DeckPath);
    }

    private void LoadDeck(string DeckPath)
    {
        XDocument doc = XDocument.Load(DeckPath);

        Deck.Cards.Clear();
        ICardDatabase database = Repository.GetCardDatabaseInstance();
        foreach (XElement cardElement in doc.XPathSelectElements("Deck/Card"))
        {
            Deck.Cards.Add(database.GetCard(cardElement.Value));
        }

        Deck.DeckType = DeckEnums.Saved;
        deckPanelManager.SetDeckType();
    }
}
