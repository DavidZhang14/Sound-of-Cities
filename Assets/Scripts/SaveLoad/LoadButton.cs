using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoadButton : MonoBehaviour, IPointerClickHandler
{
    private string saveName;
    private void Start() {
        saveName = gameObject.transform.Find("Text (TMP)").GetComponent<TMP_Text>().text;
    }
    public void LoadButtonClicked() {
        CityData data = SaveSystem.LoadCity(saveName);
        data.Deserialize();
        GameObject.Find("LoadPanel").SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) DeleteData();
    }
    private void DeleteData() {
        string message = "Are you sure to delete " + saveName + "?";
        UIController.Instance.OpenConfirmationPanel(message);
        ConfirmationPanel.YesButtonClicked += DeleteDataConfirmed;
        ConfirmationPanel.NoButtonClicked += DeleteDataCancelled;
    }
    private void DeleteDataConfirmed() {
        SaveSystem.DeleteCity(saveName);
        ConfirmationPanel.YesButtonClicked -= DeleteDataConfirmed;
        ConfirmationPanel.NoButtonClicked -= DeleteDataCancelled;
        Destroy(gameObject);
    }
    private void DeleteDataCancelled() {
        ConfirmationPanel.YesButtonClicked -= DeleteDataConfirmed;
        ConfirmationPanel.NoButtonClicked -= DeleteDataCancelled;
    }
}
