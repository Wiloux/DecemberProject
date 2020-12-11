using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    public Transform target;
    public Movement CurrentTarget;
    Movement LastTarget;
    private Vector3 camOffSet;

    public List<Movement> Players;

    public float CameraSpeed;

    public GameObject CamLockedImg;

    public Texture2D activeCursor;
    public Texture2D unActiveCursor;
    //  public GameObject cursor;

    [Range(0.01f, 1f)]
    public float smoothness = 0.5f;

    public static CameraScript instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        camOffSet = transform.position - target.transform.position;
        if (EasterEgg.instance.isNightStalker)
        {
            Players[0] = EasterEgg.instance.NightStalker.GetComponent<Movement>();
        }
        else
        {
            Players[0] = EasterEgg.instance.Syd.GetComponent<Movement>();
        }
    }

    public bool isLocked = false;
    public Vector2 maxCamPos;

    public int margin;

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.instance.GamePaused)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isLocked = !isLocked;
                CamLockedImg.SetActive(isLocked);
            }
        }
        //    cursor.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (CurrentTarget != null)
        {
            if (CurrentTarget.hover)
            {
                Cursor.SetCursor(activeCursor, Vector2.zero, CursorMode.ForceSoftware);
            }
            else if (!CurrentTarget.hover)
            {
                Cursor.SetCursor(unActiveCursor, Vector2.zero, CursorMode.ForceSoftware);
            }
        }
        else
        {
            Cursor.SetCursor(unActiveCursor, Camera.main.ScreenToWorldPoint(Input.mousePosition), CursorMode.ForceSoftware);
        }


        if (isLocked)
        {
            if (target != null && target.GetComponent<Movement>().currentState != Movement.States.Dead)
            {
                Vector3 newPos = target.position + camOffSet;
                transform.position = Vector3.Slerp(transform.position, newPos, smoothness);
            }
        }
        else
        {
            Vector2 mouseEdge = MouseScreenEdge(margin);
            Vector3 dir = new Vector3();
            bool isDiag = false;

            if (!(Mathf.Approximately(mouseEdge.x, 0f)))
            {
                if (!(Mathf.Approximately(mouseEdge.y, 0f)))
                {
                    //Move your camera depending on the sign of mouse.Edge.y

                    if (mouseEdge.y < 0 && transform.position.z > -maxCamPos.y)
                    {
                        dir += new Vector3(0, 0, -1);
                    }
                    else if (mouseEdge.y > 0 && transform.position.z < maxCamPos.y)
                    {
                        dir += new Vector3(0, 0, 1);
                    }
                    //Move your camera depending on the sign of mouse.Edge.x

                    if (mouseEdge.x < 0 && transform.position.x > -maxCamPos.x)
                    {
                        dir += new Vector3(-1, 0, 0);
                    }
                    else if (mouseEdge.x > 0 && transform.position.x < maxCamPos.x)
                    {
                        dir += new Vector3(1, 0, 0);
                    }
                    isDiag = true;
                    Camera.main.transform.position += dir * CameraSpeed * Time.deltaTime;
                } 
            }

            if (!isDiag)
            {
                if (!(Mathf.Approximately(mouseEdge.x, 0f)))
                {
                    //Move your camera depending on the sign of mouse.Edge.y

                    if (mouseEdge.x < 0 && transform.position.x > -maxCamPos.x)
                    {
                        dir += new Vector3(-1, 0, 0);
                    }
                    else if (mouseEdge.x > 0 && transform.position.x < maxCamPos.x)
                    {
                        dir += new Vector3(1, 0, 0);
                    }
                    Camera.main.transform.position += dir * CameraSpeed * Time.deltaTime * 2;
                }

                if (!(Mathf.Approximately(mouseEdge.y, 0f)))
                {
                    //Move your camera depending on the sign of mouse.Edge.y
                    if (mouseEdge.y < 0 && transform.position.z > -maxCamPos.y)
                    {
                        dir += new Vector3(0, 0, -1);
                    }
                    else if (mouseEdge.y > 0 && transform.position.z < maxCamPos.y)
                    {
                        dir += new Vector3(0, 0, 1);
                    }

                    Camera.main.transform.position += dir * CameraSpeed * Time.deltaTime * 2;
                }
            }

            Camera.main.transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime * CameraSpeed;
        }



        if (!PauseMenu.instance.GamePaused)
        {
            //Switch Focus
            if (Input.GetKey(KeyCode.F1))
            {
                SwitchTarget(0);
            }

            if (Input.GetKey(KeyCode.F2))
            {
                SwitchTarget(1);
            }
            if (Input.GetKey(KeyCode.F3))
            {
                SwitchTarget(2);
            }

        }

        if (!PauseMenu.instance.GamePaused)
        {
            //Select Character
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                {
                    if (hit.transform.gameObject.tag == "Player")
                    {
                        if (CurrentTarget != null)
                        {
                            CurrentTarget.isSelected = false;
                            CurrentTarget.wantsToUseItem = false;
                            LastTarget = CurrentTarget;
                        }
                        CurrentTarget = hit.transform.gameObject.GetComponent<Movement>();
                        CurrentTarget.isSelected = true;

                        target = Players[CurrentTarget.playerInt].transform;
                        if (LastTarget != null && LastTarget.currentItem != Movement.Items.None)
                        {
                            if (LastTarget.currentItemGhost != null)
                            {
                                Destroy(LastTarget.currentItemGhost.gameObject);
                                LastTarget.currentItemGhost = null;
                            }
                        }

                        if (LastTarget != null && LastTarget.wantsToUseItem && CurrentTarget.currentItem != Movement.Items.None)
                        {
                            CurrentTarget.wantsToUseItem = true;
                        }
                    }
                }
            }
        }
    }

    public void SwitchTarget(int i)
    {
        target = Players[i].transform;

        if (target.GetComponent<Movement>().currentState == Movement.States.Dead)
        {
            target = null;
        }
        else
        {
            if (CurrentTarget != null)
            {
                CurrentTarget.isSelected = false;
                CurrentTarget.wantsToUseItem = false;
                LastTarget = CurrentTarget;
            }

            CurrentTarget = Players[i];
            CurrentTarget.isSelected = true;

            if (LastTarget != null && LastTarget.currentItem != Movement.Items.None)
            {
                if (LastTarget.currentItemGhost != null)
                {
                    Destroy(LastTarget.currentItemGhost.gameObject);
                    LastTarget.currentItemGhost = null;
                }
            }

            if (LastTarget != null && LastTarget.wantsToUseItem && CurrentTarget.currentItem != Movement.Items.None)
            {
                CurrentTarget.wantsToUseItem = true;
            }
        }
    }
    public List<int> tap = new List<int>();
    //  int playerInt;
    float interval = 0.5f;
    Coroutine doubletap;

    public void SelectWithUI(int PlayerInt)
    {
        tap[PlayerInt]++;
        if (tap[PlayerInt] == 1)
        {
            if (CurrentTarget != null)
            {
                CurrentTarget.isSelected = false;
                CurrentTarget.wantsToUseItem = false;
                LastTarget = CurrentTarget;
            }
            if (LastTarget != null && LastTarget.currentItem != Movement.Items.None)
            {
                if (LastTarget.currentItemGhost != null)
                {
                    Destroy(LastTarget.currentItemGhost.gameObject);
                    LastTarget.currentItemGhost = null;
                }
            }

            if (LastTarget != null && LastTarget.wantsToUseItem && CurrentTarget.currentItem != Movement.Items.None)
            {
                CurrentTarget.wantsToUseItem = true;
            }
            CurrentTarget = Players[PlayerInt];
            target = Players[PlayerInt].transform;
            //playerInt = PlayerInt;
            CurrentTarget.isSelected = true;
            if (doubletap == null)
            {
                doubletap = StartCoroutine(DoubleTapInterval(PlayerInt));
            }
        }

        else if (tap[PlayerInt] > 1)
        {
            target = Players[PlayerInt].transform;
            if (CurrentTarget != null)
                CurrentTarget.isSelected = false;

            CurrentTarget = Players[PlayerInt];
            CurrentTarget.isSelected = true;
            isLocked = true;
            CamLockedImg.SetActive(isLocked);
            tap[PlayerInt] = 0;
        }
    }



    Vector2 MouseScreenEdge(int margin)
    {
        //Margin is calculated in px from the edge of the screen

        Vector2 half = new Vector2(Screen.width / 2, Screen.height / 2);

        //If mouse is dead center, (x,y) would be (0,0)
        float x = Input.mousePosition.x - half.x;
        float y = Input.mousePosition.y - half.y;

        //If x is not within the edge margin, then x is 0;
        //In another word, not close to the edge
        if (Mathf.Abs(x) > half.x - margin)
        {
            x += (half.x - margin) * ((x < 0) ? 1 : -1);
        }
        else
        {
            x = 0f;
        }

        if (Mathf.Abs(y) > half.y - margin)
        {
            y += (half.y - margin) * ((y < 0) ? 1 : -1);
        }
        else
        {
            y = 0f;
        }

        return new Vector2(x, y);
    }

    IEnumerator DoubleTapInterval(int PlayerInt)
    {
        yield return new WaitForSeconds(interval);
        tap[PlayerInt] = 0;
        doubletap = null;
    }

}