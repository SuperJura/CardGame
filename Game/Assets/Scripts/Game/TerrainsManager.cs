using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainsManager : MonoBehaviour
{
    public static string adventureTerrain;

    void Start()
    {
        if (!string.IsNullOrEmpty(adventureTerrain))
        {
            //loadaj terrain ovisno o avanturi
            LoadTerrain(adventureTerrain);
        }
        else
        {
            int terrainIndex = Random.Range(0, 4);

            switch (terrainIndex)
            {
                case 0:
                    LoadTerrain("Meadow");
                    break;
                case 1:
                    LoadTerrain("Desert");
                    break;
                case 2:
                    LoadTerrain("Bridge");
                    break;
                case 3:
                    LoadTerrain("Volcano");
                    break;
            }
        }
    }

    private void LoadTerrain(string terrainName)
    {
        GameObject terrain = (GameObject)Instantiate(Resources.Load("GameResources/Terrains/" + terrainName));
    }
}