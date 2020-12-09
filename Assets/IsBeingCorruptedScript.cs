using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsBeingCorruptedScript : MonoBehaviour
{
    public AK.Wwise.Event StopChase;
    // Use this for initialization.
    void PlayStopChase()
    {
        StopChase.Post(gameObject);
    }
}
