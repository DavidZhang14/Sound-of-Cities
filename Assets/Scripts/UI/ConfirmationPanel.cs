using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmationPanel : MonoBehaviour
{
    [SerializeField] private Button yesButton, noButton;
    [SerializeField] private TMP_Text confirmationText;
    public delegate void Confirmation();
    public static event Confirmation YesButtonClicked, NoButtonClicked;
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
        NoButtonClicked?.Invoke();
        gameObject.SetActive(false);
    }
}
