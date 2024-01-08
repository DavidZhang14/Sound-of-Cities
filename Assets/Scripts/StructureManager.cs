using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    public static StructureManager instance;
    public GameObject[] housesPrefabs, specialPrefabs;
    public PlacementManager placementManager;
    [SerializeField] private BuildingPanel buildingPanel;
    private void Awake() {
        if (instance == null) 
        instance = this;
        else Destroy(this.gameObject);
    }
    public void PlaceHouse(Vector3Int position)
    {
        if (CheckPositionBeforePlacement(position))
        {
            int buildingIndex = buildingPanel.GetDropdownValue();
            placementManager.PlaceObjectOnTheMap(position, buildingIndex, CellType.House);
            AudioPlayer.instance.PlayPlacementSound();
        }
    }

    public void PlaceSpecial(Vector3Int position)
    {
        if (CheckPositionBeforePlacement(position))
        {
            int buildingIndex = buildingPanel.GetDropdownValue();
            placementManager.PlaceObjectOnTheMap(position, buildingIndex, CellType.Special);
            AudioPlayer.instance.PlayPlacementSound();
        }
    }

    private bool CheckPositionBeforePlacement(Vector3Int position)
    {
        if (placementManager.CheckIfPositionInBound(position) == false)
        {
            Debug.Log("This position is out of bounds");
            return false;
        }
        if (placementManager.CheckIfPositionIsFree(position) == false)
        {
            Debug.Log("This position is not EMPTY");
            return false;
        }
        if(placementManager.GetNeighboursOfTypeFor(position,CellType.Road).Count <= 0)
        {
            Debug.Log("Must be placed near a road");
            return false;
        }
        return true;
    }
}
