using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PillarUi : MonoBehaviour
{
    public Pillar pillar;
    public int NumberPrefabs;
    public TextMeshProUGUI text1;
    public Image hpBar;
  
    // Start is called before the first frame update
    void Start()
    {
        text1.text = "" +    NumberPrefabs;
        
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.fillAmount = pillar.hp / pillar.hpMax;
    }
   
}
