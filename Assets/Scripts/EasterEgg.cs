using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    public bool isNightStalker;

    public bool VictoryWithoutBeingSpotted = true;


    public Objective vm;
    int NightStalkerPlayerPref;

    public static EasterEgg instance;

    public GameObject NightStalker;
    public GameObject Syd;

    private void Awake()
    {
        instance = this;

       // PlayerPrefs.SetInt("NightStalkerUnlocked", 0);
        if (PlayerPrefs.GetInt("NightStalkerUnlocked") == 1)
        {
           if (PlayerPrefs.GetInt("NightStalkerModeOn") != 1)
            {
                isNightStalker = false;
            }
            else
            {
                isNightStalker = true;
            }

        }
        else
        {
            isNightStalker = false;
        }

        if (isNightStalker)
        {
            Syd.transform.position = new Vector3(100, 100, 100);
            NightStalker.SetActive(true);
            Syd.SetActive(false);
        }
        else
        {
            NightStalker.transform.position = new Vector3(100, 100, 100);
            Syd.SetActive(true);
            NightStalker.SetActive(false);
        }
    }
    void Start()
    {
        VictoryWithoutBeingSpotted = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (vm.victory && VictoryWithoutBeingSpotted)
        {
            PlayerPrefs.SetInt("NightStalkerUnlocked", 1);
        }
    }
}
