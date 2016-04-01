using UnityEngine;
using UnityEngine.UI;

public class CustomDeckCardControls : MonoBehaviour
{
    public void AddCard()
    {
        string staticId = transform.Find("Card/CardStaticID").GetComponent<Text>().text;
        bool success = Deck.AddCard(staticId);

        if (success)
        {
            int counter = int.Parse(transform.Find("ControlPanel").Find("Counter").GetComponent<Text>().text);
            counter++;
            transform.Find("ControlPanel").Find("Counter").GetComponent<Text>().text = counter.ToString();
        }
        else
        {
            Debug.Log("Neuspjesno dodavanje karte: " + staticId);
        }
    }

    public void RemoveCard()
    {
        string staticId = transform.Find("Card/CardStaticID").GetComponent<Text>().text;
        bool success = Deck.RemoveCard(staticId);

        if (success)
        {
            int counter = int.Parse(transform.Find("ControlPanel").Find("Counter").GetComponent<Text>().text);
            counter--;
            transform.Find("ControlPanel").Find("Counter").GetComponent<Text>().text = counter.ToString();
        }
        else
        {
            Debug.Log("Neuspjesno micanje karte: " + staticId);
        }
    }
}