using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public static class SaveSystem {
    public static void SaveCity(string saveName) {
        XmlSerializer serializer = new XmlSerializer(typeof(CityData));
        string path = Application.persistentDataPath + "/" + saveName + ".city";
        FileStream fs = new FileStream(path, FileMode.Create);
        CityData data = new CityData(PlacementManager.instance.GetStructureDictionary());
        serializer.Serialize(fs, data);
        fs.Close();
        Debug.Log("Save Path: " + path);
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