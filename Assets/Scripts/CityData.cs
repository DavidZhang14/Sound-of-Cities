using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CityData
{
    public double[] playerPosition;
    public Dictionary<Vector3Int, StructureModel> structureDictionary;
    public CityData(Dictionary<Vector3Int, StructureModel> structureDictionary) {
        this.structureDictionary = structureDictionary;
    }
}