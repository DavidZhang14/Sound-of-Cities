using UnityEngine;

public class StructureSoundEmitter : MonoBehaviour
{  
    public short pitch;
    public string instrument = "Harp";
    public short targetGrid;
    public short objectVolume = 100;
    public short octave = 0;
    void OnEnable() {
        UpdateSound();
        RhythmPanel.TriggerSound += EmitSound;
    }
    void OnDisable() {
        RhythmPanel.TriggerSound -= EmitSound;
    }
    private void EmitSound() {
        if (RhythmPanel.currentGrid.Value == targetGrid)
            AkSoundEngine.PostEvent("Note_Trigger", gameObject);
    }
    public void UpdateSound() {
        AkSoundEngine.SetRTPCValue("Pitch", pitch + octave * 12, gameObject);
        AkSoundEngine.SetRTPCValue("ObjectVolume", objectVolume, gameObject);
        AkSoundEngine.SetSwitch("Instrument", instrument, gameObject);
    }
}
