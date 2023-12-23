using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CameraMovement cameraMovement;
    public RoadManager roadManager;
    public InputManager inputManager;

    public UIController uiController;

    private StructureManager structureManager;
    public PlacementManager placementManager;
    private GameObject character;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        character = GameObject.Find("Character");
        structureManager = StructureManager.instance;
        uiController.OnRoadPlacement += RoadPlacementHandler;
        uiController.OnHousePlacement += HousePlacementHandler;
        uiController.OnSpecialPlacement += SpecialPlacementHandler;
        uiController.OnEdit += EditHandler;
    }

    private void SpecialPlacementHandler()
    {
        ClearInputActions();
        uiController.buildingPanel.gameObject.SetActive(true);
        uiController.buildingPanel.DisplaySpecialList();
        inputManager.OnMouseClick += structureManager.PlaceSpecial;
    }

    private void HousePlacementHandler()
    {
        ClearInputActions();
        uiController.buildingPanel.gameObject.SetActive(true);
        uiController.buildingPanel.DisplayHouseList();
        inputManager.OnMouseClick += structureManager.PlaceHouse;
    }

    private void RoadPlacementHandler()
    {
        ClearInputActions();
        uiController.buildingPanel.gameObject.SetActive(false);
        inputManager.OnMouseClick += roadManager.PlaceRoad;
        inputManager.OnMouseHold += roadManager.PlaceRoad;
        inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
    }

    private void EditHandler() 
    {
        ClearInputActions();
        uiController.buildingPanel.gameObject.SetActive(false);
        inputManager.OnMouseClick += selectEditTarget;
    }

    private void ClearInputActions()
    {
        inputManager.OnMouseClick = null;
        inputManager.OnMouseHold = null;
        inputManager.OnMouseUp = null;
    }

    private void Update()
    {
        cameraMovement.MoveCamera(new Vector3(inputManager.CameraMovementVector.x,0, inputManager.CameraMovementVector.y));
    }

    private void selectEditTarget(Vector3Int position)
    {
        if (placementManager.CheckIfPositionIsOfType(position, CellType.House) ||
        placementManager.CheckIfPositionIsOfType(position, CellType.Special)) //检查选择的位置是否有建筑
        {
            UIController.Instance.infoPanel.gameObject.SetActive(true);
            UIController.Instance.editTarget = placementManager.GetSoundEmitter(position);
            UIController.Instance.updateInfoPanel();
        }
        else 
        {
            UIController.Instance.infoPanel.gameObject.SetActive(false);
        }
    }
    
    public void Save(string saveName = "test") {
        //TODO: character
        SaveSystem.saveCity(saveName, placementManager);
    }
    public void Load(string saveName = "test") {
        CityData data = SaveSystem.loadCity(saveName);
        placementManager.SetStructureDictionary(data.structureDictionary);
    }
}
