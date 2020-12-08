using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool GamePaused;

    public static PauseMenu instance;
    public GameObject PauseMenuPanel;
    public GameObject SurvUI;
    // Update is called once per frame

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
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
    }

}


