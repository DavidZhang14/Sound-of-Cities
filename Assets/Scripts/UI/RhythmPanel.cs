using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class RhythmPanel : NetworkBehaviour
{
    public delegate void MusicControl();
    public static event MusicControl NewGridReached, TriggerSound;
    public static RhythmPanel instance;
    public static NetworkVariable<short> currentGrid = new(0);
    public static NetworkVariable<short> currentBeat = new(0);
    [SerializeField] private RawImage[] Beats = new RawImage[5];

    public static short beatPerMeasure = 4;
    public static short gridPerMeasure = 32;

    private Color transparent = new(255, 255, 255, 0); 
    private Color opaque = new(255, 255, 255, 255);
    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void OnEnable() {
        // NewGridReached is only called on the host, not the client
        NewGridReached += NewGrid;
    }
    void OnDisable() {
        // NewGridReached is only called on the host, not the client
        NewGridReached -= NewGrid;
    }
    //The following two methods are called only at host
    private void NewGrid() {
        currentGrid.Value += 1;
        if (currentGrid.Value > gridPerMeasure) currentGrid.Value = 1;
        if (currentGrid.Value % 8 == 1) NewBeat();
        TriggerSoundClientRpc();
    }
    private void NewBeat() {
        currentBeat.Value += 1;
        if (currentBeat.Value > beatPerMeasure) currentBeat.Value = 1; 
        UpdateRhythmPanelClientRpc(currentBeat.Value);
    }
    [ClientRpc] private void TriggerSoundClientRpc() {
        //This method is called on both the host and the client
        TriggerSound?.Invoke();
    }
    [ClientRpc] private void UpdateRhythmPanelClientRpc(short beat) {
        if (beat == 1) {
            for (int i = 0; i < beatPerMeasure - 1; i++) Beats[i].color = transparent;
        }
        else {
            for (int i = 0; i < beat - 1; i++) Beats[i].color = opaque;
        }
    }
    public void UpdateBeatPerMeasure(short newMeter) {
        beatPerMeasure = newMeter;
        gridPerMeasure = (short)(newMeter*8);
        for (int i = 0; i < beatPerMeasure - 1; i++) 
        {
            Beats[i].gameObject.SetActive(true);
            if (currentBeat.Value < i + 2) Beats[i].color = transparent;
        }
        for (int i = beatPerMeasure - 1; i < Beats.Length; i++) Beats[i].gameObject.SetActive(false);
    }
    private void CallbackFunction(object in_cookie, AkCallbackType in_type, object in_info) {
        NewGridReached?.Invoke();
    }
    public void Reset() {
        AkSoundEngine.PostEvent("Main_Loop", gameObject, (uint)AkCallbackType.AK_MusicSyncGrid, CallbackFunction, null);
    }
}
