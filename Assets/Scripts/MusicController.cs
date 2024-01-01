using UnityEngine;

public class MusicController : MonoBehaviour
{
    public delegate void MusicControl();
    public static event MusicControl NewGridReached;
    public static MusicController instance;
    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        Reset();
    }

    private void CallbackFunction(object in_cookie, AkCallbackType in_type, object in_info) {
        NewGridReached?.Invoke();
    }
    public void Reset() {
        AkSoundEngine.PostEvent("Main_Loop", gameObject, (uint)AkCallbackType.AK_MusicSyncGrid, CallbackFunction, null);
    }
}
