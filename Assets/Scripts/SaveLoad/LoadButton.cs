using TMPro;
using UnityEngine;

public class LoadButton : MonoBehaviour
{
    public void LoadButtonClicked() {
        string saveName = gameObject.transform.Find("Text (TMP)").GetComponent<TMP_Text>().text;
        CityData data = SaveSystem.LoadCity(saveName);
        data.Deserialize();
        GameObject.Find("LoadPanel").SetActive(false);
    }
}
