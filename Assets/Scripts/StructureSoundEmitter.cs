using UnityEngine;

public class StructureSoundEmitter : MonoBehaviour
{  
    public short pitch = 9; //A
    public string instrument = "Harp";
    public short targetGrid = 1;
    public short objectVolume = 100;
    private RhythmPanel rhythmPanel;
    private void Awake() {
        rhythmPanel = GameObject.Find("RhythmPanel").GetComponent<RhythmPanel>();
    }
    void OnEnable() {
        MusicController.NewGridReached += EmitSound;
        UpdateSound();
    }
    void OnDisable() {
        MusicController.NewGridReached -= EmitSound;
    }
    private void EmitSound() {
        if (rhythmPanel.currentGrid == targetGrid)
        AkSoundEngine.PostEvent("Note_Trigger", gameObject);
    }
    public void UpdateSound() {
        AkSoundEngine.SetRTPCValue("Pitch", pitch, gameObject);
        AkSoundEngine.SetRTPCValue("ObjectVolume", objectVolume, gameObject);
        AkSoundEngine.SetSwitch("Instrument", instrument, gameObject);
    }
}
