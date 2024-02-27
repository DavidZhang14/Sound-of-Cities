using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class RhythmPanel : NetworkBehaviour
{
    public static RhythmPanel instance;
    public NetworkVariable<short> currentGrid = new(0);
    public NetworkVariable<short> currentBeat = new(0);
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
        MusicController.NewGridReached += NewGrid;
    }
    void OnDisable() {
        MusicController.NewGridReached -= NewGrid;
    }
    private void NewGrid() {
        if (!IsHost) return;
        currentGrid.Value += 1;
        if (currentGrid.Value > gridPerMeasure) currentGrid.Value = 1;
        if (currentGrid.Value % 8 == 1) NewBeat();
    }
    private void NewBeat() {
        currentBeat.Value += 1;
        if (currentBeat.Value > beatPerMeasure) currentBeat.Value = 1; 
        UpdateRhythmPanelClientRpc();
    }
    [ClientRpc]
    private void UpdateRhythmPanelClientRpc() {
        if (currentBeat.Value == 1) {
            for (int i = 0; i < beatPerMeasure - 1; i++) Beats[i].color = transparent;
        }
        else {
            for (int i = 0; i < currentBeat.Value - 1; i++) Beats[i].color = opaque;
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
}
