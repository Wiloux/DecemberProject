using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSoundEmitter : MonoBehaviour
{
    public void PlayOrbSound()
    { 
        {
        AkSoundEngine.PostEvent("PlayOrb", gameObject);
        }
    }
}
