using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadAScene(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Open_Close(GameObject Target)
    {
        if (Target.activeInHierarchy)
        {
            Target.SetActive(false);
        }
        else
        {
            Target.SetActive(true);
        }
    }
}
