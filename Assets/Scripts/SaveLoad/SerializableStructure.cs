using UnityEngine;
[System.Serializable]
public class SerializableStructure
{
    public Vector3Int position;
    public int buildingIndex;
    public CellType type;
    public short pitch;
    public string instrument;
    public short targetGrid;
    public short objectVolume = 100;
    public SerializableStructure(){}
    public SerializableStructure(Vector3Int position, Structure structure) {
        this.position = position;
        type = structure.type;
        if (type != CellType.Road) {
            buildingIndex = structure.buildingIndex;
            pitch = (short)(structure.soundEmitter.pitch + structure.soundEmitter.octave * 12);
            instrument = structure.soundEmitter.instrument;
            targetGrid = structure.soundEmitter.targetGrid;
            objectVolume = structure.soundEmitter.objectVolume;
        }
        else {
            buildingIndex = 0;
        }
    }
}