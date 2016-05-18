using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {
    private float movex = 0f;
    private float movey = 0f;
    private Rigidbody2D rigibody;
    private bool isMoving;

    //right, bot, left, top
    public static bool[] canMove = {true, true, true, true};

    // Use this for initialization
    void Awake()
    {
        rigibody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movex = Input.GetAxis("Horizontal");
        movey = Input.GetAxis("Vertical");

        if (movex > 0 && !isMoving && canMove[0])
        {
            StartCoroutine(Move(0.64f, 0));
        }

        if (movex < 0 && !isMoving && canMove[2])
        {
            StartCoroutine(Move(-0.64f, 0));
        }

        if (movey > 0 && !isMoving && canMove[3])
        {
            StartCoroutine(Move(0, 0.64f));
        }

        if (movey < 0 && !isMoving && canMove[1])
        {
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
            Debug.Log(isMoving);
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
            yield return new WaitForSeconds(0.01f);
        }
        isMoving = false;
    }
}