using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Character_Vortex : PlayerCharacter
{
    [SerializeField] float passiveSoundTriggerNumber;
    bool previouslyReachedPassiveSound = false;
    bool previouslyCast;
    bool castDone;
   
    public Animator animator;
    public LayerMask attackLayer;
    public float passiveStack = 0;
    public float passiveTimer = 0;
    public float lightningQueenAmp = 0.04f;
    [Header("Thunder Clap Config ##")]
    
    [Header("MCd = Skill Cooldown")]
    public float thunderClapRadius = 5f;
    public float thunderClapStunDuration;
    public float thunderClapMinDamage;
    public float thunderClapMaxDamage;
    float thunderClapCd;
    public float thunderClapMCd;
    float thunderClapOriginCd;
    [Header("Avatar of Thunder Config ##")]
    public float avatarOfThunderDuration;
    float avatarOfThunderCurrentDuration;
    public float avatarOfThunderMCd;
    [Header("Avatar of Thunder Config End ##")]
    public float avatarOfThunderMoveSpeedBonus;
    float avatarOfThunderCd;
    float avatarOfThunderOriginCd;
    public bool isAvatarOfThunderActive;
    public bool isAvartarOfThunderPreviouslyActive;
    public float avatarOfThunderMoveSpeedDecrease;
    float avatarOfThunderTBefore;
    float avatarOfThunderTNow;
    float avatarOfThunderTDelta;
    float avartarofthunderDeltaTime;
    float avartarofthunderDeltaTime2;
    public float tempestaStack;
    public bool isTempestaActive = false;
    [Header("Tempesta Config")]
    public float tempestaDuration;
    float tempestaDurationCurrent;
    public float tempestaRadius;
    public float tempestaMinDmg;
    public float tempestaMaxDmg;
    public float tempestaInterval;
    public float tempestaMCd;
    [Header("Tempesta Config End ##")]
    float tempestaTDelta;
    float tempestaCd;
   
    float tempestaOriginCd;
    [Header("FireRate ##")]
    public float fireRate = 10;
    public float bulletSpeed = 50f;
    [Header("FireRate End ##")]

    float fireTimer;
    float fireTimerMax;
    float fireRateAmp = 1;
    //public int ammoCurrent;
    //public int ammoMax;
    //public float reloadTime;
    //public float reloadAmp = 1;


    float secondaryCd;
    public float secondaryMCd;
    float secondaryOriginCd;
    public float secondaryDistance;
    public float secondaryAngle;
    public float secondaryHeight;
    public Vector3 secondaryHeightVector;
    public Color secondaryMeshColor = Color.red;
    public Mesh mesh;
    
    Ray middleScreen;
    RaycastHit hitinfo;
    Vector3 AttackTarget;
    public VortexBullet bulletPrefab;
    public Transform gunPoint;
    public LayerMask enemyLayerMask;
    public Collider[] inRadius;

    public float primaryMinDamage;
    public float primaryMaxDamage;
    public float secondaryMinDamage;
    public float secondaryMaxDamage;
    public float t = 0;
    public float tBefore = 0;
    public float tDelta = 0;

    public string chaName = "Vortex";
    GameObject chaUI;
    [SerializeField] Sprite face;
    GameObject abiUI;
    [SerializeField] Sprite abi1Sprite;
    [SerializeField] Sprite abi2Sprite;
    [SerializeField] Sprite ulitmateSprite;
    [SerializeField] Sprite priSprite;
    [SerializeField] Sprite secSprite;
    GameObject cooldown;
    GameObject coolUlt;
    GameObject coolAbi1;
    GameObject coolAbi2;
    GameObject coolSec;
    GameObject numUlt;
    GameObject numAbi1;
    GameObject numAbi2;
    GameObject numSec;
    GameObject passiveStackOb;
    TextMeshProUGUI passiveStackTxt;
    public bool allowGroundReset = false;
    public bool previouslyOutOfWalk = false;
    public override void SetCooldown()
    {
        avatarOfThunderMCd = avatarOfThunderOriginCd * (100 / (100 + coolDownReduction));
        secondaryMCd = secondaryOriginCd * (100 / (100 + coolDownReduction));
        tempestaMCd = tempestaOriginCd * (100 / (100 + coolDownReduction));
        thunderClapMCd = thunderClapOriginCd * (100 / (100 + coolDownReduction));
    }
    public override void PillarDestroyedSound()
    {
        base.PillarDestroyedSound();
        MainGame.instance.soundManager.PlaySound("VortexSpawnerDestroyedSound");
    }
    public override void Die()
    {
        base.Die();
        MainGame.instance.soundManager.PlaySound("VortexDeathSound");
    }
    public override void Start()
    {
        base.Start();
        Vector3 middle = new Vector3(0.5f, 0.5f, 0);
        chaUI = MainGame.instance.characterUI;
        abiUI = MainGame.instance.abilityUI;
        middleScreen = MainGame.instance.mainCamera.ViewportPointToRay(middle);
        cooldown = MainGame.instance.cooldownUI;
        SetCharacter();
        SetAbility();
        SetCooldownOb();
        SetAttackSpeed();
        secondaryHeightVector.y = secondaryHeight;
        avatarOfThunderOriginCd = avatarOfThunderMCd;
        secondaryOriginCd = secondaryMCd;
        tempestaOriginCd = tempestaMCd;
        thunderClapOriginCd = thunderClapMCd;
        animator = GetComponentInChildren<Animator>();
        CalculateArtifact();
    }

    public override void Update()
    {
        if(passiveStack > passiveSoundTriggerNumber)
        {
            if(!previouslyReachedPassiveSound)
            {
                MainGame.instance.soundManager.PlaySound("VortexPassiveSound");
                previouslyReachedPassiveSound = true;
            }
        }
        if(previouslyReachedPassiveSound)
        {
            if(passiveStack < passiveSoundTriggerNumber)
            {
                previouslyReachedPassiveSound = false;
            }
        }
        if (isTempestaActive)
        {
            tempestaTDelta = 0;
        }
        else
        {
            passiveTimer -= Time.deltaTime;
            if (passiveTimer <= 0)
            {
                tempestaTDelta += Time.deltaTime;
                tBefore = t;
                t = Mathf.Pow(tempestaTDelta, 2);
                tDelta = t - tBefore;
                if (passiveStack > 0)
                {
                    passiveStack -= (tDelta + 2) / 7;
                }

                if (passiveStack < 0)
                {
                    passiveStack = 0;
                }
                CalculateStack();
            }
        }

        if (secondaryCd > 0)
        {
            secondaryCd -= Time.deltaTime;

            Image imSec = coolSec.GetComponent<Image>();
            TextMeshProUGUI textSec = numSec.GetComponent<TextMeshProUGUI>();

            textSec.text = Mathf.RoundToInt(secondaryCd).ToString();
            imSec.fillAmount = secondaryCd / secondaryMCd;
        }
        else if (secondaryCd < 0)
        {
            secondaryCd = 0;
            coolSec.SetActive(false);
            numSec.SetActive(false);
        }
        if (thunderClapCd > 0)
        {
            thunderClapCd -= Time.deltaTime;

            Image imAbi1 = coolAbi1.GetComponent<Image>();
            TextMeshProUGUI textAbi1 = numAbi1.GetComponent<TextMeshProUGUI>();

            textAbi1.text = Mathf.RoundToInt(thunderClapCd).ToString();
            imAbi1.fillAmount = thunderClapCd / thunderClapMCd;
        }
        else if (thunderClapCd < 0)
        {
            thunderClapCd = 0;
            coolAbi1.SetActive(false);
            numAbi1.SetActive(false);
        }

        if (avatarOfThunderCd > 0)
        {
            avatarOfThunderCd -= Time.deltaTime;

            Image imAbi2 = coolAbi2.GetComponent<Image>();
            TextMeshProUGUI textAbi2 = numAbi2.GetComponent<TextMeshProUGUI>();

            textAbi2.text = Mathf.RoundToInt(avatarOfThunderCd).ToString();
            imAbi2.fillAmount = avatarOfThunderCd / avatarOfThunderMCd;
        }
        else if (avatarOfThunderCd < 0)
        {
            avatarOfThunderCd = 0;
            coolAbi2.SetActive(false);
            numAbi2.SetActive(false);
        }
        if (tempestaCd > 0)
        {
            tempestaCd -= Time.deltaTime;

            Image imUlt = coolUlt.GetComponent<Image>();
            TextMeshProUGUI textUlt = numUlt.GetComponent<TextMeshProUGUI>();

            textUlt.text = Mathf.RoundToInt(tempestaCd).ToString();
            imUlt.fillAmount = tempestaCd / tempestaMCd;
        }
        else if (tempestaCd < 0)
        {
            tempestaCd = 0;
            coolUlt.SetActive(false);
            numUlt.SetActive(false);
        }

        base.Update();

        passiveStackTxt.text = passiveStack.ToString();
        fireTimer -= Time.deltaTime;
        if(isAvatarOfThunderActive)
        {
            avartarofthunderDeltaTime += Time.deltaTime;
            avatarOfThunderTNow = Mathf.Pow(avartarofthunderDeltaTime, 2);
          
            avatarOfThunderMoveSpeedBonus = totalSpeedSubBonus * ((avatarOfThunderTNow + 2) / 7);
            moveSpeedBonus = avatarOfThunderMoveSpeedBonus;
            avatarOfThunderCurrentDuration -= Time.deltaTime;
            if(avatarOfThunderCurrentDuration < 0)
            {
                isAvatarOfThunderActive = false;
            }
        }
        else
        {
            if(moveSpeedBonus > 0)
            {
                avartarofthunderDeltaTime2 += Time.deltaTime;
                avatarOfThunderTDelta = Mathf.Pow(avartarofthunderDeltaTime2, 2);
                avatarOfThunderMoveSpeedDecrease = totalSpeedSubBonus * ((avatarOfThunderTNow + 2) / 7);
                moveSpeedBonus = avatarOfThunderMoveSpeedBonus - avatarOfThunderMoveSpeedDecrease;
            }
            else if(moveSpeedBonus < 0 && isAvartarOfThunderPreviouslyActive)
            {
                moveSpeedBonus = 0;
                isAvartarOfThunderPreviouslyActive = false;
                avatarOfThunderMoveSpeedDecrease = 0;
                avatarOfThunderMoveSpeedBonus = 0;
                avatarOfThunderTDelta = 0;
                avatarOfThunderTNow = 0;
                avartarofthunderDeltaTime2 = 0;
                
            }
            avartarofthunderDeltaTime = 0;
        }

        
        animator.SetFloat("BlendX", lerpX * 10);
        animator.SetFloat("BlendY", lerpY * 10);
        if(!previouslyJumped)
        {
            if(previouslyOutOfWalk)
            {
                animator.SetTrigger("BlendTrigger");
                previouslyOutOfWalk = false;
            }
            
        }
        if(previouslyCast)
        {
            if(castDone)
            {
                animator.SetTrigger("BlendTrigger");
                previouslyCast = false;
                castDone = false;
            }
        }
    }

    void SetCharacter()
    {
        TextMeshProUGUI name = chaUI.GetComponentInChildren<TextMeshProUGUI>();
        GameObject imageOb = chaUI.transform.GetChild(3).gameObject;
        Image im = imageOb.GetComponent<Image>();
        im.sprite = face;
        name.text = chaName;
    }

    void SetAbility ()
    {
        GameObject abi1Ob = abiUI.transform.GetChild(3).gameObject;
        GameObject abi2Ob = abiUI.transform.GetChild(4).gameObject;
        GameObject ultOb = abiUI.transform.GetChild(5).gameObject;
        GameObject priOb = abiUI.transform.GetChild(6).gameObject;
        GameObject secOb = abiUI.transform.GetChild(7).gameObject;

        passiveStackOb = abiUI.transform.GetChild(8).gameObject;
        passiveStackTxt = passiveStackOb.GetComponent<TextMeshProUGUI>();

        Image abi1Im = abi1Ob.GetComponent<Image>();
        Image abi2Im = abi2Ob.GetComponent<Image>();
        Image ultIm = ultOb.GetComponent<Image>();
        Image priIm = priOb.GetComponent<Image>();
        Image secIm = secOb.GetComponent<Image>();

        abi1Im.sprite = abi1Sprite;
        abi2Im.sprite = abi2Sprite;
        ultIm.sprite = ulitmateSprite;
        priIm.sprite = priSprite;
        secIm.sprite = secSprite;
    }

    void SetCooldownOb()
    {
        coolAbi1 = cooldown.transform.GetChild(0).gameObject;
        coolAbi2 = cooldown.transform.GetChild(1).gameObject;
        coolUlt = cooldown.transform.GetChild(2).gameObject;
        coolSec = cooldown.transform.GetChild(3).gameObject;
        numAbi1 = cooldown.transform.GetChild(4).gameObject;
        numAbi2 = cooldown.transform.GetChild(5).gameObject;
        numUlt = cooldown.transform.GetChild(6).gameObject;
        numSec = cooldown.transform.GetChild(7).gameObject;
    }

    public void HitEnemy()
    {
        passiveTimer = 2;
        tBefore = 0;
        t = 0;
        tDelta = 0;
        if (passiveStack <= 0)
        {
            passiveStack = 1;
        }
        else
        {
            passiveStack += 1;
        }

        CalculateStack();
    }
    public void CalculateStack()
    {
        float lightningQueen = 1 + (passiveStack * 0.04f);
        speedAmp = lightningQueen;
        fireRateAmp = lightningQueen;
        dmgAmp = lightningQueen;
        SetAttackSpeed();
    }
    public override void Jump()
    {
        base.Jump();
        StartCoroutine(JumpCoroutine());
    }
    public IEnumerator JumpCoroutine()
    {
        previouslyJumped = true;
        animator.SetTrigger("JumpStart");
        allowGroundReset = false;
        yield return new WaitForSeconds(0.25f);
        animator.SetTrigger("JumpMidAir");
        allowGroundReset = true;
    }
    public override void HitGround()
    {
        base.HitGround();
        if(previouslyJumped)
        {
            StopCoroutine(JumpCoroutine());
            animator.SetTrigger("JumpContact");
            previouslyJumped = false;
            StartCoroutine(HitGroundCoroutine());
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.tag == "Floor")
        {
            if (previouslyJumped && allowGroundReset)
                HitGround();
        }
    }
    public IEnumerator HitGroundCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        previouslyOutOfWalk = true;
    }

    public void PlayCastingAnimation(string trigger)
    {
        animator.SetTrigger(trigger);
     
        previouslyCast = true;
        StartCoroutine(CastingWait());
    }
    public IEnumerator CastingWait()
    {
        windDownAttack = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        castDone = true;
    }
    public override void PrimaryAttack()
    {
        
       if(fireTimer <= 0)
       {
            fireTimer = fireTimerMax;
            Vector3 middle = new Vector3(0.5f, 0.5f, 0);
            middleScreen = MainGame.instance.mainCamera.ViewportPointToRay(middle);
            if (Physics.Raycast(middleScreen, out hitinfo,Mathf.Infinity,attackLayer))
            {
                int rng = Random.Range((int)primaryMinDamage, (int)primaryMaxDamage + 1);
                VortexBullet bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);
                bullet.Initiate(hitinfo.point - bullet.transform.position, bulletSpeed, this, rng);
                MainGame.instance.soundManager.PlaySound("VortexGunSound");
            }
            
       }
        
        
        

    }
    
    
    public override void SecondaryAttack()
    {
        if(secondaryCd <= 0)
        {
            secondaryCd = secondaryMCd;
            inRadius = Physics.OverlapSphere(gunPoint.position, secondaryDistance, enemyLayerMask);
            coolSec.SetActive(true);
            numSec.SetActive(true);

            foreach (var item in inRadius)
            {

                if (IsInSight(item))
                {
                    EnemyAI enemy = item.GetComponent<EnemyAI>();
                    int rng = Random.Range((int)secondaryMinDamage, (int)secondaryMaxDamage + 1);
                    HitEnemy();
                    OnHitProc(enemy);
                    enemy.TakeDamage(CalculateDamage(rng));
                }
            }
        }
        
    }

    public override void Ability1()
    {
        if(thunderClapCd <= 0)
        {
            StartCoroutine(ThunderClap());
            thunderClapCd = thunderClapMCd;

            coolAbi1.SetActive(true);
            numAbi1.SetActive(true);
            PlayCastingAnimation("Casting");
            MainGame.instance.soundManager.PlaySound("VortexThunderClapSound");
        }
     
    }
    public void ThunderClapAddon(List<EnemyAI> enemies, int rng, int location)
    {
        Collider[] enemies1 = Physics.OverlapSphere(transform.position, thunderClapRadius, enemyLayerMask);
        foreach (var item in enemies1)
        {
            if (item.tag == "Enemy")
            {
                EnemyAI enemy = item.GetComponent<EnemyAI>();
                if (!enemy.hasTakenThunderClap)
                {
                    enemy.hasTakenThunderClap = true;
                    enemies.Add(enemy);
                    enemy.TakeDamage(CalculateDamage(rng));
                    enemy.listLocation = location;
                    location++;
                    enemy.enemyList = enemies;
                }
            }
        }
    }
    public IEnumerator ThunderClap()
    {
        List<EnemyAI> enemies = new List<EnemyAI>();
        int location = 0;
        int rng = Random.Range((int)thunderClapMinDamage, (int)thunderClapMaxDamage + 1);
        ThunderClapAddon(enemies, rng, location);
        /*Collider[] enemies1 = Physics.OverlapSphere(transform.position, thunderClapRadius, enemyLayerMask);
         foreach (var item in enemies1)
        {
            if(item.tag == "Enemy")
            {
                EnemyAI enemy = item.GetComponent<EnemyAI>();
                if(!enemy.hasTakenThunderClap)
                {
                    enemy.hasTakenThunderClap = true;
                    enemies.Add(enemy);
                    enemy.TakeDamage(CalculateDamage(rng));
                    enemy.listLocation = location;
                    location++;

                }
            }
        }*/
        //loop to check if takendamge from thunderclap
        //deal dmg & stun
        //add to enemies
        yield return new WaitForSeconds(0.1f);
        ThunderClapAddon(enemies, rng, location);
        //loop again
        //deal dmg again & stun
        //add to enemies
        //end loop all enemies.takethunderclap = false
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].hasTakenThunderClap = false;
            enemies.RemoveAt(enemies[i].listLocation);
            i--;
        }
    }
    public override void Ability2()
    {
        if(avatarOfThunderCd <= 0)
        {
            isAvatarOfThunderActive = true;
            avatarOfThunderCd = avatarOfThunderMCd;
            avatarOfThunderCurrentDuration = avatarOfThunderDuration;
            avatarOfThunderTNow = 0;
            avatarOfThunderTBefore = 0;
            avatarOfThunderTDelta = 0;
            avatarOfThunderMoveSpeedBonus = 0;
            isAvartarOfThunderPreviouslyActive = true;

            coolAbi2.SetActive(true);
            numAbi2.SetActive(true);
            PlayCastingAnimation("Casting");
            MainGame.instance.soundManager.PlaySound("VortexAvatarOfThunderSound");
        }
        
    }
    
    public override void Ultimate()
    {
        if(tempestaCd <= 0)
        {
            StartCoroutine(Tempesta());
            tempestaCd = tempestaMCd;
           
            coolUlt.SetActive(true);
            numUlt.SetActive(true);
            PlayCastingAnimation("Casting");
            MainGame.instance.soundManager.PlaySound("VortexTempestaSound");
        }
    }
    public IEnumerator Tempesta()
    {
        tempestaDurationCurrent = tempestaDuration;
        isTempestaActive = true;
        passiveStack += tempestaStack;
        while(isTempestaActive)
        {
            Debug.Log("Tempest");
   
            int rng = Random.Range((int)tempestaMinDmg, (int)tempestaMaxDmg + 1);
            Collider[] enemies = Physics.OverlapSphere(transform.position, tempestaRadius, enemyLayerMask);
            foreach (var item in enemies)
            {
                if (item.tag == "Enemy")
                {
                    EnemyAI enemy = item.GetComponent<EnemyAI>();
                    enemy.TakeDamage(CalculateDamage(rng));
                }
            }
            
            yield return new WaitForSeconds(tempestaInterval);
            tempestaDurationCurrent -= tempestaInterval;
            if (tempestaDurationCurrent <= 0)
            {
                isTempestaActive = false;
            }
        }
        

        
    }
    /* public override void Reload()
     {
         //play animation
         StartCoroutine(ReloadCoroutine(reloadTime * (1/reloadAmp)));
     }
     IEnumerator ReloadCoroutine(float reloadTime)
     {
         windDownAttack = reloadTime;
         yield return new WaitForSeconds(reloadTime);
         ammoCurrent = ammoMax;
     }*/
    public void SetAttackSpeed()
    {
        fireTimerMax = 1 / (fireRate * fireRateAmp * artifactFireRate);
    }

    public bool IsInSight(Collider item)
    {
        Vector3 enemyPos = item.transform.position; ;
        Vector3 gunPos = gunPoint.transform.position; ;
        if(enemyPos.y - gunPos.y > secondaryHeight)
        {
            return false;
        }
        enemyPos.y = 0;
        gunPos.y = 0;
        if(Vector3.Angle(enemyPos - gunPos,gunPoint.transform.forward) > secondaryAngle)
        {
            return false;
        }

        return true;
    }
}
