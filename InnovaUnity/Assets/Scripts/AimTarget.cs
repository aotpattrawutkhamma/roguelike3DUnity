using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTarget : MonoBehaviour
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = MainGame.instance.mainCamera.transform.position + (MainGame.instance.mainCamera.transform.forward * 7.5f);
        pos.x += 0.1f;
        transform.position = pos;
    }
}
