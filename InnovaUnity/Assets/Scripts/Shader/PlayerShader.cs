using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShader : MonoBehaviour
{
    //public Material ObjectMaterial;
    public bool delay = false;
    public SkinnedMeshRenderer materialSkin;
    public float timepass = 1;
    float negative = 1;
    // Start is called before the first frame update
    void Start()
    {
        //ObjectMaterial = GetComponent<MeshRenderer>().material;
        materialSkin = GetComponent<SkinnedMeshRenderer>();
        StartCoroutine(DelayTime());
    }

    // Update is called once per frame
    void Update()
    {
        if(delay)
        {
            timepass -= Time.deltaTime;
            materialSkin.material.SetFloat("_Timer", timepass);

        }
        //timepass = Mathf.Lerp(negative, -1, 10.5f); 
        //ObjectMaterial.SetFloat("_Timer", timepass);
        //this.gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("Time_Value", timepass);

    }
    
    public IEnumerator DelayTime()
    {
        delay = true;
        yield return new WaitForSeconds(3f);
        delay = false;
    }
}
