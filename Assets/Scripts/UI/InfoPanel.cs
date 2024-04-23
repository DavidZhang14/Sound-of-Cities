using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    public static InfoPanel Instance;
    public TMP_Text Instrument;
    public TMP_Dropdown pitchDropdown, octaveDropdown;
    public TMP_Dropdown targetBeatDropdown, targetGridDropdown;
    public Slider volumeSlider;
    private short beatPerMeasure = 6;
    private void Awake() {
        if (Instance == null) Instance = this;
    }
    public void UpdatePitch()
    {
        UIController.editTarget.pitch = (short)pitchDropdown.value;
        UIController.editTarget.UpdateSound();
    }
    public void UpdateOctave()
    {
        UIController.editTarget.octave = (short)octaveDropdown.value;
        UIController.editTarget.UpdateSound();
    }
    public void UpdateTargetGrid()
    {
        UIController.editTarget.targetGrid = (short)(targetBeatDropdown.value * 8 + targetGridDropdown.value + 1);
        //UpdateSound() is not necessary for changing targetGrid
    }
    public void UpdateObjectVolume() 
    {
        UIController.editTarget.objectVolume = (short)volumeSlider.value;
        UIController.editTarget.UpdateSound();
    }
    private void OnEnable()
    {
        UpdateBeatDropdownText();
    }
    public void UpdateInfoPanel() 
    {
        Instrument.text = UIController.editTarget.instrument;
        targetBeatDropdown.SetValueWithoutNotify((UIController.editTarget.targetGrid - 1) / 8);
        targetGridDropdown.SetValueWithoutNotify((UIController.editTarget.targetGrid - 1) % 8);
        octaveDropdown.SetValueWithoutNotify(UIController.editTarget.octave);
        pitchDropdown.value = UIController.editTarget.pitch % 12;
        volumeSlider.value = UIController.editTarget.objectVolume;
    }
    public void UpdateBeatDropdownText() {
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
