using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicState : MonoBehaviour
{

    public string StateGroup = "Overworld";
    public string State = "Tense";
    public string ExitState = "NonTense";
    public GameObject Player;
    public bool Debug_Enabled;

    private void OnTriggerEnter(Collider collision)
    {
        if (Debug_Enabled)
        {
            //        Debug.Log(State + "state set"); 
        }
        if (collision.gameObject.tag == "Player")
        {
            AkSoundEngine.SetState(StateGroup, State);
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (Debug_Enabled)
        {
            //        Debug.Log(ExitState + "state set"); 
        }
        if (collision.gameObject.tag == "Player")
        {
            AkSoundEngine.SetState(StateGroup, ExitState);
        }
    }
}