using SVS;
using UnityEngine;
using UnityEditor;
using TMPro;
using System.IO;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private CameraMovement cameraMovement;
    public RoadManager roadManager;
    private InputManager inputManager;

    public UIController uiController;

    private StructureManager structureManager;
    public PlacementManager placementManager;
    public GameObject loadBtnPrefab;
    private string savePath;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (CameraMovement.instance != null) cameraMovement = CameraMovement.instance;
        else Debug.LogError("CameraMovement not found.");
        if (InputManager.instance != null) inputManager = InputManager.instance;
        else Debug.LogError("InputManager not found.");
        if (StructureManager.instance != null) structureManager = StructureManager.instance;
        else Debug.LogError("StructureManager not found.");
        uiController.OnRoadPlacement += RoadPlacementHandler;
        uiController.OnHousePlacement += HousePlacementHandler;
        uiController.OnSpecialPlacement += SpecialPlacementHandler;
        uiController.OnEdit += EditHandler;

        savePath = Application.persistentDataPath + "/save/";
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
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
            UIController.Instance.UpdateInfoPanel();
        }
        else 
        {
            UIController.Instance.infoPanel.gameObject.SetActive(false);
        }
    }
    public static void OpenSavePanel() {
        GameObject savePanel = GameObject.Find("Canvas").transform.Find("SavePanel").gameObject;
        if (savePanel != null) savePanel.SetActive(true);
        else Debug.LogError("GameObject 'SavePanel' not found.");
    }
    public void OpenLoadPanel() {
        GameObject loadPanel = GameObject.Find("Canvas").transform.Find("LoadPanel").gameObject;
        if (loadPanel != null) loadPanel.SetActive(true);
        else Debug.LogError("GameObject 'LoadPanel' not found.");

        Transform content = GameObject.Find("Canvas").transform
                .Find("LoadPanel")
                .Find("Scroll View")
                .Find("Viewport")
                .Find("Content");
        //Clear list
        for (int i = content.childCount - 1; i >= 0; i--) {
            GameObject child = content.transform.GetChild(i).gameObject;
            Destroy(child);
        }
        //Create new list
        string[] filePaths = Directory.GetFiles(savePath);
        foreach (string filePath in filePaths)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            GameObject newBtn = Instantiate(loadBtnPrefab, content);
            newBtn.transform.Find("Text (TMP)").GetComponent<TMP_Text>().text = fileName;
        }
    }
    public void SaveButtonClicked() {
        string saveName = GameObject.Find("SaveInput").GetComponent<TMP_InputField>().text;
        SaveSystem.SaveCity(saveName);
        GameObject.Find("SavePanel").SetActive(false);
    }
    public void ExplorerButtonClicked() {
        EditorUtility.RevealInFinder(savePath);
    }
}
