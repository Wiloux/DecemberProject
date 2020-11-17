using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class Ward : MonoBehaviour
{
   public float workLeft;
    public float workAmount;

    public float revealTime;

    public bool isBeingWorkedOn;

    public GameObject PointerPrefab;
    GameObject Pointer;
    Quaternion PointerQ;

    GameObject canvas;

    void Start()
    {
        canvas = FindObjectOfType<Canvas>().gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (workLeft > 0 && !isBeingWorkedOn)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Killer")
        {
            Pointer = Instantiate(PointerPrefab, canvas.transform);
            StartCoroutine(RevealKiller(other.gameObject.GetComponent<Enemy>()));
        }
    }


    IEnumerator RevealKiller(Enemy killerScript)
    {
        float time = 0;
        while (time <= 1f) {
            Vector3 direction = CameraScript.instance.target.transform.position - killerScript.transform.position;

            PointerQ = Quaternion.LookRotation(direction);
            PointerQ.z = -PointerQ.y;
            PointerQ.x = 0;
            PointerQ.y = 0;

            Vector3 North = new Vector3(0, 0, GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles.y);
            Pointer.transform.localRotation = PointerQ * Quaternion.Euler(North);
            time += Time.deltaTime / revealTime;
            yield return null;
        }
        Destroy(Pointer);
        Destroy(gameObject);
    }
}
