using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using UnityEngine;
using UnityEngine.UI;

public class SavedDecksMenuManager : MonoBehaviour
{
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
        LoadDeckFromXml(deckPath, deckName);
    }

    private void LoadDeckFromXml(string deckPath, string deckName)
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
        deckPanelManager.SetDeckType();
    }
}