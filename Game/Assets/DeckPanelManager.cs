using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class DeckPanelManager : MonoBehaviour {

    Text deckTypeText;

    void Start () {
        deckTypeText = transform.Find("DeckType").GetComponent<Text>();
        SetDeckType();

#if UNITY_WEBPLAYER
        transform.Find("ChangeDeckBtn").gameObject.SetActive(false);
#endif
    }

    public void SetDeckType()
    {
        deckTypeText.text = Deck.DeckType.ToString();
    }
}