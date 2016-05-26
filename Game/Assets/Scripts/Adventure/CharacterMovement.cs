using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{

    public static CharacterMovement instance;

    public Sprite[] topWalking;
    public Sprite[] botWalking;
    public Sprite[] rigthWalking;
    public Sprite[] leftWalking;
    public int spritesPerSecond;


    private Sprite[] currentWalking;
    private float movex = 0f;
    private float movey = 0f;
    private Rigidbody2D rigibody;
    private SpriteRenderer renderer;
    public bool isMoving;

    //right, bot, left, top
    public static bool[] canMove = {true, true, true, true};

    // Use this for initialization
    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        rigibody = GetComponent<Rigidbody2D>();
        instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movex = Input.GetAxis("Horizontal");
        movey = Input.GetAxis("Vertical");

        if (movex > 0 && !isMoving && canMove[(int)Sides.Right])
        {
            currentWalking = rigthWalking;
            StartCoroutine(Move(0.64f, 0));
        }

        if (movex < 0 && !isMoving && canMove[(int)Sides.Left])
        {
            currentWalking = leftWalking;
            StartCoroutine(Move(-0.64f, 0));
        }

        if (movey > 0 && !isMoving && canMove[(int)Sides.Top])
        {
            currentWalking = topWalking;
            StartCoroutine(Move(0, 0.64f));
        }

        if (movey < 0 && !isMoving && canMove[(int)Sides.Bot])
        {
            currentWalking = botWalking;
            StartCoroutine(Move(0, -0.64f));
        }

    }

    private IEnumerator Move(float x, float y)
    {
        Vector3 newPosition = transform.position;
        newPosition.x += x;
        newPosition.y += y;
        if (Mathf.Abs(newPosition.x) > 5)
        {
            isMoving = false;
            yield break;
        }
        if (Mathf.Abs(newPosition.y) > 5)
        {
            isMoving = false;
            yield break;
        }

        isMoving = true;
        while (transform.position != newPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, 0.1f);

            int index = (int)(Time.time * spritesPerSecond);
            index = index % currentWalking.Length;
            renderer.sprite = currentWalking[index];

            yield return new WaitForSeconds(0.01f);
        }
        isMoving = false;
    }

    public enum Sides
    {
        Right,
        Bot,
        Left,
        Top
    }
}