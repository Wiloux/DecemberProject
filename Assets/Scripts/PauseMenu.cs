using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool GamePaused;

    public static PauseMenu instance;
    public GameObject PauseMenuPanel;
    public GameObject SurvUI;
    public GameObject GameCam;
    public GameObject PauseCam;
    // Update is called once per frame

    private void Awake()
    {
        instance = this;

        AkSoundEngine.SetState("PauseMenu", "Yes");
    }

    private void Start()
    {
        PauseCam.SetActive(GamePaused);
        GameCam.SetActive(!GamePaused);
        GamePaused = false;
        PauseMenuPanel.SetActive(GamePaused);
    }

    void Update()
    {
        Time.timeScale = GamePaused ? 0 : 1;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
    }

    public void Resume()
    {
        GamePaused = !GamePaused;
        PauseMenuPanel.SetActive(GamePaused);
        SurvUI.SetActive(!GamePaused);
        PauseCam.SetActive(GamePaused);
        GameCam.SetActive(!GamePaused);
        if (GamePaused)
        {
            AkSoundEngine.SetState("PauseMenu", "Yes");
        }
        else
        {
            AkSoundEngine.SetState("PauseMenu", "No");
        }
    }

}


