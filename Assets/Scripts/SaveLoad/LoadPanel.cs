using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
using Unity.Netcode;

public class LoadPanel : MonoBehaviour
{
    [SerializeField] private Toggle loadTempoToggle, loadLoopLengthToggle;
    public void ExplorerButtonClicked() 
    {
        Process.Start(GameManager.savePath);
    }
    private void OnEnable() {
        if (!NetworkManager.Singleton.IsServer) {
            GameManager.loadTempo = false;
            GameManager.loadBeatPerMeasure = false;
            loadTempoToggle.isOn = false;
            loadLoopLengthToggle.isOn = false;
            loadTempoToggle.interactable = false;
            loadLoopLengthToggle.interactable = false;
            return;
        }
        
        loadTempoToggle.isOn = GameManager.loadTempo;
        loadLoopLengthToggle.isOn = GameManager.loadBeatPerMeasure;
        loadTempoToggle.onValueChanged.AddListener((v) => {
            GameManager.loadTempo = v;
        });
        loadLoopLengthToggle.onValueChanged.AddListener((v) => {
            GameManager.loadBeatPerMeasure = v;
        });
    }
}
