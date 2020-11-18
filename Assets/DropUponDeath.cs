using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropUponDeath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Movement>() != null && !other.GetComponent<Movement>().hasSoul)
        {
            other.GetComponent<Movement>().hasSoul = true;
            Destroy(gameObject);
            AkSoundEngine.PostEvent("StopOrb", gameObject);
        }
    }
}
