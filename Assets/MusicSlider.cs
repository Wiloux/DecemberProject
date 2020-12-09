using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider Slider1; 
    public Slider Slider2;

    public Image Img1;
    public Image Img2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Img1.fillAmount = Slider1.value;
        Img2.fillAmount = Slider2.value;   
    }
}
