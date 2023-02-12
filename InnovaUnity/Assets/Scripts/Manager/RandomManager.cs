using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RandomManager : MonoBehaviour
{
    public TextMeshProUGUI[] cards_Description;
    public TextMeshProUGUI[] cards_Name;
    public TextMeshProUGUI[] cards_Quote;
    public int[] cards_ID;
    public Image[] cards_Image;
    public TextMeshProUGUI[] cards_Cost;
    public Transform artifact;

    public void closeRandomUI ()
    {
        //MainGame.instance.randomcanvas.SetActive(false);
        MainGame.instance.randomcanvas2.SetActive(false);
        PlayerCharacter.StopPlayer = false;
        Cursor.visible = false;
        MainGame.instance.crosshair.enabled = true;
        ArtifactBox AB = artifact.GetComponent<ArtifactBox>();
        AB.pressed = false;
        //ArtifactBox.pressed = false;
    }
}
