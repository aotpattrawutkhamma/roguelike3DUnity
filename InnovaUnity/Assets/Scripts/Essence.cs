using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Essence : MonoBehaviour
{
    public static Essence instance;
    public int value;

    void Start()
    {
        instance = this;
    }
    
    //public void essenceValue(int amount)
    //{
    //    value = amount;
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            MainGame.instance.playerCharacter.Addmoney(value);
            Destroy(this.gameObject);
        }
    }
}
