using UnityEngine;
using UnityEngine.UI;

public class RhythmPanel : MonoBehaviour
{
    public static RhythmPanel instance;
    public short currentGrid = 0;
    public short currentBeat = 0;
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
        currentGrid += 1;
        if (currentGrid > gridPerMeasure) currentGrid = 1;
        if (currentGrid % 8 == 1) NewBeat();
    }
    private void NewBeat() {
        currentBeat += 1;
        if (currentBeat > beatPerMeasure) currentBeat = 1; 
        UpdateRhythmPanel();
    }
    private void UpdateRhythmPanel() {
        if (currentBeat == 1) {
            for (int i = 0; i < beatPerMeasure - 1; i++) Beats[i].color = transparent;
        }
        else {
            for (int i = 0; i < currentBeat - 1; i++) Beats[i].color = opaque;
        }
    }
    public void UpdateBeatPerMeasure(short newMeter) {
        beatPerMeasure = newMeter;
        gridPerMeasure = (short)(newMeter*8);
        for (int i = 0; i < beatPerMeasure - 1; i++) Beats[i].gameObject.SetActive(true);
        for (int i = beatPerMeasure - 1; i < Beats.Length; i++) Beats[i].gameObject.SetActive(false);
    }
}
