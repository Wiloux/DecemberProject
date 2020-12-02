using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwAudioEmitterScript : MonoBehaviour
{
    public string EventName = "default";
    public string StopEvent = "default";
    private bool IsInCollider = false;

    private void Start()
    {
        AkSoundEngine.RegisterGameObj(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" || IsInCollider) { return; }
        IsInCollider = true;
        AkSoundEngine.PostEvent(EventName, gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player" || !IsInCollider) { return; }
        IsInCollider = false;
        AkSoundEngine.PostEvent(StopEvent, gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player" || IsInCollider) { return; }
        IsInCollider = true;
        AkSoundEngine.PostEvent(EventName, gameObject);
    }
}
