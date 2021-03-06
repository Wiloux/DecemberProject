﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using TMPro;
public class Movement : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent agent;
    private float realradius;
    private float realheight;

    public bool isNinja;

    public int playerInt;

    public float rotationSpeedMovement = 0.1f;
    float rotateVelocity;
    public bool isSelected;
    public GameObject SelectedSprite;

    public enum Items { None, Trap, Ward, FireCracker }
    public Items currentItem = Items.None;

    public enum States { Walk, Working, Corrupted, Dead, Action }
    public States currentState = States.Walk;
    public bool hasSoul;
    public GameObject soulGameObject;
    public GameObject DropSoulOnDeath;

    public Objective currentObjective;

    public float workingSpeed;

    public GameObject Killer;

    [HideInInspector]
    public Enemy KillerScript;

    public float SpottedDistance;
    RaycastHit hit;
    public LayerMask ChaseLayer;

    public bool ChaseMe = false;
    public float ChaseTimer;


    private Animator anim;

    public Material[] DisolveMat;
    public float currDisolveAmount;
    public float disolveAmount;
    public float disolveSpd;
    public int corruptedTimes = 1;

    public float helpAmount;
    public bool isBeingHelped;

    public bool wantsToUseItem = false;

    Vector3 DropPos;
    Vector3 LastPossiblePos;
    NavMeshHit navHit;
    public GameObject destination;

    public GameObject lightGameObject;
    public GameObject TextGameObject;

    ParticleSystem pixelParticle;
    void Start()
    {
        //  audioSource = GetComponent<AudioSource>();
        if (playerInt == 0)
        {
            AkSoundEngine.PostEvent("PlayGetItem", gameObject);
            StartCoroutine(StartMessage());
        }
        pixelParticle = transform.Find("PixelParticle").GetComponent<ParticleSystem>();
        DisolveMat = transform.Find("Mesh").GetComponent<Renderer>().materials;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        KillerScript = GameObject.FindObjectOfType<Enemy>();
        Killer = KillerScript.gameObject;
        SpottedDistance = KillerScript.CheckRadius;
        realradius = agent.radius;
        realheight = agent.height;
        if (isNinja)
        {
            workingSpeed += 15;
            agent.speed += 5;
        }
    }

    public bool startedWorking;
    public BearTrap currentBearTrap;
    public Ward currentWard;
    public Firecracker currentFirecracker;
    public GameObject currentItemGhost;
    public LayerMask wallMask;
    public bool hover;
    public TVClass ChoiceTV;

    public TextMeshProUGUI ObjectText;

    bool mustPlaySFX;

    [HideInInspector]
    public bool readytoadd;

    // Update is called once per frame
    void Update()
    {
        if (EasterEgg.instance.vm.victory)
        {
            soulGameObject.SetActive(false);
            currentState = States.Walk;
            isSelected = false;
            SelectedSprite.SetActive(false);
        }
        else
        {
            if (currentObjective == null || currentObjective.Type != Objective.WorkType.car)
            {

                soulGameObject.SetActive(hasSoul);
            }
            else if (currentObjective.Type == Objective.WorkType.car)
            {
                soulGameObject.SetActive(false);
            }

            if (isSelected)
            {
                hover = HoverWork();
            }
            SelectedSprite.SetActive(isSelected);
            if (isSelected && currentState != States.Dead)
            {
                destination.transform.position = agent.destination;
            }
            if (currentState != States.Dead && !isNinja)
            {
                if (KillerScript.currentState == Enemy.States.Pause)
                {
                    ChaseMe = false;
                    ChaseTimer = 0;
                }
                else
                {
                    if (Physics.Linecast(new Vector3(transform.position.x, 1f, transform.position.z), Killer.transform.position, out hit, ChaseLayer))
                    {
                        if (hit.transform.name == Killer.transform.name && hit.distance <= SpottedDistance)
                        {
                            ChaseMe = true;
                            ChaseTimer = KillerScript.ChaseDur;
                        }
                        else if (hit.transform.name != Killer.transform.name && hit.distance <= SpottedDistance)
                        {
                            ChaseMe = false;
                            if (ChaseTimer >= 0)
                            {
                                ChaseTimer -= Time.deltaTime;
                            }
                        }
                    }
                    else
                    {
                        ChaseMe = false;
                    }
                }
            }


            if (currentState != States.Working)
            {
                anim.SetBool("isWorking", false);
            }

            if (agent.velocity.magnitude != 0)
            {
                anim.SetBool("isWalking", true);
            }
            else
            {
                if (currentState != States.Working)
                {
                    anim.SetBool("isWalking", false);
                }
            }

            switch (currentState)
            {
                case States.Walk:
                    agent.isStopped = false;

                    break;
                case States.Action:
                    if (Vector3.Distance(transform.position, DropPos) < 0.5f && !startedWorking)
                    {
                        startedWorking = true;
                        switch (currentItem)
                        {
                            case Items.Trap:
                                currentBearTrap = Instantiate(Resources.Load<GameObject>("BearTrap"), DropPos, Quaternion.identity).GetComponent<BearTrap>();
                                break;
                            case Items.Ward:
                                currentWard = Instantiate(Resources.Load<GameObject>("Ward"), DropPos, Quaternion.identity).GetComponent<Ward>();
                                break;
                            case Items.FireCracker:
                                currentFirecracker = Instantiate(Resources.Load<GameObject>("Firecrack"), DropPos, Quaternion.identity).GetComponent<Firecracker>();
                                break;

                        }
                    }
                    else if (startedWorking)
                    {
                        anim.SetBool("isWalking", false);
                        anim.SetBool("isWorking", true);
                        agent.ResetPath();
                        agent.isStopped = true;
                        switch (currentItem)
                        {
                            case Items.Trap:
                                currentBearTrap.isBeingWorkedOn = true;
                                currentBearTrap.workLeft -= workingSpeed * Time.deltaTime;
                                currentBearTrap.player = this;
                                if (currentBearTrap.workLeft <= 0)
                                {
                                    wantsToUseItem = false;
                                    startedWorking = false;
                                    currentBearTrap.isBeingWorkedOn = false;
                                    currentBearTrap = null;
                                    currentState = States.Walk;
                                    currentItem = Items.None;
                                    if (currentItemGhost != null)
                                    {
                                        Destroy(currentItemGhost);
                                    }
                                }
                                break;
                            case Items.Ward:
                                currentWard.isBeingWorkedOn = true;
                                currentWard.workLeft -= workingSpeed * Time.deltaTime;
                                if (currentWard.workLeft <= 0)
                                {
                                    wantsToUseItem = false;
                                    startedWorking = false;
                                    currentWard.isBeingWorkedOn = false;
                                    currentWard = null;
                                    currentState = States.Walk;
                                    currentItem = Items.None;
                                    if (currentItemGhost != null)
                                    {
                                        Destroy(currentItemGhost);
                                    }
                                }
                                break;
                            case Items.FireCracker:
                                currentFirecracker.isBeingWorkedOn = true;
                                currentFirecracker.killer = KillerScript;
                                currentFirecracker.player = this;
                                currentFirecracker.workLeft -= workingSpeed * Time.deltaTime;
                                if (currentFirecracker.workLeft <= 0)
                                {
                                    wantsToUseItem = false;
                                    startedWorking = false;
                                    currentFirecracker.isBeingWorkedOn = false;
                                    currentFirecracker = null;
                                    currentState = States.Walk;
                                    currentItem = Items.None;
                                    if (currentItemGhost != null)
                                    {
                                        Destroy(currentItemGhost);
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case States.Working:
                    if (Vector3.Distance(transform.position, GetWorkPos(currentObjective)) <= currentObjective.MiniDistance)
                    {
                        anim.SetBool("isWalking", false);
                        anim.SetBool("isWorking", true);
                        agent.ResetPath();
                        agent.isStopped = true;
                        if (currentObjective.Type != Objective.WorkType.car)
                        {
                            currentObjective.isBeingWorkedOn = true;
                            currentObjective.WorkLeft -= workingSpeed * Time.deltaTime;

                            if (currentObjective.Type == Objective.WorkType.jerryCan && !mustPlaySFX)
                            {
                                mustPlaySFX = true;
                                AkSoundEngine.PostEvent("PlayJerrycan", gameObject);
                            }

                            if (currentObjective.Type == Objective.WorkType.chest && !mustPlaySFX)
                            {
                                mustPlaySFX = true;
                                AkSoundEngine.PostEvent("FireworksSetup", gameObject);
                            }
                        }
                        else
                        {
                            if (!readytoadd)
                            {
                                currentObjective.Workers++;
                                readytoadd = true;
                            }
                        }
                        Quaternion lookAtRot = Quaternion.LookRotation(currentObjective.transform.position - transform.position);
                        float yRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAtRot.eulerAngles.y, ref rotateVelocity, rotationSpeedMovement * (Time.deltaTime * 5));
                        transform.eulerAngles = new Vector3(0, yRotation, 0);


                        if (currentObjective.WorkLeft <= 0)
                        {
                            currentObjective.isBeingWorkedOn = false;
                            if (currentObjective.Type != Objective.WorkType.car && currentObjective.Type != Objective.WorkType.player)
                                currentObjective.isFinished = true;

                            currentState = States.Walk;
                            switch (currentObjective.Type)
                            {
                                case Objective.WorkType.player:
                                    Movement move = currentObjective.GetComponent<Movement>();
                                    move.currDisolveAmount = disolveAmount;
                                    move.currentState = States.Walk;
                                    move.PlayWwiseEvent("StopStatic");
                                    move.agent.radius = realradius;
                                    move.agent.height = realheight;
                                    move.isBeingHelped = false;
                                    currentObjective.isBeingWorkedOn = false;
                                    move.anim.SetBool("isStunned", false);
                                    move.pixelParticle.Stop();
                                    for (int u = 0; u < move.DisolveMat.Length; u++)
                                    {
                                        move.DisolveMat[u].SetFloat("Vector1_DD60D333", 0);
                                    }
                                    currentObjective.WorkLeft = helpAmount;
                                    break;
                                case Objective.WorkType.jerryCan:
                                    hasSoul = true;
                                    StartCoroutine(ShowFoundObject("orb", 3, "#69ffe6"));
                                    AkSoundEngine.PostEvent("StopJerrycan", gameObject);
                                    AkSoundEngine.PostEvent("PlayOrb", gameObject);
                                    AkSoundEngine.PostEvent("PlayGetItem", gameObject);
                                    break;
                                case Objective.WorkType.chest:
                                    AkSoundEngine.PostEvent("FireworksStop", gameObject);
                                    AkSoundEngine.PostEvent("PlayGetItem", gameObject);
                                    int i = Random.Range(0, 2);
                                    if (i == 1)
                                    {
                                        currentItem = Items.Trap;
                                        StartCoroutine(ShowFoundObject("Beartrap"));
                                    }
                                    else
                                    {
                                        StartCoroutine(ShowFoundObject("Fireworks"));
                                        currentItem = Items.FireCracker;
                                    }
                                    break;
                                case Objective.WorkType.car:
                                    soulGameObject.SetActive(false);
                                    break;
                            }
                            break;
                        }
                    }
                    break;
                case States.Corrupted:
                    ChaseMe = false;
                    wantsToUseItem = false;
                    if (currentItemGhost != null)
                    {
                        Destroy(currentItemGhost);
                    }
                    anim.SetBool("isStunned", true);
                    agent.SetDestination(transform.position);
                    agent.isStopped = true;

                    if (!isBeingHelped)
                    {
                        if (currDisolveAmount > 0)
                        {
                            currDisolveAmount -= Time.deltaTime * disolveSpd;
                            for (int u = 0; u < DisolveMat.Length; u++)
                            {
                                DisolveMat[u].SetFloat("Vector1_DD60D333", ((disolveAmount / currDisolveAmount) - 1) * 0.25f);
                            }
                            pixelParticle.Play();
                        }
                        else
                        {
                            for (int h = 0; h < DisolveMat.Length; h++)
                            {
                                DisolveMat[h].SetFloat("Vector1_DD60D333", 0.1f);
                            }
                            currDisolveAmount = 0;
                            pixelParticle.Stop();

                            KillerScript.spd += 0.05f;
                            lightGameObject.SetActive(false);
                            TextGameObject.SetActive(false);
                            currentState = States.Dead;
                        }
                    }
                    break;
                case States.Dead:
                    ChaseMe = false;
                    ChoiceTV.tvGameObject.SetActive(true);
                    agent.isStopped = true;
                    transform.position = new Vector3(ChoiceTV.objective.PS.transform.position.x + 2f, ChoiceTV.objective.PS.transform.position.y, ChoiceTV.objective.PS.transform.position.z);
                    for (int u = 0; u < DisolveMat.Length; u++)
                    {
                        DisolveMat[u].SetFloat("Vector1_DD60D333", (ChoiceTV.objective.WorkLeft / ChoiceTV.objective.WorkNeeded) + 0.1f);
                    }
                    if (ChoiceTV.objective.isFinished)
                    {
                        //  KillerScript.CurrentTransformObjective = ChoiceTV.objective.PS.transform;
                        corruptedTimes = 0;
                        for (int u = 0; u < DisolveMat.Length; u++)
                        {
                            DisolveMat[u].SetFloat("Vector1_DD60D333", 0);
                        }
                        PlayWwiseEvent("StopStatic");
                        agent.radius = realradius;
                        realheight = agent.height;
                        anim.SetBool("isStunned", false);
                        currentState = States.Walk;
                        ChoiceTV.survivor = null;
                        ChoiceTV.objective.WorkLeft = ChoiceTV.objective.WorkNeeded;
                        ChoiceTV.objective.isFinished = false;
                        agent.SetDestination(transform.position);
                        lightGameObject.SetActive(true);
                        TextGameObject.SetActive(true);
                        KillerScript.tvscript.Tvs.Add(ChoiceTV);
                        ChaseMe = false;
                        ChoiceTV.tvGameObject.SetActive(false);
                        ChoiceTV.survivor = null;
                    }
                    break;

            }


            if (!PauseMenu.instance.GamePaused)
            {
                if (Input.GetKeyDown(KeyCode.E) && currentItem != Items.None && currentState != States.Corrupted && isSelected)
                {
                    if (wantsToUseItem)
                    {
                        wantsToUseItem = false;
                        //   DropPos = null;

                        if (currentItemGhost != null)
                        {
                            Destroy(currentItemGhost);
                            currentState = States.Walk;
                        }
                        if (currentBearTrap != null)
                        {
                            startedWorking = false;
                            currentBearTrap.isBeingWorkedOn = false;
                        }
                        if (currentWard != null)
                        {
                            startedWorking = false;
                            currentWard.isBeingWorkedOn = false;
                        }
                        if (currentFirecracker != null)
                        {
                            startedWorking = false;
                            currentFirecracker.isBeingWorkedOn = false;
                        }
                    }
                    else
                    {
                        switch (currentItem)
                        {
                            case Items.Trap:
                                currentState = States.Walk;
                                currentItemGhost = Instantiate(Resources.Load<GameObject>("BearTrapGhost"), DropPos, Quaternion.identity);
                                break;
                            case Items.Ward:
                                currentState = States.Walk;
                                currentItemGhost = Instantiate(Resources.Load<GameObject>("WardGhost"), DropPos, Quaternion.identity);
                                break;
                            case Items.FireCracker:
                                currentState = States.Walk;
                                currentItemGhost = Instantiate(Resources.Load<GameObject>("FirecrackGhost"), DropPos, Quaternion.identity);
                                break;
                        }

                        wantsToUseItem = true;
                    }
                }

                if (currentState != States.Dead && isSelected)
                {
                    if (wantsToUseItem && !startedWorking && currentState != States.Action)
                    {
                        RaycastHit hitt;
                        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitt, Mathf.Infinity))
                        {
                            if (hitt.transform.tag == "Ground")
                            {
                                currentItemGhost.transform.position = hitt.point;
                            }
                        }

                    }
                    else if (currentState == States.Action && currentItemGhost != null)
                    {
                        currentItemGhost.transform.position = DropPos;
                    }

                    if (Input.GetMouseButton(1) && isSelected && currentState != States.Corrupted && !wantsToUseItem)
                    {
                        if (currentBearTrap != null)
                        {
                            startedWorking = false;
                            currentBearTrap.isBeingWorkedOn = false;
                        }
                        if (currentWard != null)
                        {
                            startedWorking = false;
                            currentWard.isBeingWorkedOn = false;
                        }
                        if (currentFirecracker != null)
                        {
                            startedWorking = false;
                            currentFirecracker.isBeingWorkedOn = false;
                        }
                        RaycastHit hit;

                        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, wallMask))
                        {
                            if (hit.transform.tag == "Ground")
                            {
                                if (currentObjective != null)
                                {
                                    if (currentObjective.Type == Objective.WorkType.car)
                                    {
                                        if (readytoadd)
                                        {
                                            currentObjective.Workers--;
                                            readytoadd = false;
                                        }
                                    }

                                    if (currentObjective.Type == Objective.WorkType.jerryCan)
                                    {
                                        if (mustPlaySFX)
                                        {
                                            mustPlaySFX = false;
                                            AkSoundEngine.PostEvent("StopJerrycan", gameObject);
                                        }
                                    }

                                    if (currentObjective.Type == Objective.WorkType.chest)
                                    {
                                        if (mustPlaySFX)
                                        {
                                            mustPlaySFX = false;
                                            AkSoundEngine.PostEvent("FireworksStop", gameObject);
                                        }
                                    }
                                    currentObjective.isBeingWorkedOn = false;

                                    if (currentObjective.GetComponent<Movement>() != null)
                                    {
                                        currentObjective.GetComponent<Movement>().isBeingHelped = false;
                                    }
                                    currentObjective = null;
                                }
                                anim.SetBool("isWorking", false);
                                LastPossiblePos = hit.point;
                                if (NavMesh.SamplePosition(hit.point, out navHit, 100, -1))
                                {
                                    //     test.transform.position = navHit.position;
                                    agent.SetDestination(navHit.position);
                                }

                                Quaternion lookAtRot = Quaternion.LookRotation(hit.point - transform.position);
                                float yRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAtRot.eulerAngles.y, ref rotateVelocity, rotationSpeedMovement * (Time.deltaTime * 5));
                                transform.eulerAngles = new Vector3(0, yRotation, 0);
                                currentState = States.Walk;
                            }
                            else if (hit.transform.tag == "Objective")
                            {
                                if (hit.transform.GetComponent<Objective>() != currentObjective)
                                {
                                    if (currentObjective != null)
                                        currentObjective = null;

                                    currentObjective = hit.transform.GetComponent<Objective>();
                                    if (currentObjective.isFinished || currentObjective.isBeingWorkedOn)
                                    {
                                        currentObjective = null;
                                        return;
                                    }
                                    if (currentObjective.Type == Objective.WorkType.car && !hasSoul)
                                    {
                                        currentObjective = null;
                                        AkSoundEngine.PostEvent("StopJerrycan", gameObject);
                                        AkSoundEngine.PostEvent("FireworksStop", gameObject);
                                        return;
                                    }
                                    if (currentObjective.Type == Objective.WorkType.jerryCan && hasSoul)
                                    {
                                        currentObjective = null;
                                        return;
                                    }
                                    if (currentObjective.Type == Objective.WorkType.chest && currentItem != Items.None)
                                    {
                                        currentObjective = null;
                                        return;
                                    }

                                    if (currentObjective.Type == Objective.WorkType.jerryCan)
                                    {
                                        if (mustPlaySFX)
                                            mustPlaySFX = false;

                                    }
                                    anim.SetBool("isWorking", false);
                                    agent.SetDestination(GetWorkPos(currentObjective));
                                    currentState = States.Working;
                                }
                            }
                            else if (hit.transform.tag == "Player" && hit.transform.GetComponent<Movement>().currentState == States.Corrupted)
                            {
                                if (currentObjective != null)
                                {
                                    currentObjective.isBeingWorkedOn = false;
                                    currentObjective = null;
                                }

                                currentObjective = hit.transform.GetComponent<Objective>();
                                currentObjective.GetComponent<Movement>().isBeingHelped = false;

                                if (currentObjective.isFinished || currentObjective.isBeingWorkedOn)
                                {
                                    currentObjective = null;
                                    return;
                                }
                                AkSoundEngine.PostEvent("StopJerrycan", gameObject);
                                AkSoundEngine.PostEvent("FireworksStop", gameObject);
                                currentObjective.GetComponent<Movement>().isBeingHelped = true;
                                currentObjective.isBeingWorkedOn = true;
                                anim.SetBool("isWorking", false);
                                agent.SetDestination(GetWorkPos(currentObjective));
                                currentState = States.Working;
                            }
                        }
                    }
                    else if (Input.GetMouseButton(1) && wantsToUseItem && isSelected && currentState != States.Corrupted)
                    {
                        RaycastHit hit;

                        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, wallMask))
                        {
                            if (currentBearTrap != null)
                            {
                                startedWorking = false;
                                currentBearTrap.isBeingWorkedOn = false;
                            }

                            if (currentWard != null)
                            {
                                startedWorking = false;
                                currentWard.isBeingWorkedOn = false;
                            }
                            if (currentFirecracker != null)
                            {
                                startedWorking = false;
                                currentFirecracker.isBeingWorkedOn = false;
                            }
                            if (hit.transform.tag == "Ground")
                            {
                                if (currentObjective != null)
                                {
                                    if (currentObjective.Type == Objective.WorkType.car)
                                    {
                                        if (readytoadd)
                                        {
                                            currentObjective.Workers--;
                                            readytoadd = false;
                                        }
                                    }
                                    currentObjective.isBeingWorkedOn = false;
                                    if (currentObjective.GetComponent<Movement>() != null)
                                    {
                                        currentObjective.GetComponent<Movement>().isBeingHelped = false;
                                    }
                                    currentObjective = null;
                                }
                                agent.isStopped = false;
                                AkSoundEngine.PostEvent("StopJerrycan", gameObject);
                                AkSoundEngine.PostEvent("FireworksStop", gameObject);
                                anim.SetBool("isWorking", false);
                                anim.SetBool("isWalking", true);
                                agent.SetDestination(hit.point);
                                DropPos = hit.point;
                                Quaternion lookAtRot = Quaternion.LookRotation(hit.point - transform.position);
                                float yRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAtRot.eulerAngles.y, ref rotateVelocity, rotationSpeedMovement * (Time.deltaTime * 5));
                                transform.eulerAngles = new Vector3(0, yRotation, 0);
                                currentState = States.Action;
                            }
                        }
                    }
                }
            }
        }
    }

    //public List<AudioClip> footSFX = new List<AudioClip>();
    //public AudioSource audioSource;
    public void PlayRdmFootSound()
    {
        //int i = Random.Range(0, footSFX.Count);
        //float j = Random.Range(0.80f, 1.20f);
        //audioSource.pitch = j;
        //audioSource.PlayOneShot(footSFX[i]);
    }
    public bool HoverWork()
    {

        if (!PauseMenu.instance.GamePaused)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, wallMask))
            {
                if ((hit.transform.GetComponent<Objective>() != null))
                {
                    if (hit.transform.GetComponent<Objective>() != currentObjective)
                    {
                        Objective currentObjective = hit.transform.GetComponent<Objective>();
                        if (currentObjective.Type == Objective.WorkType.jerryCan && hasSoul)
                        { return false; }
                        if (currentObjective.isFinished || currentObjective.isBeingWorkedOn)
                        { return false; }
                        if (currentObjective.Type == Objective.WorkType.chest && currentItem != Items.None)
                        { return false; }
                        if (currentObjective.Type == Objective.WorkType.car && !hasSoul)
                        { return false; }
                        if (currentObjective.GetComponent<Movement>() != null)
                        {
                            Movement currMovement = currentObjective.GetComponent<Movement>();
                            if (currMovement != GetComponent<Movement>())
                            {
                                if (currMovement.isBeingHelped || currMovement.currentState != Movement.States.Corrupted && currMovement.currentState != Movement.States.Dead)
                                { return false; }
                            }
                            else { return false; }
                        }
                        return true;
                    }
                    return true;
                }

                return false;
            }
            return false;
        }
        return false;
    }
    Vector3 GetWorkPos(Objective currentObj)
    {
        if (currentObj.PS != null)
        {
            if (currentObjective.Type != Objective.WorkType.car)
            {
                return currentObjective.PS.transform.position;
            }
            else
            {
                if (currentObjective.Workers == 0)
                {
                    return currentObjective.PS.transform.position;
                }
                else if (currentObjective.Workers == 2)
                {
                    return new Vector3(currentObjective.PS.transform.position.x + 1f, currentObjective.PS.transform.position.y, currentObjective.PS.transform.position.z - 1f);

                }
                else
                {
                    return new Vector3(currentObjective.PS.transform.position.x - 1f, currentObjective.PS.transform.position.y, currentObjective.PS.transform.position.z - 1f);
                }
            }
        }
        else
        {
            return currentObjective.transform.position;
        }
    }

    IEnumerator StartMessage()
    {
        ObjectText.enabled = true;
        ObjectText.text = "Find 3 <color=#69ffe6>orbs</color> to overload the TV";
        yield return new WaitForSeconds(4);
        ObjectText.enabled = false;
    }

    IEnumerator ShowFoundObject(string Object, float time = 3, string color = "green")
    {
        ObjectText.enabled = true;
        ObjectText.text = "You found " + "<color="+color+">" + Object + "</color>";
        yield return new WaitForSeconds(time);
        ObjectText.enabled = false;
    }

    public void PlayWwiseEvent(string EventName)
    {
        AkSoundEngine.PostEvent(EventName, gameObject);
    }
    //private void OnDrawGizmos()
    //{
    //    if (Killer != null)
    //    {
    //        if (Physics.Linecast(new Vector3(transform.position.x, 1f, transform.position.z), Killer.transform.position, out hit))
    //        {
    //            if (hit.transform.name == Killer.transform.name && hit.distance <= SpottedDistance)
    //            {
    //                Gizmos.color = Color.yellow;
    //                Gizmos.DrawLine(new Vector3(transform.position.x, 1f, transform.position.z), Killer.transform.position);
    //            }
    //        }
    //    }
    //}
}
