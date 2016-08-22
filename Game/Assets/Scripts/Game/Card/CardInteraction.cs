using UnityEngine;
using UnityEngine.EventSystems;

public class CardInteraction : MonoBehaviour, IPointerClickHandler
{

    public RectTransform CdField;
    public bool Playable; //karte bota se nemogu odigrati

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Playable)
        {
            PlayCard();
        }
    }

    public void PlayCard()
    {
        RectTransform myRectTransform = GetComponent<RectTransform>();
        if (CdField.childCount < 5)
        {
            transform.SetParent(CdField);
            TurnsManager.instance.EndPickPhase(myRectTransform);
            if (TurnsManager.gameMode == Enumerations.GameModes.Online && TurnsManager.instance.IsCurrentPlayerA())
            {
                string staticId = myRectTransform.Find("CardStaticID").GetComponent<UnityEngine.UI.Text>().text;
                string serverMessage = "cardPlayed|" + staticId;
                ServerGameBehavior.SendMessage(serverMessage);
            }
        }
    }
}