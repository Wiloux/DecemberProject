using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    public List<Objective> Objectives = new List<Objective>();

    public List<SpottedSurvior> Players = new List<SpottedSurvior>();
    public float ChaseDur;
    public Transform CurrentTransformObjective;
    public SpottedSurvior CurrentTransformPlayer;
    public float MiniDistance = 2f;
    public float CheckRadius;

    public LayerMask PlayerLayer;

    public enum States { Patrolling, Chasing, Pause }
    public States currentState = States.Patrolling;

    [HideInInspector]
    public NavMeshAgent agent;

    public Light chaseLight;

    private RaycastHit hit;

    public float CorruptRange;

    public float waitAfterCorruptTime;
    float currentWaitAfterCorruptTime;

    Animator anim;

    public List<Material> IdleMat;
    public List<Material> ChaseMat;
    public List<Material> CorruptMat;

    public float spd;
    public GameObject mesh;

    public TVs tvscript;


    void Start()
    {

        tvscript = GameObject.FindGameObjectWithTag("TV").GetComponent<TVs>();
        set_skinned_mat(mesh, 2, IdleMat[Random.Range(0, IdleMat.Count)]);

        foreach (Objective go in FindObjectsOfType(typeof(Objective)))
        {
            if (go.gameObject.GetComponent<Movement>() == false)
            {
                if (go.gameObject.GetComponent<Objective>().Type != Objective.WorkType.chest)
                    Objectives.Add(go);
            }
        }

        foreach (Movement Player in FindObjectsOfType(typeof(Movement)))
        {
            SpottedSurvior newSurvi = new SpottedSurvior();
            newSurvi.PlayerScript = Player;
            Players.Add(newSurvi);
        }

        chaseLight = GetComponentInChildren<Light>();
        agent = GetComponent<NavMeshAgent>();
        chaseLight.color = Color.yellow;
        CurrentTransformObjective = Objectives[Random.Range(0, Objectives.Count)].transform;
        agent.speed = spd;
        anim = GetComponent<Animator>();
    }
    public float slowdownLength = 2f;
    float t = 1f;
    // Update is called once per frame
    void Update()
    {


        if (t < 1)
        {
            t += slowdownLength * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(0, 1, t); // notice we are using the unscaledDelta now
                                                  //     Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);
        }

        foreach (Objective go in Objectives)
        {
            if (go.isFinished)
            {
                Objectives.Remove(go);
            }
        }

        anim.SetBool("stopped", agent.isStopped);
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
        switch (currentState)
        {
            case States.Pause:
                if (currentWaitAfterCorruptTime >= 0)
                {
                    agent.isStopped = true;
                    currentWaitAfterCorruptTime -= Time.deltaTime;
                }
                else
                {
                    CurrentTransformPlayer = null;
                    set_skinned_mat(mesh, 2, IdleMat[Random.Range(0, IdleMat.Count)]);
                    currentState = States.Patrolling;
                    chaseLight.color = Color.yellow;
                    agent.isStopped = false;
                }
                break;
            case States.Patrolling:

                Objective currentScriptObj = new Objective();
                if (Vector3.Distance(transform.position, CurrentTransformObjective.position) < MiniDistance) {

                    CurrentTransformObjective = Objectives[Random.Range(0, Objectives.Count)].transform;
                    currentScriptObj = CurrentTransformObjective.GetComponent<Objective>();
                }

                foreach (SpottedSurvior Player in Players)
                {
                    if (Player.PlayerScript.ChaseMe && Player.PlayerScript.currentState != Movement.States.Corrupted && Player.PlayerScript.currentState != Movement.States.Dead)
                    {
                        t = 0;
                        Player.isSpotted = true;
                        Player.Timer = ChaseDur;
                        CurrentTransformPlayer = Player;
                        if (CameraScript.instance.CurrentTarget != null)
                        {
                            CameraScript.instance.CurrentTarget.isSelected = false;
                            CameraScript.instance.CurrentTarget = Player.PlayerScript;
                        }
                        CameraScript.instance.isLocked = true;
                        CameraScript.instance.CamLockedImg.SetActive(true);
                        CameraScript.instance.target = Player.PlayerScript.transform;

                        Player.PlayerScript.isSelected = true;
                        currentState = States.Chasing;
                        set_skinned_mat(mesh, 2, ChaseMat[Random.Range(0, ChaseMat.Count)]);
                        chaseLight.color = Color.red;
                    }
                }



                agent.SetDestination(CurrentTransformObjective.position);


                break; 
            case States.Chasing:

                //Timer when Player is off radar
                bool NoMoreSpotted = true;


                agent.speed += 0.03f * Time.deltaTime;

                //Corrupts
                if (CurrentTransformPlayer.isSpotted && !CurrentTransformPlayer.isCorrupted && Vector3.Distance(CurrentTransformPlayer.PlayerScript.transform.position, transform.position) < CorruptRange)
                {
                    CurrentTransformPlayer.PlayerScript.PlayWwiseEvent("StopJerrycan");
                    CurrentTransformPlayer.PlayerScript.PlayWwiseEvent("PlayStatic");
                    CurrentTransformPlayer.PlayerScript.corruptedTimes++;
                    CurrentTransformPlayer.PlayerScript.currentState = Movement.States.Corrupted;
                    CurrentTransformPlayer.PlayerScript.disolveAmount = CurrentTransformPlayer.PlayerScript.disolveAmount / CurrentTransformPlayer.PlayerScript.corruptedTimes;
                    CurrentTransformPlayer.PlayerScript.currDisolveAmount = CurrentTransformPlayer.PlayerScript.disolveAmount;
                    CurrentTransformPlayer.PlayerScript.transform.LookAt(transform.position, Vector3.left);
                    transform.LookAt(CurrentTransformPlayer.PlayerScript.transform.position, Vector3.left);
                    CurrentTransformPlayer.isCorrupted = true;
                    CurrentTransformPlayer.isSpotted = false;
                    CurrentTransformPlayer.PlayerScript.ChaseTimer = 0;
                    currentWaitAfterCorruptTime = waitAfterCorruptTime;
                    agent.speed = spd;

                    if (CurrentTransformPlayer.PlayerScript.readytoadd)
                    {
                        CurrentTransformPlayer.PlayerScript.currentObjective.Workers--;
                        CurrentTransformPlayer.PlayerScript.readytoadd = false;
                    }

                    if (CurrentTransformPlayer.PlayerScript.ChoiceTV.survivor == null)
                    {
                        TVClass newtv = tvscript.Tvs[Random.Range(0, tvscript.Tvs.Count)];
                        tvscript.Tvs.Remove(newtv);
                        CurrentTransformPlayer.PlayerScript.ChoiceTV = newtv;
                        Material[] mats = newtv.tvGameObject.GetComponent<Renderer>().materials;
                        mats[2] = tvscript.pMat[CurrentTransformPlayer.PlayerScript.playerInt];
                        newtv.tvGameObject.GetComponent<Renderer>().materials = mats;                      
                        newtv.survivor = CurrentTransformPlayer.PlayerScript;
                    }
                    currentState = States.Pause;
                    set_skinned_mat(mesh, 2, CorruptMat[Random.Range(0, CorruptMat.Count)]);
                    break;
                }

                if (CurrentTransformPlayer.isCorrupted)
                {
                    if (CurrentTransformPlayer.PlayerScript.currentState != Movement.States.Corrupted)
                        CurrentTransformPlayer.isCorrupted = false;
                }

                foreach (SpottedSurvior Player in Players)
                {
                    if (Player.PlayerScript.ChaseTimer <= 0)
                    {
                        //      Debug.Log("Stops Chase : " + Player.PlayerScript.transform.name);
                        Player.isSpotted = false;
                    }

                    if (Player.isSpotted)
                    {
                        NoMoreSpotted = false;
                    }
                }
                //Checks if should go back to Patroll
                if (NoMoreSpotted)
                {
                    //     Debug.Log("Going Back to Patroll");
                    CurrentTransformPlayer = null;
                    currentState = States.Patrolling;
                    agent.speed = spd;
                    set_skinned_mat(mesh, 2, IdleMat[Random.Range(0, IdleMat.Count)]);
                    chaseLight.color = Color.yellow;
                }
                //Checks for new Main Target 
                else
                {
                    if (!CurrentTransformPlayer.isSpotted || CurrentTransformPlayer.isCorrupted)
                    {
                        float BestDistance = 1000f;
                        foreach (SpottedSurvior Player in Players)
                        {
                            if (Vector3.Distance(transform.position, Player.PlayerScript.transform.position) <= BestDistance && !Player.isCorrupted)
                            {
                                BestDistance = Vector3.Distance(transform.position, Player.PlayerScript.transform.position);
                                CurrentTransformPlayer = Player;
                                Debug.Log("New Target :" + Player.PlayerScript.transform.name);
                            }
                        }
                    }
                }


                //Checks for better target when searching for a while
                //if (CurrentTransformPlayer.Timer <= ChaseDur / 2)
                //{
                //    foreach (SpottedSurvior Player in Players)
                //    {
                //        if (Player.isSpotted && Player != CurrentTransformPlayer)
                //        {
                //            if (Vector3.Distance(transform.position, Player.PlayerScript.transform.position) <= Vector3.Distance(CurrentTransformPlayer.PlayerScript.transform.position, transform.position))
                //            {
                //                Debug.Log("New Target :" + Player.PlayerScript.transform.name + " (too close)");
                //                CurrentTransformPlayer = Player;
                //            }
                //        }
                //    }
                //}

                if (CurrentTransformPlayer != null)
                {
                    agent.SetDestination(CurrentTransformPlayer.PlayerScript.transform.position);
                }
                break;

        }
    }

    //bool CheckIfinRadius(SpottedSurvior Surv = null)
    //{
    //    Collider[] hitColls = Physics.OverlapSphere(transform.position, CheckRadius, PlayerLayer);
    //    for (var i = 0; i < hitColls.Length; i++)
    //    {
    //        Debug.Log(hitColls[i]);
    //        if (Surv != null)
    //        {
    //            if (hitColls[i].GetComponent<Movement>() == Surv.PlayerScript)
    //            {
    //                return hitColls[i];
    //            }
    //        }
    //        else if (Surv == null)
    //        {
    //            return hitColls[i];
    //        }
    //    }
    //    return false;
    //}

    void OnDrawGizmos()
    {


        if (currentState == States.Chasing)
        {
            if (!Physics.Linecast(transform.position, CurrentTransformPlayer.PlayerScript.transform.position))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, CurrentTransformPlayer.PlayerScript.transform.position);
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, CorruptRange);
        }
        else
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, CheckRadius);
        }
    }

    void set_skinned_mat(GameObject obj, int Mat_Nr, Material Mat)
    {

        SkinnedMeshRenderer renderer = obj.GetComponentInChildren<SkinnedMeshRenderer>();

        Material[] mats = renderer.materials;

        mats[Mat_Nr] = Mat;

        renderer.materials = mats;
    }

    [System.Serializable]
    public class SpottedSurvior
    {
        public Movement PlayerScript;
        public bool isSpotted;
        public bool isCorrupted;
        public float Timer;
    }
}
