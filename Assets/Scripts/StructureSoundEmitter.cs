using UnityEngine;

public class StructureSoundEmitter : MonoBehaviour
{  
    public short pitch;
    public string instrument = "Harp";
    public short targetGrid;
    public short objectVolume = 100;
    void OnEnable() {
        RhythmPanel.TriggerSound += EmitSound;
        UpdateSound();
    }
    void OnDisable() {
        RhythmPanel.TriggerSound -= EmitSound;
    }
    private void EmitSound() {
        if (RhythmPanel.currentGrid.Value == targetGrid)
            AkSoundEngine.PostEvent("Note_Trigger", gameObject);
    }
    public void UpdateSound() {
        AkSoundEngine.SetRTPCValue("Pitch", pitch, gameObject);
        AkSoundEngine.SetRTPCValue("ObjectVolume", objectVolume, gameObject);
        AkSoundEngine.SetSwitch("Instrument", instrument, gameObject);
    }
}
