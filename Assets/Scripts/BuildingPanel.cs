using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingPanel : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown buildingDropdown;
    List<string> newOptions = new();
    public int GetDropdownValue() 
    {
        return buildingDropdown.value;
    }

    public void DisplayHouseList() 
    {
        buildingDropdown.ClearOptions();
        newOptions.Clear();
        for (int i = 0; i < StructureManager.instance.housesPrefabs.Length; i++)
        {
            newOptions.Add(StructureManager.instance.housesPrefabs[i].name
            + " (" + StructureManager.instance.housesPrefabs[i].GetComponent<StructureSoundEmitter>().instrument + ")");
        }
        buildingDropdown.AddOptions(newOptions);
    }

    public void DisplaySpecialList() 
    {
        buildingDropdown.ClearOptions();
        newOptions.Clear();
        for (int i = 0; i < StructureManager.instance.specialPrefabs.Length; i++)
        {
            newOptions.Add(StructureManager.instance.specialPrefabs[i].name
            + " (" + StructureManager.instance.specialPrefabs[i].GetComponent<StructureSoundEmitter>().instrument + ")");
        }
        buildingDropdown.AddOptions(newOptions);
    }
}
