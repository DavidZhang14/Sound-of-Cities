using AK.Wwise;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class AdvancedPanel : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider, tempoSlider, loopLengthSlider;
    [SerializeField] private TMP_Text volumeText, tempoText, loopLengthText, ipText;
    [SerializeField] private RTPC volumeRTPC, tempoRTPC;
    [SerializeField] private Toggle randomPitchToggle, randomRhythmToggle;

    void Start()
    {
        volumeSlider.onValueChanged.AddListener((v) => {
            volumeRTPC.SetGlobalValue(v);
            volumeText.SetText("Master Volume\n" + (int)v);
        });
        tempoSlider.onValueChanged.AddListener((v) => {
            tempoRTPC.SetGlobalValue(v);
            MusicController.instance.Reset();
            tempoText.SetText("Tempo\n" + (int)v);
        });
        loopLengthSlider.onValueChanged.AddListener((v) => {
            RhythmPanel.instance.UpdateBeatPerMeasure((short)v);
            if (InfoPanel.instance) InfoPanel.instance.UpdateBeatDropdownText();
            loopLengthText.SetText("Loop Length\n" + (int)v);
        });
        randomPitchToggle.onValueChanged.AddListener((v) => {
            GameManager.randomPitch = v;
        });
        randomRhythmToggle.onValueChanged.AddListener((v) => {
            GameManager.randomRhythm = v;
        });
    } 
    private void OnEnable() {  
        volumeSlider.value = volumeRTPC.GetValue(gameObject);
        volumeText.SetText("Master Volume\n" + (int)volumeSlider.value);

        tempoSlider.value = tempoRTPC.GetValue(gameObject);
        tempoText.SetText("Tempo\n" + (int)tempoSlider.value);

        loopLengthSlider.value = RhythmPanel.beatPerMeasure;
        loopLengthText.SetText("Loop Length\n" + RhythmPanel.beatPerMeasure);

        randomPitchToggle.isOn = GameManager.randomPitch;
        randomRhythmToggle.isOn = GameManager.randomRhythm;

        ipText.SetText("Local IP Address: " + GameManager.localIp);

    }

    public void Exit() {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    public void TransposeUp() {
        foreach (var pair in PlacementManager.instance.GetStructureDictionary())
        {
            Structure structure = pair.Value;
            if (structure.type != CellType.Road) {
                structure.soundEmitter.pitch += 1;
                if (structure.soundEmitter.pitch == 12) structure.soundEmitter.pitch = 0;
                structure.soundEmitter.UpdateSound();
            }
        }
        if(UIController.editTarget) UIController.Instance.UpdateInfoPanel();
    }

    public void TransposeDown() {
        foreach (var pair in PlacementManager.instance.GetStructureDictionary())
        {
            Structure structure = pair.Value;
            if (structure.type != CellType.Road) {
                structure.soundEmitter.pitch -= 1;
                if (structure.soundEmitter.pitch == -1) structure.soundEmitter.pitch = 11;
                structure.soundEmitter.UpdateSound();
            }
        }
        if(UIController.editTarget) UIController.Instance.UpdateInfoPanel();
    }
}
