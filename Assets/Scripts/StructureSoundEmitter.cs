using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureSoundEmitter : MonoBehaviour
{  
    public string pitch = "A";
    public string instrument = "Harp";
    public short targetGrid = 1;
    public RhythmPanel rhythmPanel;
    void OnEnable() {
        MusicController.NewGridReached += EmitSound;
        UpdateSound();
        rhythmPanel = GameObject.Find("RhythmPanel").GetComponent<RhythmPanel>();
    }
    void OnDisable() {
        MusicController.NewGridReached -= EmitSound;
    }
    private void EmitSound() {
        if (rhythmPanel.currentGrid == targetGrid)
        AkSoundEngine.PostEvent("Note_Trigger", this.gameObject);
    }

    public void UpdateSound() {
        AkSoundEngine.SetSwitch("Pitch", pitch, this.gameObject);
        AkSoundEngine.SetSwitch("Instrument", instrument, this.gameObject);
    }
}
