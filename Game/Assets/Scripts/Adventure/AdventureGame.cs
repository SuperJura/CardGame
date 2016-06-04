using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class AdventureGame : MonoBehaviour
{

    const string path = "/save.sav";
    public static string currentMap;
    // Use this for initialization
    void Awake()
    {
        LoadAdventureGame();
    }

    private void LoadAdventureGame()
    {
        if (File.Exists(Application.persistentDataPath + path))
        {
            Debug.Log("Loading");
            LoadGame();
        }
        else
        {
            Debug.Log("Creating " + Application.persistentDataPath + path);
            SaveData save = new SaveData();
            save.map = "Tutorial_1";
            save.SetPosition(new Vector3(-4.46f, 4.8f, 0));
            SaveGame(save);
            LoadGame();
        }
    }

    private static void LoadGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream saveFile = File.Open(Application.persistentDataPath + path, FileMode.Open);
        SaveData save = (SaveData) bf.Deserialize(saveFile);
        saveFile.Close();
        currentMap = save.map;
        
        GameObject map = (GameObject)Instantiate(Resources.Load("AdventureResources/Maps/" + save.map));
        GameObject player =  (GameObject)Instantiate(Resources.Load("AdventureResources/Player"));
        player.transform.position = save.GetVector3Position();
    }

    public static void SaveGame(SaveData save)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream saveFile = File.Create(Application.persistentDataPath + path);
        bf.Serialize(saveFile, save);
        saveFile.Close();
    }

    public static void SaveGame()
    {
        SaveData save = new SaveData();
        save.map = currentMap;
        save.SetPosition(CharacterMovement.instance.transform.position);

        SaveGame(save);
    }

[Serializable]
public struct SaveData
    {
        public float positionX;
        public float positionY;
        public float positionZ;
        public string map;

        public void SetPosition(Vector3 position)
        {
            positionX = position.x;
            positionY = position.y;
            positionZ = position.z;
        }

        public Vector3 GetVector3Position()
        {
            return new Vector3(positionX, positionY, positionZ);
        }
    }
}
