using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class CityData
{
    public double[] playerPosition = new double[2];
    public List<SerializableStructure> structures = new();
    public CityData() {
        Dictionary<Vector3Int, Structure> structureDictionary = PlacementManager.instance.GetStructureDictionary();
        foreach(KeyValuePair<Vector3Int, Structure> pair in structureDictionary) {
            SerializableStructure structure = new SerializableStructure(pair.Key, pair.Value);
            structures.Add(structure);
        }
    }
    public CityData(Dictionary<Vector3Int, Structure> structureDictionary) {
        foreach(KeyValuePair<Vector3Int, Structure> pair in structureDictionary) {
            SerializableStructure structure = new SerializableStructure(pair.Key, pair.Value);
            structures.Add(structure);
        }
    }
    public void Deserialize() {
        for(int i = 0; i < structures.Count; i++)
            PlacementManager.instance.PlaceObjectOnTheMap(structures[i].position, structures[i].buildingIndex, structures[i].type);
            //TODO: pitch and rhythm
    }
}