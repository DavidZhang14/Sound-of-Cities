using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public delegate void MusicControl();
    public static event MusicControl NewGridReached;
    void Start()
    {
        AkSoundEngine.PostEvent("Main_Loop", this.gameObject, (uint)AkCallbackType.AK_MusicSyncGrid, callbackFunction, null);
    }

    private void callbackFunction(object in_cookie, AkCallbackType in_type, object in_info) {
        if (NewGridReached != null)
        NewGridReached();
    }
}
