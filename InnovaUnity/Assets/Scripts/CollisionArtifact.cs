using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionArtifact : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag == "PlayerBullet")
        {
            Debug.Log(gameObject.name);
            Destroy(MainGame.instance.randomManager.artifact.GetComponent<ArtifactBox>());
            MainGame.instance.playerCharacter.addArtifact(gameObject.name);
        }
    }
}
