using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SurvivorStatsUI : MonoBehaviour
{
    public Movement Player;

    private Image img;
    private Image imgWorkAmount;
    public Image currObj;
    TextMeshProUGUI infotext;
    public Animator CharaWorkBGAnim;
    RectTransform pos;
    public Sprite Syd;
    public Sprite NS;

    private void Start()
    {


        pos = GetComponent<RectTransform>();
        infotext = transform.Find("Info").GetComponent<TextMeshProUGUI>();
        currObj = transform.Find("CurrentObj").GetComponent<Image>();
        img = transform.Find("Bar").GetComponent<Image>();
        imgWorkAmount = transform.Find("ActionBar").GetComponent<Image>();

        if (NS != null)
        {
            if (EasterEgg.instance.isNightStalker)
            {
                Player = EasterEgg.instance.NightStalker.GetComponent<Movement>();
                transform.Find("CharaImg").GetComponent<Image>().sprite = NS;
            }
            else
            {
                Player = EasterEgg.instance.Syd.GetComponent<Movement>();
                transform.Find("CharaImg").GetComponent<Image>().sprite = Syd;
            }
        }

        CharaWorkBGAnim = transform.Find("CharaImgBG").GetComponent<Animator>();

    }
    // Update is called once per frame
    void Update()
    {
        CharaWorkBGAnim.SetBool("Chased", Player.KillerScript.CurrentTransformPlayer.PlayerScript != null && Player.KillerScript.CurrentTransformPlayer.PlayerScript == Player && Player.KillerScript.currentState == Enemy.States.Chasing && Player.currentState != Movement.States.Dead && Player.currentState != Movement.States.Corrupted);
        CharaWorkBGAnim.SetBool("Hidding", Vector3.Distance(Player.Killer.transform.position, Player.transform.position) <= Player.SpottedDistance +7.5f && Player.currentState != Movement.States.Dead && Player.currentState != Movement.States.Corrupted);

        if (Player.isSelected)
        {
            pos.anchoredPosition = new Vector2(-92, pos.anchoredPosition.y);
        }
        else
        {
            pos.anchoredPosition = new Vector2(-74, pos.anchoredPosition.y);
        }
        if (Player.currentItem == Movement.Items.None)
        {
            currObj.sprite = Resources.Load<Sprite>("None");
        }
        else if (Player.currentItem == Movement.Items.Trap)
        {
            currObj.sprite = Resources.Load<Sprite>("bearTrap");
        }
        else if (Player.currentItem == Movement.Items.Ward)
        {
            currObj.sprite = Resources.Load<Sprite>("Ward");
        }
        else if (Player.currentItem == Movement.Items.FireCracker)
        {
            currObj.sprite = Resources.Load<Sprite>("fireCracker");
        }

        if (Player.currentState == Movement.States.Working || Player.currentState == Movement.States.Action)
        {
            infotext.text = "Working";
            imgWorkAmount.color = Color.yellow;

            if (Player.currentState == Movement.States.Working)
            {
                imgWorkAmount.gameObject.SetActive(true);
                img.gameObject.SetActive(true);
                imgWorkAmount.fillAmount = 1 - (Player.currentObjective.WorkLeft / Player.currentObjective.WorkNeeded);
            }

            if (Player.startedWorking)
            {
                imgWorkAmount.gameObject.SetActive(true);
                img.gameObject.SetActive(true);
                switch (Player.currentItem)
                {
                    case Movement.Items.Trap:
                    imgWorkAmount.fillAmount = 1 - (Player.currentBearTrap.workLeft / Player.currentBearTrap.workAmount);
                    break;
                    case Movement.Items.Ward:
                        imgWorkAmount.fillAmount = 1 - (Player.currentWard.workLeft / Player.currentWard.workAmount);
                        break;
                    case Movement.Items.FireCracker:
                        imgWorkAmount.fillAmount = 1 - (Player.currentFirecracker.workLeft / Player.currentFirecracker.workAmount);
                        break;
                }
            }
        }
        else if (Player.currentState == Movement.States.Walk)
        {
            infotext.text = "Fine";
            imgWorkAmount.gameObject.SetActive(false);
            img.gameObject.SetActive(false);
            //       imgWorkAmount.color = Color.white;
        }
        else if (Player.currentState == Movement.States.Corrupted)
        {
            infotext.text = "Fading";
            imgWorkAmount.gameObject.SetActive(true);
            img.gameObject.SetActive(true);
            imgWorkAmount.color = Color.blue;
            imgWorkAmount.fillAmount = 1 - (Player.currDisolveAmount / Player.disolveAmount);
        }
        else if (Player.currentState == Movement.States.Dead)
        {
            infotext.text = "Digitalised";
            imgWorkAmount.gameObject.SetActive(false);
            img.gameObject.SetActive(false);
        }
    }
}
