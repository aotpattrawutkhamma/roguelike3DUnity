using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public PlayerCharacter playerCharacter;
    Quaternion playerRotation;
    Quaternion previousLook;

    Vector3 direction = new Vector3(1, 0, 1);
    private void Start()
    {
        //playerCharacter = GetComponent<PlayerCharacter>();
    }
    void Update()
    {
        //playerRotation = Quaternion.LookRotation(playerCharacter.playerAngle);
        playerRotation = MainGame.instance.mainCamera.transform.rotation;
        playerRotation = new Quaternion(0, playerRotation.y, 0, playerRotation.w);
        transform.rotation = playerRotation;
        previousLook = playerRotation;
        /*if (playerCharacter.isMoving)
        {
            transform.rotation = playerRotation;
            previousLook = playerRotation;
        }
        else
        {
            if(playerCharacter.isDoingNothing)
            transform.rotation = previousLook;
        }*/


    }
}
