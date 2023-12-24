[System.Serializable]
public class SerializableStructure
{
    public int buildingIndex;
    public CellType type;
    public short pitch;
    public string instrument;
    public short targetGrid;
    public SerializableStructure(){}
    public SerializableStructure(Structure structure) {
        type = structure.type;
        if (type != CellType.Road) {
            buildingIndex = structure.buildingIndex;
            pitch = structure.structureSoundEmitter.pitch;
            instrument = structure.structureSoundEmitter.instrument;
            targetGrid = structure.structureSoundEmitter.targetGrid;
        }
        else {
            buildingIndex = 0;
        }
    }
}