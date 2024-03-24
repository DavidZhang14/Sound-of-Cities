using AK.Wwise;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Unity.Netcode;

public class AdvancedPanel : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider, tempoSlider, loopLengthSlider;
    [SerializeField] private TMP_Text volumeText, tempoText, loopLengthText, joinCodeText;
    [SerializeField] private RTPC volumeRTPC, tempoRTPC;
    [SerializeField] private Toggle randomPitchToggle, randomRhythmToggle;

    void Start()
    {
        volumeSlider.onValueChanged.AddListener((v) => {
            volumeRTPC.SetGlobalValue(v);
            volumeText.SetText("Master Volume\n" + (int)v);
        });
        tempoSlider.onValueChanged.AddListener((v) => {
            //tempoSlider is interactable only in host mode
            if (!NetworkManager.Singleton.IsHost) return;
            tempoRTPC.SetGlobalValue(v);
            RhythmPanel.instance.Reset();
            tempoText.SetText("Tempo\n" + (int)v);
            RhythmPanel.tempo.Value = (short)v;
        });
        loopLengthSlider.onValueChanged.AddListener((v) => {
            RhythmPanel.instance.UpdateBeatPerMeasureClientRpc((short)v);
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

        if (NetworkManager.Singleton.IsHost) {
            tempoSlider.interactable = true;
            loopLengthSlider.interactable = true;
            tempoSlider.value = tempoRTPC.GetValue(gameObject);
        }
        else {
            tempoSlider.interactable = false;
            loopLengthSlider.interactable = false;
            tempoSlider.value = RhythmPanel.tempo.Value;
        }
        tempoText.SetText("Tempo\n" + (int)tempoSlider.value);

        loopLengthSlider.value = RhythmPanel.beatPerMeasure;
        loopLengthText.SetText("Loop Length\n" + RhythmPanel.beatPerMeasure);

        randomPitchToggle.isOn = GameManager.randomPitch;
        randomRhythmToggle.isOn = GameManager.randomRhythm;

        if (!GameManager.singlePlayer)
            joinCodeText.SetText("Join Code: " + GameManager.joinCode);

    }

    public void Exit() {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    public void Return() {
        gameObject.SetActive(false);
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
