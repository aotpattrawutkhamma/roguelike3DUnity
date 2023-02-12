using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProjectile : MonoBehaviour
{
    Vector3 direction;
   
    // bullet
    public GameObject bullet;

    //bullet force
    public float shootForce, upwardForce;

    //Gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletLeft, bulletShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Transform attackPoint;

    //Graphics
    public GameObject muzzleFlash;

    //bug fix
    public bool allowInvoke = true;

    private void Awake()
    {
        bulletLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        if (bulletLeft < magazineSize && !reloading)
        {
            Reload();
        }
        if (readyToShoot && shooting && !reloading && bulletLeft <= 0)
        {
            Reload();
        }
        
        // shooting อาจจะส่งมาจาก EnemyAI
        if (readyToShoot && shooting)
        {
            bulletShot = 0;

            Shoot();
        }

        
    }

    public void GunShoot ()
    {
        Debug.Log("gunshoot");
        shooting = true;
    }

    public void StopShoot ()
    {
        shooting = false;
    }
    public void NewShoot()
    {
        Vector3 directionTowardPlayer = MainGame.instance.playerCharacter.transform.position - transform.position;

        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        targetPoint = Vector3.zero;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            targetPoint = hit.point;
        }

        //Calculate firection from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Calculate new direction with spread

        Vector3 directionWithSpread = directionTowardPlayer + new Vector3(x, y, 0);
        /*Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);*/


        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Add forces to bullet
        //currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        //currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        direction = directionWithSpread.normalized * shootForce;
        Bullet newBullet = currentBullet.GetComponent<Bullet>();
        newBullet.Ownership(this.gameObject.GetComponent<EnemyAI>());
        newBullet.Initiate(direction, shootForce);
    }
    private void Shoot()
    {
        readyToShoot = false;
        Vector3 directionTowardPlayer = MainGame.instance.playerCharacter.transform.position - transform.position;
        
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        targetPoint = Vector3.zero;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            targetPoint = hit.point;
        }

        //Calculate firection from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Calculate new direction with spread
        
        Vector3 directionWithSpread = directionTowardPlayer + new Vector3(x, y, 0);
        /*Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);*/


        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Add forces to bullet
        //currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        //currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        direction = directionWithSpread.normalized * shootForce;
        Bullet newBullet = currentBullet.GetComponent<Bullet>();
        newBullet.Ownership(this.gameObject.GetComponent<EnemyAI>());
        newBullet.Initiate(direction,shootForce);
    
        
        

        bulletLeft--;
        bulletShot++;

        //Invoke resetShot function (if not already invoked) with your timeBetweenShooting
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        if (bulletShot < bulletsPerTap && bulletLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        //Allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletLeft = magazineSize;
        reloading = false;
    }
}
