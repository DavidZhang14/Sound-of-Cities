using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Structure
{
    public int buildingIndex;
    public CellType type;
    public StructureModel model;
    public Structure(int buildingIndex, CellType type, StructureModel model) {
        this.buildingIndex = buildingIndex;
        this.type = type;
        this.model = model;
    }
}