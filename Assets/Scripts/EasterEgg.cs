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
        if(PlayerPrefs.GetInt("NightStalkerUnlocked") == 1)
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
            NightStalker.SetActive(true);
            Syd.SetActive(false);
        }
        else
        {
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
        if (vm.victory)
        {
            PlayerPrefs.SetInt("NightStalkerUnlocked", 1);
        }
    }
}
