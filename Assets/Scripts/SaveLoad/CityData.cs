using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CityData
{
    public float[] playerPosition = new float[2];
    public List<SerializableStructure> structures = new();
    public CityData() {}
    public CityData(Dictionary<Vector3Int, Structure> structureDictionary, Vector3 playerPosition) {
        foreach(KeyValuePair<Vector3Int, Structure> pair in structureDictionary) {
            SerializableStructure structure = new SerializableStructure(pair.Key, pair.Value);
            structures.Add(structure);
        }
        this.playerPosition[0] = playerPosition.x;
        this.playerPosition[1] = playerPosition.z;
    }
    private static Vector3 cameraOffset = new Vector3(-4, 7, -6);
    public void Deserialize() {
        PlacementManager.instance.ClearCity();

        // Set player and camera position
        Vector3 newPosition = new Vector3(playerPosition[0], 0, playerPosition[1]);
        GameObject.Find("Character").transform.position = newPosition;
        GameObject.Find("Main Camera").transform.position = newPosition + cameraOffset;
        for(int i = 0; i < structures.Count; i++) {
            PlacementManager.instance.PlaceObjectOnTheMap(
                structures[i].position, 
                structures[i].buildingIndex, 
                structures[i].type, 
                structures[i].pitch, 
                structures[i].targetGrid);
        }
    }
}