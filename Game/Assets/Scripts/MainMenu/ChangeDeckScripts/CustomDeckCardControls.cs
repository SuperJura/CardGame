using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CustomDeckCardControls : MonoBehaviour {

    public void AddCard()
    {
        string staticID = transform.Find("Card/CardStaticID").GetComponent<Text>().text;
        bool success = Deck.AddCard(staticID);

        if (success)
        {
            int counter = int.Parse(transform.Find("ControlPanel").Find("Counter").GetComponent<Text>().text);
            counter++;
            transform.Find("ControlPanel").Find("Counter").GetComponent<Text>().text = counter.ToString();
        }
        else
        {
            Debug.Log("Neuspjesno dodavanje karte: " + staticID);
        }
    }

    public void RemoveCard()
    {
        string staticID = transform.Find("Card/CardStaticID").GetComponent<Text>().text;
        bool success = Deck.RemoveCard(staticID);

        if (success)
        {
            int counter = int.Parse(transform.Find("ControlPanel").Find("Counter").GetComponent<Text>().text);
            counter--;
            transform.Find("ControlPanel").Find("Counter").GetComponent<Text>().text = counter.ToString();
        }
        else
        {
            Debug.Log("Neuspjesno micanje karte: " + staticID);
        }
    }
}