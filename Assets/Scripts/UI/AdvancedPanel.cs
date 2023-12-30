using UnityEngine;
using UnityEngine.UI;

public class AdvancedPanel : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider, tempoSlider;
    [SerializeField] private AK.Wwise.RTPC volumeRTPC;

    void Start()
    {
        volumeSlider.onValueChanged.AddListener((v) => {
            volumeRTPC.SetGlobalValue(v);
        });
    }
}
