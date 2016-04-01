using UnityEngine;
using UnityEngine.UI;

public class DeckPanelManager : MonoBehaviour
{
    private Text deckTypeText;

    private void Start()
    {
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