using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour
{
    //player positions - 
    //top left X=-4.46 Y=4.8
    //bot right X=4.5 Y=-4.8
    public int nextLevel;
    public CharacterMovement.Sides side;

    void OnTriggerStay2D(Collider2D other)
    {
        if (side == CharacterMovement.Sides.Right)
        {
            if (!CharacterMovement.instance.isMoving)
            {
                Vector3 position = CharacterMovement.instance.transform.position;
                position.x = -4.46f;
                CharacterMovement.instance.transform.position = position;
            }
        }
        if (side == CharacterMovement.Sides.Left)
        {
            //todo change map to right (player stays on y axis, x = 4.5)
            if (!CharacterMovement.instance.isMoving)
            {
                Vector3 position = CharacterMovement.instance.transform.position;
                position.x = 4.5f;
                CharacterMovement.instance.transform.position = position;
            }
        }
        if (side == CharacterMovement.Sides.Top)
        {
            //todo change map to right (player stays on x axis, y = -4.8)
            if (!CharacterMovement.instance.isMoving)
            {
                Vector3 position = CharacterMovement.instance.transform.position;
                position.y = -4.8f;
                CharacterMovement.instance.transform.position = position;
            }
        }
        if (side == CharacterMovement.Sides.Bot)
        {
            //todo change map to right (player stays on x axis, y = 4.8)
            if (!CharacterMovement.instance.isMoving)
            {
                Vector3 position = CharacterMovement.instance.transform.position;
                position.y = 4.8f;
                CharacterMovement.instance.transform.position = position;
            }
        }
    }

    public static class Maps
    {
        public static Map[] maps;

        static Maps()
        {
            maps = new Map[1];
            maps[0] = new Map(1, "Tutorial_1");
        }

    }

    public class Map
    {
        public int id;
        public string name;

        public Map(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}