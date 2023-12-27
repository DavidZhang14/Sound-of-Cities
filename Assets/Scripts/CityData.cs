using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class CityData
{
    public float[] playerPosition = new float[2];
    public List<SerializableStructure> structures = new();
    public CityData() {
        Dictionary<Vector3Int, Structure> structureDictionary = PlacementManager.instance.GetStructureDictionary();
        foreach(KeyValuePair<Vector3Int, Structure> pair in structureDictionary) {
            SerializableStructure structure = new SerializableStructure(pair.Key, pair.Value);
            structures.Add(structure);
        }
    }
    public CityData(Dictionary<Vector3Int, Structure> structureDictionary, Vector3 playerPosition) {
        foreach(KeyValuePair<Vector3Int, Structure> pair in structureDictionary) {
            SerializableStructure structure = new SerializableStructure(pair.Key, pair.Value);
            structures.Add(structure);
        }
        this.playerPosition[0] = playerPosition.x;
        this.playerPosition[1] = playerPosition.z;
    }
    public void Deserialize() {
        Vector3 newPosition = new Vector3(playerPosition[0], 0, playerPosition[1]);
        GameObject.Find("Character").transform.position = newPosition;
        for(int i = 0; i < structures.Count; i++)
            PlacementManager.instance.PlaceObjectOnTheMap(structures[i].position, structures[i].buildingIndex, structures[i].type);
            //TODO: pitch and rhythm
    }
}