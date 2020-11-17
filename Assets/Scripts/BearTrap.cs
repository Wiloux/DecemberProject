using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    public float workLeft;
    public float workAmount;

    public float stunTime;

    public bool isBeingWorkedOn;

    // Update is called once per frame
    void Update()
    {
        if (workLeft > 0 && !isBeingWorkedOn)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Killer")
        {
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
