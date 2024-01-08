using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MessagePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText; 
    private const float fadeDuration = 1f;
    private const float displayDuration = 1f;
    public static MessagePanel instance;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start() {
        messageText.CrossFadeAlpha(0f, 0f, false);
        gameObject.GetComponent<Image>().CrossFadeAlpha(0f, 0f, false);
    }
    public void Message(string message) {
        StopCoroutine(nameof(FadeOutRoutine));
        messageText.text = message;
        StartCoroutine(nameof(FadeOutRoutine));
    }

    private IEnumerator FadeOutRoutine()
    {
        // Display the message
        messageText.CrossFadeAlpha(1f, 0f, false);
        gameObject.GetComponent<Image>().CrossFadeAlpha(1f, 0f, false);
        // Wait for the message to display
        yield return new WaitForSeconds(displayDuration);
        // Fade out the message
        messageText.CrossFadeAlpha(0f, fadeDuration, false);
        gameObject.GetComponent<Image>().CrossFadeAlpha(0f, fadeDuration, false);
    }
}
