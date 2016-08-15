using UnityEngine;
using System.Collections;

public class CantMoveTrigger : MonoBehaviour
{
    public CharacterMovement.Sides side;

    void OnTriggerStay2D(Collider2D other)
    {
        CharacterMovement.canMove[(int)side] = false;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        CharacterMovement.canMove[(int) side] = true;
    }

    void OnDestroy()
    {
        CharacterMovement.canMove[(int) side] = true;
    }
}
