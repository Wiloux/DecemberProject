using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NightStalkerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject NSOption;
    public Toggle toggle;
    void Start()
    {
     //   PlayerPrefs.SetInt("NightStalkerUnlocked", 0);
        if (PlayerPrefs.GetInt("NightStalkerModeOn") == 0)
        {
            toggle.isOn = false;
        }
        else
        {
            toggle.isOn = true;
        }

        if (PlayerPrefs.GetInt("NightStalkerUnlocked") != 1)
        {
            NSOption.SetActive(false);
        }
        else
        {
            NSOption.SetActive(true);
        }

    }

    public void NightStalkerMode(bool On)
    {
        if (On)
        {
            PlayerPrefs.SetInt("NightStalkerModeOn", 1);
        }
        else
        {
            PlayerPrefs.SetInt("NightStalkerModeOn", 0);

        }
    }

  
    // Update is called once per frame
    void Update()
    {

    }
}
