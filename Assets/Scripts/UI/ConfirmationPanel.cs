using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmationPanel : MonoBehaviour
{
    [SerializeField] private Button yesButton, noButton;
    [SerializeField] private TMP_Text confirmationText;
    public delegate void Confirmed();
    public static event Confirmed YesButtonClicked;
    private void Awake() {
        yesButton.onClick.AddListener(YesButtonClick);
        noButton.onClick.AddListener(NoButtonClick);
    }
    private void YesButtonClick()
    {
        YesButtonClicked?.Invoke();
        gameObject.SetActive(false);
    }
    private void NoButtonClick()
    {
        gameObject.SetActive(false);
    }
}
