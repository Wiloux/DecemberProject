using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class Firecracker : MonoBehaviour
{
    public float workLeft;
    public float workAmount;

    public Enemy killer;
    public Movement player;
    public bool isActive;

    AudioSource audioSource;
    public AudioClip clip;
    public bool isBeingWorkedOn;

    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (workLeft > 0 && !isBeingWorkedOn)
        {
            Destroy(gameObject);
        }
        if (workLeft <= 0)
        {
            if (player.currentObjective.isBeingWorkedOn && !isActive)
            {
                if (killer.currentState == Enemy.States.Patrolling)
                {
                    audioSource = GetComponent<AudioSource>();
                    isActive = true;
                    audioSource.PlayOneShot(clip);
                    killer.CurrentTransformObjective = transform;
                    Debug.Log("boom");
                }
            }
        }

        if (isActive && killer.CurrentTransformObjective != this.transform)
        {
            Destroy(gameObject);
        }
    }
}
