using UnityEngine;
using System.Collections;

public class RoofDissapear : MonoBehaviour
{
    GameObject roofGameObject;

    void Start()
    {
        roofGameObject = transform.Find("Roof").gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        roofGameObject.SetActive(false);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        roofGameObject.SetActive(true);
    }
}
