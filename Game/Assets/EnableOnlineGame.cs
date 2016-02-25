using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnableOnlineGame : MonoBehaviour {
    
    void Start () {
        GameObject.Find("GameManager").GetComponent<GamesManager>().OnServerConnectionOpened += EnableOnlineGame_OnServerConnectionOpened;
    }

    private void EnableOnlineGame_OnServerConnectionOpened()
    {
        this.GetComponent<Button>().interactable = true;
    }
}