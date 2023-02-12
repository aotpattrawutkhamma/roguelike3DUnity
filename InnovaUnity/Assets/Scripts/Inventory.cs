using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public Dictionary<string, int> inventoryDic = new Dictionary<string, int>();
    Dictionary<string, GameObject> inventoryUi = new Dictionary<string, GameObject>();
    public Image lastArtifactImage;
    [SerializeField] List<string> lastObjectString;

    public TMP_FontAsset normalFont;
    
    private void Start()
    {
        addItemInDic();
        lastArtifactImage = Instantiate(lastArtifactImage);
        lastArtifactImage.transform.position = new Vector3(0, 0, 0);
    }

    void addItemInDic()
    {
        //inventoryDic.Add("1", 0);
        //inventoryDic.Add("2", 0);
        //inventoryDic.Add("3", 0);
        //inventoryDic.Add("4", 0);

        inventoryDic.Add("Morpheus's Dream", 0);
        inventoryDic.Add("Boots of Asmodel", 0);
        inventoryDic.Add("Nidbai's Mercy", 0);
        inventoryDic.Add("Azrael's Talent", 0);
        inventoryDic.Add("Amulet of Nuriel", 0);
        inventoryDic.Add("Cloth Armor", 0);
        inventoryDic.Add("Israfil", 0);
        inventoryDic.Add("Power Crystal", 0);
        inventoryDic.Add("Die of Fate", 0);
        inventoryDic.Add("Lens of Truth", 0);
        inventoryDic.Add("Potion of Longetivity", 0);
        inventoryDic.Add("Pendulum Clock", 0);
        inventoryDic.Add("Silver Gloves", 0);
    }
    public void ReceiveAllArtifactHold(int num)
    {
        string[] listOfArtifact = { "Morpheus's Dream", "Boots of Asmodel", "Nidbai's Mercy", "Azrael's Talent", "Amulet of Nuriel", "Cloth Armor", "Israfil", "Power Crystal",
                                    "Die of Fate", "Lens of Truth", "Potion of Longetivity", "Pendulum Clock", "Silver Gloves"};
        for (int i = 0; i < listOfArtifact.Length; i++)
        {
            if (inventoryDic[listOfArtifact[i]] > 0)
            {
                inventoryDic[listOfArtifact[i]] += num;
                GameObject ob = inventoryUi[listOfArtifact[i]];
                lastArtifactImage.transform.SetParent(ob.transform);
                lastArtifactImage.transform.localPosition = new Vector3(0, 0, 0);
                lastArtifactImage.transform.SetAsFirstSibling();
                TextMeshProUGUI changeStack = ob.GetComponentInChildren<TextMeshProUGUI>();
                changeStack.text = "x" + inventoryDic[listOfArtifact[i]];
                
            }
        }
       
    }
    public void ReceiveArtifact(string cardNum)
    {
        int cardNo = int.Parse(cardNum);
        int id = MainGame.instance.randomManager.cards_ID[cardNo];
        string idString = MainGame.instance.artifactManager.artifactList[id - 1].artifactName;

        GameObject inven = MainGame.instance.itemCanvas;
       
        inventoryDic[idString] += 1;
        lastObjectString.Add(idString);
        if (inventoryDic[idString] == 1)
        {

            GameObject ob = new GameObject();
            inventoryUi.Add(idString, ob);
            ob.AddComponent<RectTransform>();
            ob.transform.localPosition = Vector3.zero;
            ob.name = idString;
            ob.transform.SetParent(MainGame.instance.itemCanvas.transform);

            lastArtifactImage.transform.SetParent(ob.transform);
            lastArtifactImage.transform.localPosition = new Vector3(0, 0, 0);

            Image image = MainGame.instance.randomManager.cards_Image[cardNo];
            Image im = Instantiate(image);
            TextMeshProUGUI text = MainGame.instance.randomManager.cards_Name[cardNo];
            TextMeshProUGUI textInstaniate = Instantiate(text);
            //textInstaniate.name = id;

            im.transform.SetParent(ob.transform);
            im.transform.localPosition = new Vector3(0, 0, 0);
            im.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            textInstaniate.transform.SetParent(ob.transform);
            textInstaniate.font = normalFont;
            textInstaniate.text = "x" + inventoryDic[idString];
            textInstaniate.fontSize = 30;
            textInstaniate.color = Color.black;
            textInstaniate.transform.localPosition = new Vector3(15, -20, 0);

            MainGame.instance.playerCharacter.CalculateArtifact();
        }
        else
        {
            //GameObject ob = GameObject.Find(id);
            GameObject ob = inventoryUi[idString];
            lastArtifactImage.transform.SetParent(ob.transform);
            lastArtifactImage.transform.localPosition = new Vector3(0, 0, 0);
            lastArtifactImage.transform.SetAsFirstSibling();
            TextMeshProUGUI changeStack = ob.GetComponentInChildren<TextMeshProUGUI>();
            changeStack.text = "x" + inventoryDic[idString];
        }
        if (idString == "Amulet of Nuriel")
        {
            ReceiveAllArtifactHold(2);
        }
    }
}
