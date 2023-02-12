using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    public static ThirdPersonCamera TPC;


    [SerializeField] float camX;
    float rotationX;
    float rotationY;
    float sensitivity = 5;
    [SerializeField]Vector3 eulerRotation;
    Vector3 offset;
    Vector3 newOffset;
    Quaternion camRotation;
    [SerializeField]float rotateHorizontal;
    [SerializeField]float rotateVeritcal;
    public static bool isMovable = true;
    void Start()
    {
        TPC = this;
      eulerRotation = transform.rotation.eulerAngles;
      offset = transform.position - MainGame.instance.playerCharacter.transform.position;
        StartCoroutine(delayCameraControl());
    }

    // Update is called once per frame
    void Update()
    {
        //if (PlayerCharacter.StopPlayer)
        //{
        //    return;
        //}
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            isMovable = !isMovable;
        }
        if(isMovable)
        {
            rotateHorizontal = Input.GetAxis("Mouse X");
            rotateVeritcal = Input.GetAxis("Mouse Y");

            eulerRotation.x -= rotateVeritcal * sensitivity;
            eulerRotation.y += rotateHorizontal * sensitivity;
            if (eulerRotation.x > 70 && eulerRotation.x < 90)
                eulerRotation.x = 70;
            if (eulerRotation.x < -30)
                eulerRotation.x = -30;

            transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);
            camRotation = transform.rotation;

            newOffset = camRotation * offset;
           
            
            transform.position = MainGame.instance.playerCharacter.transform.position + newOffset;
        }

    }

    public IEnumerator delayCameraControl()
    {
        isMovable = false;
        yield return new WaitForSeconds(0.1f);
        isMovable = true;
    }
}
