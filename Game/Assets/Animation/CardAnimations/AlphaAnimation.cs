using UnityEngine;
using UnityEngine.UI;

namespace Assets.Animation.CardAnimations
{
    public class AlphaAnimation : MonoBehaviour
    {
        private Image image;
        private Color color;
        private float originalAlpha;
        private bool fading;
        // Use this for initialization
        void OnEnable ()
        {
            image = GetComponent<Image>();
            originalAlpha = image.color.a;
            fading = false;
        }
	
        // Update is called once per frame
        void Update ()
        {
            color = image.color;
            if (image.color.a < 0.90 && !fading) color.a += 0.02f;
            else
            {
                color.a -= 0.01f;
                fading = true;
            }
            image.color = color;
        }

        void OnDisable()
        {
            color.a = originalAlpha;
            image.color = color;
        }
    }
}
