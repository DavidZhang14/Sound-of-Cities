using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmPanel : MonoBehaviour
{
    private short beatPerMeasure = 4;
    public short currentBeat = 1;
    public void newBeat() {
        currentBeat += 1;
        if (currentBeat > beatPerMeasure) currentBeat = 1; 
        
    }
}
