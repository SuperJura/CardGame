using UnityEngine;
using System.Collections;
using System.Linq;

public class MapManager : MonoBehaviour
{
    //player positions - 
    //top left X=-4.46 Y=4.8
    //bot right X=4.5 Y=-4.8
    public int nextMap;
    public CharacterMovement.Sides side;

    void OnTriggerEnter2D(Collider2D other)
    {
        CharacterMovement.instance.whereToSwitch = side;
        CharacterMovement.instance.nextMap = nextMap;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        CharacterMovement.instance.whereToSwitch = CharacterMovement.Sides.Noone;
        CharacterMovement.instance.nextMap = -1;
    }

    public static void ChangeMap(int mapId)
    {
        GameObject currentMap = GameObject.Find(AdventureGame.currentMap + "(Clone)");
        Destroy(currentMap);
        string newMapName = Maps.FindMap(mapId).name;
        Instantiate(Resources.Load("AdventureResources/Maps/" + newMapName));
        AdventureGame.currentMap = newMapName;
        AdventureGame.SaveGame();
    }

    public static class Maps
    {
        public static Map[] maps;

        static Maps()
        {
            maps = new Map[2];
            maps[0] = new Map(1, "Tutorial_1");
            maps[1] = new Map(2, "Tutorial_2");
        }

        public static Map FindMap(int id)
        {
            return maps.FirstOrDefault(map => map.id == id);
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