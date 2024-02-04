using UnityEngine;

public class StructureSoundEmitter : MonoBehaviour
{  
    public short pitch;
    public string instrument = "Harp";
    public short targetGrid = 1;
    public short objectVolume = 100;
    private RhythmPanel rhythmPanel;
    private void Awake() {
        rhythmPanel = GameObject.Find("RhythmPanel").GetComponent<RhythmPanel>();
        if (GameManager.randomPitch) pitch = (short)Random.Range(0, 12);
        else pitch = 9; //A
        if (GameManager.randomRhythm) targetGrid = (short)Random.Range(0, 16);
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
