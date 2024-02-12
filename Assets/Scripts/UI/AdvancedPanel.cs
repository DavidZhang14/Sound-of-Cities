using AK.Wwise;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class AdvancedPanel : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider, tempoSlider, loopLengthSlider;
    [SerializeField] private TMP_Text volumeText, tempoText, loopLengthText;
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
    }
    public void Exit() {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
