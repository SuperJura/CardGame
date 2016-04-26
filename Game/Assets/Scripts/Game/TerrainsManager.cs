using UnityEngine;

public class TerrainsManager : MonoBehaviour
{
    void Start()
    {
        int terrainIndex = Random.Range(0, 2);

        Debug.Log(terrainIndex);
        switch (terrainIndex)
        {
            case 0:
                LoadTerrain("Meadow");
                break;
            case 1:
                LoadTerrain("Desert");
                break;
        }
    }

    private void LoadTerrain(string terrainName)
    {
        GameObject go = (GameObject)Resources.Load("GameResources/Terrains/" + terrainName);

        Transform terrain = Instantiate((Transform)go.transform);
        terrain.SetParent(null);
    }
}