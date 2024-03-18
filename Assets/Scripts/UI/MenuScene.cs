using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuScene : MonoBehaviour
{
    [SerializeField] private Button singlePlayerButton, hostButton, clientButton;
    [SerializeField] private TMP_InputField ipInputField;
    private void Start() {
        singlePlayerButton.onClick.AddListener(() => {
            GameManager.singlePlayer = true;
            // Load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
        hostButton.onClick.AddListener(() => 
        {
            GameManager.singlePlayer = false;
            GameManager.clientMode = false;
            // Load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });

        clientButton.onClick.AddListener(() =>
        {
            GameManager.singlePlayer = false;
            GameManager.clientMode = true;
            GameManager.joinCode = ipInputField.text;
            // Load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
    }
}
