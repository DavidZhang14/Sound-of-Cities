using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Numerics;

public static class SaveSystem {
    public static void saveCity(string saveName, PlacementManager placementManager) {
        XmlSerializer serializer = new XmlSerializer(typeof(CityData));
        string path = Application.persistentDataPath + "/" + saveName + ".city";
        FileStream fs = new FileStream(path, FileMode.Create);
        CityData data = new CityData(placementManager.GetStructureDictionary());
        serializer.Serialize(fs, data);
        fs.Close();
    }
    
    public static CityData loadCity(string saveName) {
        string path = Application.persistentDataPath + "/" + saveName + ".city";
        if (File.Exists(path)) {
            XmlSerializer serializer = new XmlSerializer(typeof(CityData));
            FileStream fs = new FileStream(path, FileMode.Open);
            CityData data = serializer.Deserialize(fs) as CityData;
            fs.Close();
            return data;
        }
        else {
            Debug.LogError("File not found: " + path);
            return null;
        }
    }
}