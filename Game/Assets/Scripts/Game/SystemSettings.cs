using UnityEngine;
using System.Collections;

public class SystemSettings : MonoBehaviour {

    public static float GetCardWidth()
    {
        if (Screen.width < 1000)
        {
            return 65;
        }
        if (Screen.width < 1500)
        {
            return 140;
        }
        return 150;
    }   //postepeno povecavaj sirinu ovisno o sirini ekrana
}
