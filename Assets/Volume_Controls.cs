using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume_Controls : MonoBehaviour
{
    public Slider thisSlider;
    public float musicVolume;
    public float sfxVolume;

    public void SetSpecificVolume(string whatValue)
    {
        float sliderValue = thisSlider.value;

        if (whatValue == "Music Slider")
        {
            musicVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("MusicVolume", musicVolume);
        }
        if (whatValue == "SFX Slider")
        {
            musicVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("SFXVolume", sfxVolume);
        }
    }
}
