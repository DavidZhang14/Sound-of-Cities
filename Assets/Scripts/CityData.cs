using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class CityData
{
    public double[] playerPosition;
    [SerializeField] private List<Vector3Int> positions = new List<Vector3Int>();
    [SerializeField] private List<Structure> structures = new List<Structure>();
    public CityData(){}
    public CityData(Dictionary<Vector3Int, Structure> structureDictionary) {
        foreach(KeyValuePair<Vector3Int, Structure> pair in structureDictionary) {
            positions.Add(pair.Key);
            structures.Add(pair.Value);
        }
    }
    public Dictionary<Vector3Int, Structure> structureDictionary() {
        Dictionary<Vector3Int, Structure> structureDictionary = new Dictionary<Vector3Int, Structure>();
        for(int i = 0; i < positions.Count; i++) 
            structureDictionary.Add(positions[i], structures[i]);
        return structureDictionary;
    }
}