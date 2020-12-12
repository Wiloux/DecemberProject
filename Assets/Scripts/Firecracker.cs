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
        AkSoundEngine.PostEvent("FireworksSetup", gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {



            if (workLeft > 0 && !isBeingWorkedOn)
            {
                AkSoundEngine.PostEvent("FireworksStop", gameObject);
                Destroy(gameObject);
            }

            if (player.currentState == Movement.States.Corrupted)
            {
                AkSoundEngine.PostEvent("FireworksStop", gameObject);
                Destroy(gameObject);
            }


            if (workLeft <= 0)
            {

                AkSoundEngine.PostEvent("FireworksStop", gameObject);
                if (player.currentObjective != null && player.currentObjective.isBeingWorkedOn && !isActive)
                {

                    if (killer.currentState == Enemy.States.Patrolling)
                    {
                        AkSoundEngine.PostEvent("FireworksTrigger", gameObject);
                        particles.SetActive(true);
                        isActive = true;
                        killer.CurrentTransformObjective = transform;
                    }
                }
            }

            if (isActive && player.currentFirecracker != null)
            {
                Destroy(gameObject);
            }

            if (isActive && killer.CurrentTransformObjective != this.transform)
            {

                Destroy(gameObject);
            }
        }
        else
        {
            AkSoundEngine.PostEvent("FireworksStop", gameObject);
            Destroy(gameObject);
        }
    }
}
