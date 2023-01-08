using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraMovement cameraMovement;
    public RoadManager roadManager;
    public InputManager inputManager;

    public UIController uiController;

    public StructureManager structureManager;
    public PlacementManager placementManager;

    private void Start()
    {
        uiController.OnRoadPlacement += RoadPlacementHandler;
        uiController.OnHousePlacement += HousePlacementHandler;
        uiController.OnSpecialPlacement += SpecialPlacementHandler;
        uiController.OnEdit += EditHandler;
    }

    private void SpecialPlacementHandler()
    {
        ClearInputActions();
        inputManager.OnMouseClick += structureManager.PlaceSpecial;
    }

    private void HousePlacementHandler()
    {
        ClearInputActions();
        inputManager.OnMouseClick += structureManager.PlaceHouse;
    }

    private void RoadPlacementHandler()
    {
        ClearInputActions();
        inputManager.OnMouseClick += roadManager.PlaceRoad;
        inputManager.OnMouseHold += roadManager.PlaceRoad;
        inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
    }

    private void EditHandler() 
    {
        ClearInputActions();
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
        if (placementManager.CheckIfPositionIsOfType(position, CellType.Structure) ||
        placementManager.CheckIfPositionIsOfType(position, CellType.SpecialStructure)) //检查选择的位置是否有建筑
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
}
