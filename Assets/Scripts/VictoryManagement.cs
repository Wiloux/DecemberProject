using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryManagement : MonoBehaviour
{

    Animator anim;
    LoadScene loadSceneScript;
    public List<Movement> Players= new List<Movement>();
    Objective car;
    // Start is called before the first frame update
    private void Awake()
    {
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        loadSceneScript = GetComponent<LoadScene>();

        foreach (Movement Player in FindObjectsOfType(typeof(Movement)))
        {
            Players.Add(Player);
        }


        foreach (Objective go in FindObjectsOfType(typeof(Objective)))
        {
            if (go.Type == Objective.WorkType.car)
            {
                car = go;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        foreach (Movement p in Players)
        {
            if(p.currentState == Movement.States.Corrupted || p.currentState == Movement.States.Dead)
            {
                i++;
            }

            if (i >= 3)
            {
                anim.SetTrigger("Lost");
             
            }
        }

         //   anim.SetTrigger("Victory");

    }

    public void StaticGoesHere()
    {
        AkSoundEngine.PostEvent("GameOverFadeOut", gameObject);
    }
}
