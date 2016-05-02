using UnityEngine;

public class TerrainsManager : MonoBehaviour
{
    void Start()
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

    private void LoadTerrain(string terrainName)
    {
        GameObject go = (GameObject)Resources.Load("GameResources/Terrains/" + terrainName);

        Transform terrain = Instantiate(go.transform);
        terrain.SetParent(null);
    }
}