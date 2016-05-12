using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlphaAnimationFast : MonoBehaviour {


    private Image image;
    private Color color;
    private float originalAlpha;

    void OnEnable()
    {
        image = GetComponent<Image>();
        originalAlpha = image.color.a;
        color = image.color;
        color.a = 1;
        image.color = color;
    }

    void OnDisable()
    {
        color.a = originalAlpha;
        image.color = color;
    }
}
