using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArtifactBox : MonoBehaviour
{
    //TextMeshProUGUI text;
    GameObject player;
    Rigidbody playerRbdy;
    //Transform origin;
    //public static ArtifactBox AB;
    public bool closeToPlayer = false;
    public bool pressed = false;
    public bool choose = false;
    public List<int> cardID;
    public float[] costOfCardID = new float[10];
    public float hp = 40;
    public float distance;
    Canvas canvasWorldSpace;
    [SerializeField]  private float shoot_time = 3f;
    public bool get_shoot = false;
    public bool other_shoot = false;
    public float destroy_time = 30f;


    public enum ArtifactPrice
    {
        Low,
        Medium,
        High
    }

    ArtifactPrice currentArtifact;

    void Start()
    {
        //AB = this;
        //text = this.transform.GetComponentInChildren<TextMeshProUGUI>();
        //text.gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        //origin = this.transform;
    }

    void Update()
    {
        if (get_shoot)
        {
            shoot_time -= 1 * Time.deltaTime;
        }

        if (other_shoot)
        {
            shoot_time = 3f;
            other_shoot = false;
        }

        if (shoot_time <= 0)
        {
            hp = 40;
            shoot_time = 3f;
            get_shoot = false;
        }

        if (destroy_time <= 0)
        {
            MainGame.instance.randomcanvas2.SetActive(false);
            canvasWorldSpace.transform.position = Vector3.zero;
            ArtifactBox ab = gameObject.GetComponent<ArtifactBox>();
            Destroy(ab);
        }

        if (pressed && destroy_time > 0)
        {
            canvasWorldSpace = MainGame.instance.canvasWorldSpace;
            Vector3 dir = transform.position - (player.transform.position);

            Quaternion lookRotation = Quaternion.LookRotation(dir);
            canvasWorldSpace.transform.rotation = lookRotation;
            Vector3 pos = transform.position + new Vector3(0,3,0);
            canvasWorldSpace.transform.position = pos - (dir / 2);
            destroy_time -= 1 * Time.deltaTime;
        }

        //distance = Vector3.Distance(player.transform.position, transform.position);

        //if (distance <= 15)
        //{
        //    //MainGame.instance.interactF.SetActive(true);
        //    closeToPlayer = true;
        //}
        //else if (distance > 18)
        //{
        //    hp = 40;
        //}
        //else if(closeToPlayer)
        //{
        //    //MainGame.instance.interactF.SetActive(false);
        //    closeToPlayer = false;
        //}

        ////if (closeToPlayer && !pressed)
        if (hp <= 0 && !pressed)
        {
            //MainGame.instance.interactF.SetActive(false);
            //canvasWorldSpace = MainGame.instance.canvasWorldSpace;
            //Vector3 dir = transform.position - (player.transform.position);

            //Quaternion lookRotation = Quaternion.LookRotation(dir);
            //canvasWorldSpace.transform.rotation = lookRotation;
            //canvasWorldSpace.transform.position = transform.position - (dir / 2);
            //_direction = (player.transform.position.position - transform.position).normalized;

            //rotate us over time according to speed until we are in the required rotation
            //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
            //canvasWorldSpace.transform.LookAt(player.transform);
            gameObject.transform.GetChild(0).gameObject.SetActive(false);

            //PlayerCharacter.StopPlayer = true;
            if (cardID.Count == 0)
            {
                randomArtifact();
            }
            showCard();
            //Cursor.lockState = CursorLockMode.Confined;
            //MainGame.instance.crosshair.enabled = false;
            //Cursor.visible = true;
            pressed = true;
        }
    }

    public void randomArtifact()
    {
        int[] card = { 0, 0, 0 };
        int random, i = 0;
        bool have = false;

        while (i < 3)
        {
            have = false;
            random = Random.Range(1, 8);
            if (i != 0)
            {
                for (int j = 0; j < i; j++)
                {
                    if (random == card[j])
                    {
                        have = true;
                        break;
                    }
                }
            }
            if (!have)
            {
                card[i] = random;
                //addCard(random, i);
                cardID.Add(random);
                i++;
            }
        }
    }

    private void showCard ()
    {
        RandomManager randomManager = MainGame.instance.randomManager;

        randomManager.artifact = gameObject.transform;
        for (int i = 0; i < 3; i++)
        {
            //string cost = MainGame.instance.artifactManager.artifactList[cardID[i] - 1].cost.ToString();

            int type = MainGame.instance.artifactManager.artifactList[cardID[i] - 1].costType;

            checktype(type);
            string cost = randomCost(i);

            randomManager.cards_Description[i].text = MainGame.instance.artifactManager.artifactList[cardID[i]-1].description;
            randomManager.cards_Name[i].text = MainGame.instance.artifactManager.artifactList[cardID[i]-1].artifactName;
            randomManager.cards_Quote[i].text = MainGame.instance.artifactManager.artifactList[cardID[i] - 1].quote;
            randomManager.cards_Image[i].sprite = MainGame.instance.artifactManager.artifactList[cardID[i] - 1].image;
            randomManager.cards_Cost[i].text = cost;
            randomManager.cards_ID[i] = cardID[i];

            /* random canvas
             * MainGame.instance.randomcanvas.SetActive(true);
             */
            MainGame.instance.randomcanvas2.SetActive(true);
            //PlayerCharacter.StopPlayer = true;
            MainGame.instance.playerCharacter.artifactBox = this;
        }
    }

    private void checktype (int type)
    {
        switch (type)
        {
            case 1: // Low
                currentArtifact = ArtifactPrice.Low;
                break;
            case 2: // Medium
                currentArtifact = ArtifactPrice.Medium;
                break;
            case 3: // High
                currentArtifact = ArtifactPrice.High;
                break;
            default:
                Debug.Log(currentArtifact);
                Debug.Log("No Price tag");
                break;
        }

    }

    private string randomCost(int i)
    {
        string costString;
        float cost;
        switch (currentArtifact)
        {
            case ArtifactPrice.Low:
                cost = Random.Range(1000,1501) * (1 + (MainGame.instance.gameDifficulty * 0.7f));
                break;
            case ArtifactPrice.Medium:
                cost = Random.Range(3300,3601) * (1 + (MainGame.instance.gameDifficulty * 0.7f));
                break;
            case ArtifactPrice.High:
                cost = Random.Range(5000, 7001) * (1 + (MainGame.instance.gameDifficulty * 0.7f));
                break;
            default:
                cost = 9999999;
                break;
        }

        cost = MainGame.instance.RoundNumber(cost);
        costOfCardID[i] = cost;
        costString = cost.ToString();

        return costString;
    }

    public void TakeDamage(float dmg)
    {
        hp -= dmg;
    }
}
