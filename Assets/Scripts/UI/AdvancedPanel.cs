using AK.Wwise;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedPanel : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider, tempoSlider;
    [SerializeField] private TMP_Text volumeText, tempoText;
    [SerializeField] private RTPC volumeRTPC, tempoRTPC;

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
    } 
    private void OnEnable() {
        volumeSlider.value = volumeRTPC.GetValue(gameObject);
        volumeText.SetText("Master Volume\n" + (int)volumeSlider.value);

        tempoSlider.value = tempoRTPC.GetValue(gameObject);
        tempoText.SetText("Tempo\n" + (int)tempoSlider.value);
    }
}
