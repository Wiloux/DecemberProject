using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public enum WorkType { jerryCan, car, trap, player, chest, tv }

    public bool isBeingWorkedOn;
    public bool isFinished;
    public WorkType Type;
    public float WorkLeft;
    public float WorkNeeded;
    public float MiniDistance;

    public int nbOfPlayerWorking;
    public List<GameObject> souls = new List<GameObject>();

    public int Workers = 0;

    Animator anim;

    public ParticleSystem PS;


    private void Start()
    {
        if (GetComponentInChildren<ParticleSystem>().transform.name != "PixelParticle")
            PS = GetComponentInChildren<ParticleSystem>();

        switch (Type)
        {
            case WorkType.jerryCan:
                WorkNeeded = 150f;
                WorkLeft = WorkNeeded;
                anim = GetComponent<Animator>();
                break;
            case WorkType.car:
                WorkNeeded = 50f;
                WorkLeft = WorkNeeded;
                break;
            case WorkType.trap:
                WorkNeeded = 150f;
                WorkLeft = WorkNeeded;
                break;
            case WorkType.player:
                WorkNeeded = GetComponent<Movement>().helpAmount;
                WorkLeft = WorkNeeded;
                break;
            case WorkType.chest:
                WorkNeeded = 35f;
                WorkLeft = WorkNeeded;
                break;
            case WorkType.tv:
                WorkNeeded = 75f;
                WorkLeft = WorkNeeded;
                break;
        }
    }

    private void Update()
    {

        if (PS != null)
        {
            if (!isBeingWorkedOn && !isFinished)
            {
                if (Type != WorkType.car)
                {
                    PS.Play();
                }
                else
                {
                    if (CameraScript.instance.CurrentTarget != null && CameraScript.instance.CurrentTarget.hasSoul)
                    {
                        PS.Play();
                    }
                    else
                    {
                        PS.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
                    }
                }
            }
            else
            {
                PS.Stop();
            }

        }

        if (!isFinished)
        {
            if (Type == WorkType.jerryCan)
            {
                anim.Play("Open", 0, 1 - (WorkLeft / WorkNeeded));
            }
        }
        else
        {
            if (Type == WorkType.jerryCan)
            {
                anim.SetBool("Openned", true);
            }
        }

        if (Type == WorkType.car && Workers >= 1)
        {
            souls[0].SetActive(true);
            if (Workers >= 2)
            {
                souls[1].SetActive(true);
            }
        }

        if (Type == WorkType.car && Workers == 3)
        {
            WorkLeft -= 2 * Time.deltaTime;
            souls[2].SetActive(true);
            if (WorkLeft <= 0)
            {
                Debug.Log("YouWin");
            }
        }
    }
}
