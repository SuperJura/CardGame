using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
    public Texture background;

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), background);
    }
}