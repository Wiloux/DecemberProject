using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    public float workLeft;
    public float workAmount;

    public float stunTime;

    public bool isBeingWorkedOn;


    public Movement player;
    // Update is called once per frame
    private void Start()
    {
        AkSoundEngine.PostEvent("BeartrapSetup", gameObject);
    }
    void Update()
    {

        if (player.currentState == Movement.States.Corrupted)
        {
            AkSoundEngine.PostEvent("BeartrapStop", gameObject);
            Destroy(gameObject);
        }



        if (workLeft > 0 && !isBeingWorkedOn)
        {
            AkSoundEngine.PostEvent("BeartrapStop", gameObject);
            Destroy(gameObject);
        }
        else if (workLeft <= 0)
        {
            AkSoundEngine.PostEvent("BeartrapStop", gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Killer")
        {
            AkSoundEngine.PostEvent("BeartrapTrigger", gameObject);
            StartCoroutine(LockKiller(other.gameObject.GetComponent<Enemy>()));
        }
    }


    IEnumerator LockKiller(Enemy killerScript)
    {
        killerScript.agent.isStopped = true;
        yield return new WaitForSeconds(stunTime);
        killerScript.agent.isStopped = false;
        Destroy(gameObject);
    }
}
