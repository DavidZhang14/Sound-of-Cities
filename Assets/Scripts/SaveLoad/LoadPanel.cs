using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;

public class LoadPanel : MonoBehaviour
{
    [SerializeField] private Toggle loadTempoToggle, loadLoopLengthToggle;
    public void ExplorerButtonClicked() 
    {
        Process.Start(GameManager.savePath);
    }
    private void OnEnable() {
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
