using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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

    public Material soulmat;
    public Material pmat;

    public ParticleSystem PS;

  // public MeshRenderer face;
  //  public List<Material> facemat = new List<Material>();

    public bool victory;

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

        
        if (Type == WorkType.car)
        {
            souls[0].SetActive(Workers >= 1);
            souls[1].SetActive(Workers >= 2);
            souls[2].SetActive(Workers == 3);

          //  arms[0].material = Workers >= 1 ? soulmat : pmat;
          //  arms[1].material = Workers >= 2 ? soulmat : pmat;
         //   arms[2].material = Workers >= 3 ? soulmat : pmat;
        }
        else if (Type == WorkType.car && victory)
        {
            souls[0].SetActive(false);
            souls[1].SetActive(false);
            souls[2].SetActive(false);
        }




        if (Type == WorkType.car && Workers == 3)
        {
            WorkLeft -= 2 * Time.deltaTime;

            if (WorkLeft <= 0 && !victory)
            {
                victory = true;
                PB.Play(asset);
                Debug.Log("YouWin");
            }
        }


    }

    public PlayableDirector PB;
    public PlayableAsset asset;
    void set_skinned_mat(GameObject obj, int Mat_Nr, Material Mat)
    {

        SkinnedMeshRenderer renderer = obj.GetComponentInChildren<SkinnedMeshRenderer>();

        Material[] mats = renderer.materials;

        mats[Mat_Nr] = Mat;

        renderer.materials = mats;
    }

}
