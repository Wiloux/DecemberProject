using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVs : MonoBehaviour
{
    public List<TVClass> Tvs;

    void Start()
    {
        {
            foreach (Transform child in transform)
            {
                TVClass newclass = new TVClass();
                newclass.tvPos = child.transform;
                newclass.tvGameObject = child.transform.gameObject;
                newclass.objective = newclass.tvGameObject.GetComponent<Objective>();
                Tvs.Add(newclass);
                child.gameObject.SetActive(false);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

    }


}

[System.Serializable]
public class TVClass
{
    public Transform tvPos;
    public GameObject tvGameObject;
    public Objective objective;
    public Movement survivor;
}
