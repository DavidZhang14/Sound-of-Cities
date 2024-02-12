using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager instance;
    public int width, height;
    Grid placementGrid;

    private Dictionary<Vector3Int, Structure> temporaryRoadobjects = new();
    private Dictionary<Vector3Int, Structure> structureDictionary = new();

    private void Awake() {
        if (instance == null) instance = this;
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

    internal void PlaceObjectOnTheMap(Vector3Int position, int buildingIndex, CellType type, short pitch, short targetGrid, short objectVolume)
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
        structure.buildingIndex = buildingIndex;
        structureDictionary.Add(position, structure);

        // Set pitch, rhythm, and object volume
        structure.soundEmitter.pitch = pitch;
        structure.soundEmitter.targetGrid = targetGrid;
        structure.soundEmitter.objectVolume = objectVolume;
        structure.soundEmitter.UpdateSound();

        DestroyNatureAt(position);
    }
    internal void PlaceObjectOnTheMap(Vector3Int position, int buildingIndex, CellType type)
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
        structure.buildingIndex = buildingIndex;
        structureDictionary.Add(position, structure);

        // Set pitch, rhythm, and object volume
        if (GameManager.randomPitch) structure.soundEmitter.pitch = (short)Random.Range(0, 12);
        else structure.soundEmitter.pitch = 9; //A
        if (GameManager.randomRhythm) structure.soundEmitter.targetGrid = (short)(Random.Range(0, RhythmPanel.beatPerMeasure * 8) + 1);
        else structure.soundEmitter.targetGrid = 1;
        structure.soundEmitter.UpdateSound();

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
        List<Vector3Int> neighbours = new();
        foreach (var point in neighbourVertices)
        {
            neighbours.Add(new Vector3Int(point.X, 0, point.Y));
        }
        return neighbours;
    }

    private Structure CreateANewStructureModel(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        GameObject structureOBject = new(type.ToString());
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
        List<Vector3Int> path = new();
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
        return structureDictionary[position].soundEmitter;
    }
    public Dictionary<Vector3Int, Structure> GetStructureDictionary() {
        return structureDictionary;
    }
    public void TryClearCity() {
        UIController.Instance.OpenConfirmationPanel("Are you sure to clear the city?");
        ConfirmationPanel.YesButtonClicked += ClearCity;
        ConfirmationPanel.NoButtonClicked += ClearCityCancelled;
    }
    public void ClearCity() {
        RemoveAllTemporaryStructures();
        for (int i = gameObject.transform.childCount - 1; i >= 0; i--) {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            Destroy(child);
        }
        structureDictionary.Clear();
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                placementGrid[i,j] = CellType.Empty;
            }
        }
        // Close info panel
        if (InfoPanel.instance) InfoPanel.instance.gameObject.SetActive(false);
        ConfirmationPanel.YesButtonClicked -= ClearCity;
        ConfirmationPanel.NoButtonClicked -= ClearCityCancelled;
    }
    private void ClearCityCancelled() {
        ConfirmationPanel.YesButtonClicked -= ClearCity;
        ConfirmationPanel.NoButtonClicked -= ClearCityCancelled;
    }

    private Vector3Int deletePos;
    internal void TryDeleteObject(Vector3Int position) {
        deletePos = position;
        CellType cellType = placementGrid[position.x, position.z];
        if (cellType == CellType.Road) {
            DeleteObjectConfirmed();
            RoadManager.instance.FixRoadPrefabs();
        }
        else if (cellType != CellType.Empty) {
            string message = "Are you sure to delete this structure?";
            UIController.Instance.OpenConfirmationPanel(message);
            ConfirmationPanel.YesButtonClicked += DeleteObjectConfirmed;
            ConfirmationPanel.NoButtonClicked += DeleteObjectCancelled;
        }
    }

    private void DeleteObjectConfirmed() {
        ConfirmationPanel.YesButtonClicked -= DeleteObjectConfirmed;
        ConfirmationPanel.NoButtonClicked -= DeleteObjectCancelled;
        structureDictionary.TryGetValue(deletePos, out Structure structure);

        // If the structure is the edit target, close the info panel
        if (structure.soundEmitter == UIController.Instance.editTarget)
            InfoPanel.instance.gameObject.SetActive(false);

        Destroy(structure.gameObject);
        structureDictionary.Remove(deletePos);
        placementGrid[deletePos.x, deletePos.z] = CellType.Empty;
    }
    
    private void DeleteObjectCancelled() {
        ConfirmationPanel.YesButtonClicked -= DeleteObjectConfirmed;
        ConfirmationPanel.NoButtonClicked -= DeleteObjectCancelled;
    }
}
