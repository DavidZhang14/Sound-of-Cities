using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmPanel : MonoBehaviour
{
    private short beatPerMeasure = 4;
    private short gridPerMeasure = 32;
    public short currentGrid = 0;
    public short currentBeat = 0;
    [SerializeField] private RawImage Beat2;
    [SerializeField] private RawImage Beat3;
    [SerializeField] private RawImage Beat4;

    private Color transparent = new Color(255, 255, 255, 0); 
    private Color opaque = new Color(255, 255, 255, 255);

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
            Beat2.color = transparent;
            Beat3.color = transparent;
            Beat4.color = transparent;
        }
        if (currentBeat >= 2) Beat2.color = opaque;
        if (currentBeat >= 3) Beat3.color = opaque;
        if (currentBeat >= 4) Beat4.color = opaque;
    }
}
