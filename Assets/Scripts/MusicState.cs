using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicState : MonoBehaviour
{

    public string StateGroup = "Overworld";
    public string State = "Tense";
    public string ExitState = "NonTense";
    public List<Movement> Players;
    public GameObject SelectedPlayer;
    public float CheckDistance;


    private void Start()
    {
        AkSoundEngine.SetState(StateGroup, ExitState);

        foreach (Movement Player in FindObjectsOfType(typeof(Movement)))
        {
            Players.Add(Player);
        }

    }


    private void Update()
    {
        foreach(Movement Player in Players)
        {
            if (Player.isSelected)
            {
                SelectedPlayer = Player.gameObject;
            }
        }



        if(SelectedPlayer != null)
        {
            if(Vector3.Distance(SelectedPlayer.transform.position, transform.position) >= CheckDistance)
            {
                AkSoundEngine.SetState(StateGroup, ExitState);
                
            }
            else
            {
                AkSoundEngine.SetState(StateGroup, State);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, CheckDistance);
    }

    //private void OnTriggerEnter(Collider collision)
    //{
    //    if (Debug_Enabled)
    //    {
    //        //        Debug.Log(State + "state set"); 
    //    }
    //    if (collision.gameObject.tag == "Player")
    //    {

    //    }
    //}
    //private void OnTriggerExit(Collider collision)
    //{

    //    if (collision.gameObject.tag == "Player")
    //    {
    //        AkSoundEngine.SetState(StateGroup, ExitState);
    //    }
    //}
}