using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    public static InfoPanel instance;
    public TMP_Text Instrument;
    public TMP_Dropdown pitchDropdown;
    public TMP_Dropdown targetGridDropdown;
    public Slider volumeSlider;
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
        UIController.Instance.editTarget.targetGrid = (short)(targetGridDropdown.value + 1);
        //UpdateSound() is not necessary for changing targetGrid
    }
    public void UpdateObjectVolume() {
        UIController.Instance.editTarget.objectVolume = (short)volumeSlider.value;
        UIController.Instance.editTarget.UpdateSound();
    }
}
