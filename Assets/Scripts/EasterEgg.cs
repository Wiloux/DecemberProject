using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    public bool isNightStalker;


    public static EasterEgg instance;

    public GameObject NightStalker;
    public GameObject Syd;

    private void Awake()
    {
        instance = this;
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


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
