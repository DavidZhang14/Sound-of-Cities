using SVS;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using TMPro;
using System.IO;
using Unity.Netcode;
using System.Net;
using Unity.Netcode.Transports.UTP;
public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    private CameraMovement cameraMovement;
    public RoadManager roadManager;
    private InputManager inputManager;

    public UIController uiController;

    private StructureManager structureManager;
    public PlacementManager placementManager;
    public GameObject loadBtnPrefab;
    public static string serverIP = null;
    public static bool clientMode = false;
    private string savePath;
    public static bool randomPitch = true, randomRhythm = true;

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
            
        if (clientMode) StartClient(); 
        else StartHost();
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
        inputManager.OnMouseClick += SelectEditTarget;
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

    private void SelectEditTarget(Vector3Int position)
    {
        if (placementManager.CheckIfPositionIsOfType(position, CellType.House) ||
        placementManager.CheckIfPositionIsOfType(position, CellType.Special)) //检查选择的位置是否有建筑
        {
            // Destroy previous outline
            if (UIController.editTarget && 
                (UIController.editTarget != placementManager.GetSoundEmitter(position)))
                    Destroy(UIController.editTarget.gameObject.GetComponent<Outline>());

            UIController.Instance.infoPanel.gameObject.SetActive(true);
            UIController.editTarget = placementManager.GetSoundEmitter(position);

            // Create new outline
            if (!UIController.editTarget.gameObject.GetComponent<Outline>())
                UIController.editTarget.gameObject.AddComponent<Outline>();

            UIController.Instance.UpdateInfoPanel();
        }
        else 
        {
            UIController.Instance.infoPanel.gameObject.SetActive(false);
            if (UIController.editTarget) 
            {
                Destroy(UIController.editTarget.gameObject.GetComponent<Outline>());
                UIController.editTarget = null;
            }
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
        Process.Start(savePath);
    }

    public void StartHost() {
        if (NetworkManager.Singleton.IsServer) return;

        NetworkManager.Singleton.Shutdown();
        IPHostEntry hostEntry=Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in hostEntry.AddressList) {
            if (ip.AddressFamily==System.Net.Sockets.AddressFamily.InterNetwork) {
                serverIP = ip.ToString();
            }
        }
        if (serverIP != null) NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(serverIP, 7777);
        NetworkManager.Singleton.StartHost();
    }
    public void StartClient() {
        if (!NetworkManager.Singleton.IsServer) return;
        NetworkManager.Singleton.Shutdown();
        NetworkManager.Singleton.StartClient();
    }
}
