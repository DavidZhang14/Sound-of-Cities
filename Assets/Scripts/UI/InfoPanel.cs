using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    public static InfoPanel instance;
    public TMP_Text Instrument;
    public TMP_Dropdown pitchDropdown;
    public TMP_Dropdown targetBeatDropdown, targetGridDropdown;
    public Slider volumeSlider;
    private short beatPerMeasure = 6;
    private void Awake() {
        if (instance == null) instance = this;
    }
    public void UpdatePitch()
    {
        UIController.Instance.editTarget.pitch = (short)pitchDropdown.value;
        UIController.Instance.editTarget.UpdateSound();
    }
    public void UpdateTargetGrid()
    {
        UIController.Instance.editTarget.targetGrid = (short)(targetBeatDropdown.value * 8 + targetGridDropdown.value + 1);
        //UpdateSound() is not necessary for changing targetGrid
    }
    public void UpdateObjectVolume() {
        UIController.Instance.editTarget.objectVolume = (short)volumeSlider.value;
        UIController.Instance.editTarget.UpdateSound();
    }
    private void OnEnable(){
        UpdateBeatDropdownText();
    }
    public void UpdateBeatDropdownText()
    {
        if (RhythmPanel.beatPerMeasure != beatPerMeasure)
        {
            beatPerMeasure = RhythmPanel.beatPerMeasure;
            for (int i = targetBeatDropdown.options.Count; i > beatPerMeasure; i--)
                targetBeatDropdown.options[i-1].text = "\"Beat " + i + "\"";
            for (int i = 1; i <= beatPerMeasure; i++)
                targetBeatDropdown.options[i-1].text = "Beat " + i;
        }
    }
}
