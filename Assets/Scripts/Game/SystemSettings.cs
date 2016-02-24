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
            return 100;
        }
        if (Screen.width < 2000)
        {
            return 135;
        }
        return 200;
    }   //postepeno povecavaj sirinu ovisno o sirini ekrana
}
