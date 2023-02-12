using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCharacter : BaseCharacter
{
    public bool isInit = false;
    public float animationSmooth = 0.1f;
    public ArtifactBox artifactBox;
    public float jumpForce;
    public bool previouslyJumped = false;
    public float blendX, blendY;
    protected float lerpX;
    protected float lerpY;
    public float blendXBefore,blendYBefore;
    public bool isDoingNothing = true;
    public int i2 = 0, j = 0, k = 0;
    public Vector3 playerAngle;
    public LayerMask enemyLayer;
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode ability1 = KeyCode.E;
    public KeyCode ability2 = KeyCode.LeftShift;
    public KeyCode specialSkill = KeyCode.Z;
    public KeyCode reload = KeyCode.R;
    public Rigidbody rigidBody;
    public bool isMoving = false;
    public float totalSpeed;
    public float totalSpeedSubBonus;
    public float speed = 5;
    public float speedAmp = 1;
    public float moveSpeedBonus = 0;
    public float maxSpeed = 100;
    public float dmgAmp = 1;
    public Vector3 a, b, c;
    public bool isGrounded = true;
    public int jumpCharge = 1;
    public int jumpChargeMax = 1;
    public float windDownAttack = 0;
    public float criticalRate = 0;
    protected float criticalDamage = 200f;
    public float criticalRateTotal;
    public float criticalDamageTotal;
    public static bool StopPlayer;
    public Dictionary<string, int> artifactStack;
    public float coolDownReduction;
    Vector3 distanceBefore;
    Vector3 distanceAfter;
    float distanceDelta;

    float lerpSpeed;
    public Image hpBar;
    public Inventory inventory;
    [SerializeField] float attackPowerTotal;
    [SerializeField] float artifactCriticalRate;
    [SerializeField] float artifactCriticalDamage;
    [SerializeField] float artifactMoveSpeedBonus;
    [SerializeField] float artifactBonusDamage;
    [SerializeField] float artifactCooldownReduction;
    [SerializeField] float artifactBonusHp;
    [SerializeField] float artifactBonusDef;
    [SerializeField] protected float artifactFireRate;
    [SerializeField] float artifactFireRateBonus;

    [SerializeField] float artifactMorpheusDreamSleepPerStack;
    [SerializeField] float artifactMorpheusDreamDamagePerStack;
    [SerializeField] float artifactMorpheusDreamCriticalChancePerStack;
    [SerializeField] float artifactMorpheusDreamSleepDuration;

    [SerializeField] float artifactBootsOfAsmodelMoveSpeedPerStack;
    [SerializeField] float artifactBootsOfAsmodelCooldownReductionPerStack;
    [SerializeField] float artifactBootsOfAsmodelExplosionDamage; //Do not Stack
    [SerializeField] float artifactBootsOfAsmodelExplosionRadiusPerStack; //Stack
    float artifactBootsOfAsmodelExplosionRadiusTotal;
    float artifactBootsOfAsmodelCharge = 0;

    [SerializeField] float artifactNidbaiMercyHpPerStack;
    [SerializeField] float artifactNidbaiMercyArmorPerStack;
    protected float artifactNidbaiMercyAttackStack = 0;
    protected float artifactNidbaiMercyStackTimer = 0;
    [SerializeField] float artifactNidbaiMercyLeechInitial = 7;
    [SerializeField] float artifactNidbaiMercyLeechPerStack = 3f;
    protected float artifactNidbaiMercyTotalLeech = 0;

    [SerializeField] float artifactAzraelTalentCriticalChancePerStack;
    [SerializeField] float artifactAzraelTalentCriticalDamagePerStack;
    [SerializeField] float artifactAzraelTalentProcNumberOrigin;
    float artifactAzraelTalentProcNumberCurrent;
    float artifactAzraelTalentProcNumberMax;

    [SerializeField] float artifactAmuletOfNurielMovespeedPerStack;
    [SerializeField] float artifactAmuletOfNurielNegativeHpPerStack;
    [SerializeField] float artifactAmuletOfNurielSearingWindDurationMax;
    float artifactAmuletOfNurielSearingWindDurationCurrent;
    float artifactAmuletOfNurielSearingWindProcChance;
    [SerializeField] float artifactAmuletOfNurielSearingWindProcChanceOrigin;
    bool artifactAmuletOfNurielIsSearingWindActive = false;

    [SerializeField] float artifactClothArmorDefensePerStack;
    [SerializeField] float artifactPowerCrystalAttackPerStack;
    [SerializeField] float artifactDieOfFateCriticalChancePerStack;
    [SerializeField] float artifactLensOfTruthCriticalDamagePerStack;
    [SerializeField] float artifactPotionOfLongetivityHpPerStack;
    [SerializeField] float artifactPendulumClockCooldownReductionPerStack;
    [SerializeField] float artifactSilverGlovesFireRatePerStack;



    [SerializeField] private float money;



    GameObject playerMoney;
    TextMeshProUGUI[] moneyText = new TextMeshProUGUI[2];
    public float CalculateDamage(float dmg)
    {
       
        float newDmg = dmg;
        newDmg += attackPowerTotal;
        int rng = Random.Range(0, 100);
     
        if (rng < criticalRateTotal)
        {
            newDmg = newDmg * (criticalDamageTotal / 100);
            Debug.Log("1");
        }
        else if(artifactAzraelTalentProcNumberCurrent <= 0)
        {
            artifactAzraelTalentProcNumberCurrent = artifactAzraelTalentProcNumberMax;
            newDmg = newDmg * (criticalDamageTotal / 100);
            Debug.Log("2");
        }
        newDmg = newDmg * dmgAmp;

        if(artifactNidbaiMercyAttackStack > 0)
        {
            artifactNidbaiMercyAttackStack--;
            Heal(newDmg * artifactNidbaiMercyTotalLeech);
        }
        return newDmg;
    }
    
    public void OnHitProc(BaseCharacter enemy)
    {
        int rngSleep = Random.Range(0, 100);
        if(rngSleep < CalculatePercent(artifactMorpheusDreamSleepPerStack,inventory.inventoryDic["Morpheus's Dream"]))
        {
            enemy.TakeSleep(artifactMorpheusDreamSleepDuration);
        }
        if(artifactBootsOfAsmodelCharge >= 100)
        {
            artifactBootsOfAsmodelCharge = 0;
            Collider[] inRadius = Physics.OverlapSphere(enemy.transform.position, artifactBootsOfAsmodelExplosionRadiusTotal, enemyLayer);
            foreach (var item in inRadius)
            {
                if(item.tag == "Enemy")
                {
                    BaseCharacter thisEnemy = item.GetComponent<BaseCharacter>();
                    thisEnemy.TakeDamage(artifactBootsOfAsmodelExplosionDamage);
                    OnHitProc(thisEnemy);
                }
            }
        }
        if(inventory.inventoryDic["Azrael's Talent"] > 0)
        {
            artifactAzraelTalentProcNumberCurrent--;
        }
        if(inventory.inventoryDic["Amulet of Nuriel"] > 0)
        {
            if(!artifactAmuletOfNurielIsSearingWindActive)
            {
                int rngSearingWind = Random.Range(0, 100);
                if(rngSearingWind < artifactAmuletOfNurielSearingWindProcChance)
                {
                    artifactAmuletOfNurielIsSearingWindActive = true;
                    artifactAmuletOfNurielSearingWindDurationCurrent = artifactAmuletOfNurielSearingWindDurationMax;
                }
            }
        }
     
    }
    public float CalculatePercent(float percent, int stack)
    {
        float newPercent = (1 - Mathf.Pow((100 - percent) / 100, stack)) * 100;
        return newPercent;
    }
    public void CalculateArtifact()
    {
        artifactCriticalRate = (artifactMorpheusDreamCriticalChancePerStack * inventory.inventoryDic["Morpheus's Dream"]) + (artifactAzraelTalentCriticalChancePerStack * inventory.inventoryDic["Azrael's Talent"]) + (artifactDieOfFateCriticalChancePerStack * inventory.inventoryDic["Die of Fate"]);
        artifactCriticalDamage = (artifactAzraelTalentCriticalDamagePerStack * inventory.inventoryDic["Azrael's Talent"]) + (artifactLensOfTruthCriticalDamagePerStack * inventory.inventoryDic["Lens of Truth"]);
        artifactBonusDamage = (artifactMorpheusDreamDamagePerStack * inventory.inventoryDic["Morpheus's Dream"]) + (artifactPowerCrystalAttackPerStack * inventory.inventoryDic["Power Crystal"]);
        artifactMoveSpeedBonus = (artifactBootsOfAsmodelMoveSpeedPerStack * inventory.inventoryDic["Boots of Asmodel"]) + (artifactAmuletOfNurielMovespeedPerStack * inventory.inventoryDic["Amulet of Nuriel"]);
        artifactCooldownReduction = (artifactBootsOfAsmodelCooldownReductionPerStack * inventory.inventoryDic["Boots of Asmodel"]) + (artifactPendulumClockCooldownReductionPerStack * inventory.inventoryDic["Pendulum Clock"]);
        artifactBootsOfAsmodelExplosionRadiusTotal = artifactBootsOfAsmodelExplosionRadiusPerStack * inventory.inventoryDic["Boots of Asmodel"];
        artifactBonusHp = artifactNidbaiMercyHpPerStack * inventory.inventoryDic["Nidbai's Mercy"] - (artifactAmuletOfNurielNegativeHpPerStack * inventory.inventoryDic["Amulet of Nuriel"]) + (artifactPotionOfLongetivityHpPerStack * inventory.inventoryDic["Potion of Longetivity"]);
        artifactBonusDef = (artifactNidbaiMercyArmorPerStack * inventory.inventoryDic["Nidbai's Mercy"]) + (artifactClothArmorDefensePerStack * inventory.inventoryDic["Cloth Armor"]);
        artifactAzraelTalentProcNumberMax = artifactAzraelTalentProcNumberOrigin - inventory.inventoryDic["Azrael's Talent"];
        artifactFireRateBonus = (artifactSilverGlovesFireRatePerStack * inventory.inventoryDic["Silver Gloves"]);
        if(artifactAzraelTalentProcNumberMax < 1)
        {
            artifactAzraelTalentProcNumberMax = 1;
        }
        artifactAmuletOfNurielSearingWindProcChance = CalculatePercent(artifactAmuletOfNurielSearingWindProcChanceOrigin, inventory.inventoryDic["Amulet of Nuriel"]);

        attackPowerTotal = attackPower + artifactBonusDamage;
        criticalRateTotal = criticalRate + artifactCriticalRate;
        criticalDamageTotal = criticalDamage + artifactCriticalDamage;
        if(inventory.inventoryDic["Azrael's Talent"] > 0)
        {
            if(criticalRateTotal > 100)
            {
                float excess = criticalRateTotal - 100;
                criticalDamageTotal = criticalDamage + artifactCriticalDamage + excess;
            }
        }
        artifactAzraelTalentProcNumberCurrent = artifactAzraelTalentProcNumberMax;
        maxHpCurrent = maxHpBase + artifactBonusHp;
        defCurrent = defBase + artifactBonusDef;
        artifactFireRate = artifactFireRateBonus + 1;
        SetCooldown();
    }
    public virtual void SetCooldown()
    {

    }
    public void Heal(float heal)
    {

    }
    public override void Start()
    {
        base.Start();
        SetHP();
        rigidBody = GetComponent<Rigidbody>();
        StopPlayer = false;
        Start_SetCooldown();
        enemyLayer = LayerMask.GetMask("Enemy");
        Start_SetPlayerMoney();
        lerpX = 0;
        lerpY = 0;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (artifactNidbaiMercyStackTimer > 0)
        {
            artifactNidbaiMercyStackTimer -= Time.deltaTime;
        }
        if (artifactNidbaiMercyStackTimer < 0)
        {
            artifactNidbaiMercyStackTimer = 0;
            artifactNidbaiMercyAttackStack = 0;
        }
        if (artifactAmuletOfNurielSearingWindDurationCurrent > 0)
        {
            artifactAmuletOfNurielSearingWindDurationCurrent -= Time.deltaTime;
        }
        if (artifactAmuletOfNurielSearingWindDurationCurrent < 0)
        {
            artifactAmuletOfNurielIsSearingWindActive = false;
            artifactAmuletOfNurielSearingWindDurationCurrent = 0;
        }
      
        distanceAfter = transform.position;
        distanceDelta = Vector3.Distance(distanceBefore, distanceAfter);
        distanceBefore = transform.position;
        
        //function decrease hp

        if (hp > maxHpCurrent)
        {
            hp = maxHpCurrent;
        }

        lerpSpeed = 3f * Time.deltaTime;

        HPBarFiller();
        ColorChanger();

        //if (StopPlayer) return;
        if (jumpCharge == jumpChargeMax && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            isMoving = false;
        }
        if (inventory.inventoryDic["Boots of Asmodel"] > 0)
        {
            artifactBootsOfAsmodelCharge += distanceDelta;
            if (artifactBootsOfAsmodelCharge > 100)
                artifactBootsOfAsmodelCharge = 100;
        }
        totalSpeedSubBonus = speed * speedAmp;
        totalSpeed = speed * speedAmp + moveSpeedBonus + artifactMoveSpeedBonus;
        if(totalSpeed > maxSpeed)
        {
            totalSpeed = maxSpeed;
        }
        windDownAttack -= Time.deltaTime;
        base.Update();
        Vector3 currentVelocity = rigidBody.velocity;
        if(windDownAttack <= 0)
        {
            if (Input.GetKey(forwardKey) && !StopPlayer)
            {
                currentVelocity.z = 1 * totalSpeed;
                if(k != 1)
                {
                    blendYBefore = k;
                }
                k = 1;
                isMoving = true;
            }
            else if (Input.GetKey(backwardKey) && !StopPlayer)
            {
                currentVelocity.z = -1 * totalSpeed;
                if (k != -1)
                {
                    blendYBefore = k;
                }
                k = -1;
                isMoving = true;
            }
            else
            {
                currentVelocity.z = 0;
                if (k != 0)
                {
                    blendYBefore = k;
                }
                k = 0;

            }
            
          

            if (Input.GetKey(leftKey) && !StopPlayer)
            {
                currentVelocity.x = -1 * totalSpeed;
                if(i2 != -1)
                {
                    blendXBefore = i2;
                }
                i2 = -1;
                isMoving = true;
            }
            else if (Input.GetKey(rightKey) && !StopPlayer)
            {
                currentVelocity.x = 1 * totalSpeed;
                if (i2 != 1)
                {
                    blendXBefore = i2;
                }
                i2 = 1;
                isMoving = true;
            }
            else
            {
                currentVelocity.x = 0;
                if (i2 != 0)
                {
                    blendXBefore = i2;
                }
                i2 = 0;
            }
            lerpX = Mathf.Lerp(blendXBefore, i2, animationSmooth);
            lerpY = Mathf.Lerp(blendYBefore, k, animationSmooth);
            blendXBefore = lerpX;
            blendYBefore = lerpY;
        }
        else
        {
            k = 0;
            i2 = 0;
            isMoving = false;
            currentVelocity.x = 0;
            currentVelocity.z = 0;
        }
        


        if(Input.GetKeyDown(KeyCode.Space) && !StopPlayer)
        {
            if(jumpCharge > 0)
            {
                jumpCharge--;
                rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                Jump();
            }
            
        }
        float yVelo = currentVelocity.y;
        currentVelocity.y = 0;
        Quaternion camRotation = MainGame.instance.mainCamera.transform.rotation;
        //camRotation = new Quaternion(camRotation.x, 0, camRotation.z, camRotation.w);
        camRotation = new Quaternion(0, camRotation.y, camRotation.z, camRotation.w);
        Vector3 newVelo = camRotation * currentVelocity;
        newVelo.y = yVelo;
        if (ThirdPersonCamera.isMovable)
        {
            rigidBody.velocity = newVelo;
        }

        //Vector3 direction = new Vector3(i2, 0f, k);
        // playerAngle = camRotation.normalized * direction;
        Vector3 direction2 = new Vector3(1, 0, 1);
        direction2 = direction2.normalized;
        playerAngle = camRotation.normalized * direction2;
        playerAngle = new Vector3(playerAngle.x, 0, playerAngle.z);
        playerAngle = playerAngle.normalized;

        if (Input.GetKey(ability1) && !StopPlayer)
        {
            if(windDownAttack <= 0)
            {
                Ability1();
            }
           
        }
        if(Input.GetKeyDown(ability2) && !StopPlayer)
        {
            if (windDownAttack <= 0)
            {
                Ability2();
            }
            
        }
        if(Input.GetKeyDown(specialSkill) && !StopPlayer)
        {
            if (windDownAttack <= 0)
            {
            
                Ultimate();
            }
           
        }
        if(Input.GetKeyDown(reload) && !StopPlayer)
        {
            Reload();
        }
        if(Input.GetKey(KeyCode.Mouse0))
        {
            if (windDownAttack <= 0)
            {
                PrimaryAttack();
            }
            
        }
        if(Input.GetKeyDown(KeyCode.Mouse1) && !StopPlayer)
        {
            if (windDownAttack <= 0)
            {
                SecondaryAttack();
            }
            
        }

        
       
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Floor")
        {
            jumpCharge = jumpChargeMax;
            HitGround();
            if(!isInit)
            {
                isInit = !isInit;
                MainGame.instance.enemySpawner.Initialize();
            }
        }
    }


    private void Start_SetPlayerMoney()
    {
        playerMoney = MainGame.instance.moneyUI;
        moneyText = playerMoney.GetComponentsInChildren<TextMeshProUGUI>();
        moneyText[0].text = money.ToString();
        moneyText[1].text = money.ToString();
    }

    public virtual void HitGround()
    {

    }
    private void SetHP()
    {
        hp = maxHpBase;
        maxHpCurrent = maxHpBase;
    }

    private void Start_SetCooldown()
    {
        GameObject cooldown = MainGame.instance.cooldownUI;
        for (int i = 0; i < cooldown.transform.childCount; i++)
        {
            cooldown.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void HPBarFiller()
    {
        hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, (hp / maxHpCurrent), lerpSpeed);
    }

    void ColorChanger()
    {
        Color hpColor = Color.Lerp(Color.red, Color.green, (hp / maxHpCurrent));
        hpBar.color = hpColor;
    }

    //void showCard(int card , int CardNumber)
    //{
    //    string cost = ArtifactNumber[card - 1].GetComponent<Artifacts>().cost.ToString();
    //    cards_Description[CardNumber].text = ""+ArtifactNumber[card-1].GetComponent<Artifacts>().description;
    //    cards_Name[CardNumber].text = "" + ArtifactNumber[card-1].GetComponent<Artifacts>().artifactName;
    //    cards_Quote[CardNumber].text = "" + ArtifactNumber[card-1].GetComponent<Artifacts>().quote;
    //    cards_Image[CardNumber].sprite = ArtifactNumber[card-1].GetComponent<Artifacts>().image;
    //    cards_Cost[CardNumber].text = cost;

    //    string id = card.ToString();
    //    cards_ID[CardNumber] = id;

    //    MainGame.instance.randomcanvas.SetActive(true);
    //    StopPlayer = true;
    //}

    public void addArtifact (string number)
    {
        int place = int.Parse(number);
        int id = MainGame.instance.randomManager.cards_ID[place];
        //GameObject artifactFolder = MainGame.instance.artifactFolder;
        //int count = artifactFolder.transform.childCount;

        //for (int i = 0; i < count; i++)
        //{
        //    artifactFolder.transform.GetChild(i);
        //}

        Debug.Log("add");
        float cost = artifactBox.costOfCardID[place];
        if (money >=  cost)
        {
            //MainGame.instance.randomcanvas.SetActive(false);
            MainGame.instance.randomcanvas2.SetActive(false);
            MainGame.instance.inventory.ReceiveArtifact(number);
            Cursor.visible = false;
            MainGame.instance.crosshair.enabled = true;
            money -= cost;
            moneyText[0].text = money.ToString();
            moneyText[1].text = money.ToString();
            //Destroy(artifactBox.gameObject);
            StopPlayer = false;
        }
    }
    

    public void Addmoney(float moneyRecieve)
    {
        float gameDifficulty = MainGame.instance.gameDifficulty;
        money = (moneyRecieve * gameDifficulty) + money;

        moneyText[0].text = money.ToString();
        moneyText[1].text = money.ToString();
    }

    public virtual void Jump()
    {

    }
    public virtual void PrimaryAttack()
    {

    }
    public virtual void SecondaryAttack()
    {

    }
    public virtual void Ability1()
    {

    }
    public virtual void Ability2()
    {

    }
    public virtual void Ultimate()
    {

    }
    public virtual void Reload()
    {

    }
    public override void TakeDamage(float dmg)
    {
        if(artifactAmuletOfNurielIsSearingWindActive)
        {

        }
        else
        {
            base.TakeDamage(dmg);
            if(hp <= 0)
            {
                Die();
            }
        }

        if(inventory.inventoryDic["Nidbai's Mercy"] > 0)
        {
            artifactNidbaiMercyTotalLeech = artifactNidbaiMercyLeechInitial + (artifactNidbaiMercyLeechPerStack * (inventory.inventoryDic["Nidbai's Mercy"] - 1));
            artifactNidbaiMercyStackTimer = 2;
        }
    }
    public virtual void Die()
    {

    }
    public virtual void PillarDestroyedSound()
    {

    }
}


