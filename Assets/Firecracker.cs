using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using UnityEngine.VFX;

public class Firecracker : MonoBehaviour
{
    public float workLeft;
    public float workAmount;

    public Enemy killer;
    public Movement player;
    public bool isActive;
    public GameObject particles;
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

            if (player.currentObjective != null && player.currentObjective.isBeingWorkedOn && !isActive)
            {
                if (killer.currentState == Enemy.States.Patrolling)
                {
                    particles.SetActive(true);
                    isActive = true;
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
