using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuScene : MonoBehaviour
{
    [SerializeField] private Button hostButton, clientButton;
    [SerializeField] private TMP_InputField ipInputField;
    private void Start() {
        hostButton.onClick.AddListener(() => 
        {
            GameManager.clientMode = false;
            // Load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });

        clientButton.onClick.AddListener(() =>
        {
            GameManager.clientMode = true;
            GameManager.joinCode = ipInputField.text;
            // Load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
    }
}
