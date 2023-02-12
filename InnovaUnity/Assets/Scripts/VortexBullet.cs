using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexBullet : MonoBehaviour
{
    public Vector3 direction;
    public float bulletSpeed;
    public Character_Vortex owner;
    public float dmg;
    public Vector3 lastFramePosition = new Vector3(0, 0, 0);
    public LayerMask layerMask;
    public void Initiate(Vector3 dir, float speed, Character_Vortex mother, float damage)
    {
        direction = dir;
        bulletSpeed = speed;
        owner = mother;
        dmg = damage;
        Collider col = GetComponent<Collider>();
        
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction.normalized * Time.deltaTime * bulletSpeed);
        if(lastFramePosition != Vector3.zero)
        {
            Vector3 dir = transform.position - lastFramePosition;
            RaycastHit hit;
            Ray ray = new Ray(transform.position, dir.normalized);
            if(Physics.Raycast(ray, out hit, dir.magnitude,layerMask))
            {
                if(hit.collider.tag == "Enemy" || hit.collider.tag == "Protector")
                {
                    owner.HitEnemy();
                    BaseCharacter enemy = hit.collider.GetComponent<BaseCharacter>();
                    owner.OnHitProc(enemy);
                    enemy.TakeDamage(owner.CalculateDamage(dmg));
                }
                else if(hit.collider.tag == "Floor" || hit.collider.tag == "Obstacle" || hit.collider.tag == "Invisible Wall")
                {
                    Destroy(this.gameObject);
                }
                else if(hit.collider.tag == "Pillar")
                {
                    owner.HitEnemy();
                    Pillar pillar = hit.collider.GetComponent<Pillar>();
                    pillar.TakeDamage(owner.CalculateDamage(dmg));
                    Destroy(this.gameObject);
                }
                else if (hit.collider.gameObject.tag == "ArtifactHP")
                {

                    ArtifactBox artifactBox = hit.collider.GetComponentInParent<ArtifactBox>();
                    artifactBox.TakeDamage(10);
                    Destroy(this.gameObject);
                }
            }
        }



        lastFramePosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Protector")
        {
           
            owner.HitEnemy();
            BaseCharacter enemy = other.GetComponent<BaseCharacter>();
            owner.OnHitProc(enemy);
            enemy.TakeDamage(owner.CalculateDamage(dmg));
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "Floor" || other.gameObject.tag == "Obstacle" || other.gameObject.tag == "Invisible Wall")
        {
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "Pillar")
        {
            
            owner.HitEnemy();
            Pillar pillar = other.GetComponent<Pillar>();
            pillar.TakeDamage(owner.CalculateDamage(dmg));
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "ArtifactHP")
        {
          
            if (PlayerCharacter.StopPlayer == false)
            { 
                ArtifactBox artifactBox = other.GetComponentInParent<ArtifactBox>();
                if (artifactBox.get_shoot == false)
                {
                    artifactBox.get_shoot = true;
                }
                else
                {
                    artifactBox.other_shoot = true;
                }
                artifactBox.TakeDamage(10);
            }
            Destroy(this.gameObject);
        }
    }

}
