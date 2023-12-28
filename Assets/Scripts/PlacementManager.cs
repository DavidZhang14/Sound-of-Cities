using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager instance;
    public int width, height;
    Grid placementGrid;

    private Dictionary<Vector3Int, Structure> temporaryRoadobjects = new Dictionary<Vector3Int, Structure>();
    private Dictionary<Vector3Int, Structure> structureDictionary = new Dictionary<Vector3Int, Structure>();

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    private void Start()
    {
        placementGrid = new Grid(width, height);
    }

    internal CellType[] GetNeighbourTypesFor(Vector3Int position)
    {
        return placementGrid.GetAllAdjacentCellTypes(position.x, position.z);
    }

    internal bool CheckIfPositionInBound(Vector3Int position)
    {
        if(position.x >= 0 && position.x < width && position.z >=0 && position.z < height)
        {
            return true;
        }
        return false;
    }

    internal void PlaceObjectOnTheMap(Vector3Int position, int buildingIndex, CellType type, short pitch = 9, short targetGrid = 1)
    {
        if (type == CellType.Road) {
            RoadManager.instance.PlaceRoad(position);
            RoadManager.instance.FinishPlacingRoad();
            return;
        }
        placementGrid[position.x, position.z] = type;
        GameObject structurePrefab = null;
        if (type == CellType.House) structurePrefab = StructureManager.instance.housesPrefabs[buildingIndex];
        else if (type == CellType.Special) structurePrefab = StructureManager.instance.specialPrefabs[buildingIndex];
        Structure structure = CreateANewStructureModel(position, structurePrefab, type);
        structureDictionary.Add(position, structure);

        // Set pitch and rhythm
        structure.structureSoundEmitter.pitch = pitch;
        structure.structureSoundEmitter.targetGrid = targetGrid;
        structure.structureSoundEmitter.UpdateSound();

        DestroyNatureAt(position);
    }

    private void DestroyNatureAt(Vector3Int position)
    {
        RaycastHit[] hits = Physics.BoxCastAll(position + new Vector3(0, 0.5f, 0), new Vector3(0.5f, 0.5f, 0.5f), transform.up, Quaternion.identity, 1f, 1 << LayerMask.NameToLayer("Nature"));
        foreach (var item in hits) Destroy(item.collider.gameObject);
    }

    internal bool CheckIfPositionIsFree(Vector3Int position)
    {
        return CheckIfPositionIsOfType(position, CellType.Empty);
    }

    internal bool CheckIfPositionIsOfType(Vector3Int position, CellType type)
    {
        return placementGrid[position.x, position.z] == type;
    }

    internal void PlaceTemporaryStructure(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        placementGrid[position.x, position.z] = type;
        Structure structure = CreateANewStructureModel(position, structurePrefab, type);
        temporaryRoadobjects.Add(position, structure);
    }

    internal List<Vector3Int> GetNeighboursOfTypeFor(Vector3Int position, CellType type)
    {
        var neighbourVertices = placementGrid.GetAdjacentCellsOfType(position.x, position.z, type);
        List<Vector3Int> neighbours = new List<Vector3Int>();
        foreach (var point in neighbourVertices)
        {
            neighbours.Add(new Vector3Int(point.X, 0, point.Y));
        }
        return neighbours;
    }

    private Structure CreateANewStructureModel(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        GameObject structureOBject = new GameObject(type.ToString());
        structureOBject.transform.SetParent(transform);
        structureOBject.transform.localPosition = position;
        Structure structure = structureOBject.AddComponent<Structure>();
        structure.type = type;
        structure.CreateModel(structurePrefab);
        return structure;
    }

    internal List<Vector3Int> GetPathBetween(Vector3Int startPosition, Vector3Int endPosition)
    {
        var resultPath = GridSearch.AStarSearch(placementGrid, new Point(startPosition.x, startPosition.z), new Point(endPosition.x, endPosition.z));
        List<Vector3Int> path = new List<Vector3Int>();
        foreach (Point point in resultPath)
        {
            path.Add(new Vector3Int(point.X, 0, point.Y));
        }
        return path;
    }

    internal void RemoveAllTemporaryStructures()
    {
        foreach (var structure in temporaryRoadobjects.Values)
        {
            var position = Vector3Int.RoundToInt(structure.transform.position);
            placementGrid[position.x, position.z] = CellType.Empty;
            Destroy(structure.gameObject);
        }
        temporaryRoadobjects.Clear();
    }

    internal void AddtemporaryStructuresToStructureDictionary()
    {
        foreach (var roadObject in temporaryRoadobjects)
        {
            Structure structure = roadObject.Value;
            structureDictionary.Add(roadObject.Key, structure);
            DestroyNatureAt(roadObject.Key);
        }
        temporaryRoadobjects.Clear();
    }

    public void ModifyStructureModel(Vector3Int position, GameObject newModel, Quaternion rotation)
    {
        if (temporaryRoadobjects.ContainsKey(position))
            temporaryRoadobjects[position].SwapModel(newModel, rotation);
        else if (structureDictionary.ContainsKey(position))
            structureDictionary[position].SwapModel(newModel, rotation);
    }

    public StructureSoundEmitter GetSoundEmitter(Vector3Int position) {
        return structureDictionary[position].structureSoundEmitter;
    }
    public Dictionary<Vector3Int, Structure> GetStructureDictionary() {
        return structureDictionary;
    }
    public void ClearCity() {
        foreach (KeyValuePair<Vector3Int, Structure> pair in structureDictionary)
            Destroy(pair.Value.gameObject);
        structureDictionary.Clear();
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                placementGrid[i,j] = CellType.Empty;
            }
        }
    }
}
