using UnityEngine;
using System.Collections;

public class SpriteAnimation : MonoBehaviour
{
    public Sprite[] frames;
    public int framesPerSecond;
    public bool randomizeFlip = true;

    private SpriteRenderer renderer;
    // Use this for initialization

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    void Start ()
    {
        if (randomizeFlip)
        {
            renderer.flipX = Random.Range(0f, 1f) > 0.5f;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        int index = (int)(Time.time * framesPerSecond);
        index = index % frames.Length;
        renderer.sprite = frames[index];
    }
}
