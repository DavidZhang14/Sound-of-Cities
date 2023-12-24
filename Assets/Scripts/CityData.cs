using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class CityData
{
    public double[] playerPosition = new double[2];
    public List<Vector3Int> positions = new List<Vector3Int>();
    public List<SerializableStructure> structures = new List<SerializableStructure>();
    public CityData() {
        Dictionary<Vector3Int, Structure> structureDictionary = PlacementManager.instance.GetStructureDictionary();
        foreach(KeyValuePair<Vector3Int, Structure> pair in structureDictionary) {
            positions.Add(pair.Key);
            SerializableStructure structure = new SerializableStructure(pair.Value);
            structures.Add(structure);
        }
    }
    public CityData(Dictionary<Vector3Int, Structure> structureDictionary) {
        foreach(KeyValuePair<Vector3Int, Structure> pair in structureDictionary) {
            positions.Add(pair.Key);
            SerializableStructure structure = new SerializableStructure(pair.Value);
            structures.Add(structure);
        }
    }
    public void Deserialize() {
        for(int i = 0; i < positions.Count; i++)
            PlacementManager.instance.PlaceObjectOnTheMap(positions[i], structures[i].buildingIndex, structures[i].type);
            //TODO: pitch and rhythm
    }
}