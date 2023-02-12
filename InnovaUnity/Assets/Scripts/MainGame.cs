using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
    public static MainGame instance;
    public Camera mainCamera;
  
    public PlayerCharacter playerCharacter;
   
    public GameObject itemCanvas;
    public Inventory inventory;
    public SoundManager soundManager;
    public Dictionary<string, AudioSource> sounds;
    public GunProjectile gunProjectile;
    public EnemyAI enemyAI;

    public Artifacts artifacts;
    public ArtifactManager artifactManager;
    //public GameObject randomcanvas;
    public GameObject randomcanvas2;
    public Canvas canvasWorldSpace;

    public GameObject characterUI;
    public GameObject abilityUI;
    public GameObject cooldownUI;
    public GameObject interactF;
    
    public Image crosshair;

    public EnemySpawner enemySpawner;
    public float gameDifficulty = 1;
    public GameObject moneyUI;
    public RandomManager randomManager;
    public PillarSpawner pillarSpawner;
    public GameObject artifactFolder;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        //MainGame.instance.randomcanvas.SetActive(false);
        MainGame.instance.randomcanvas2.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        interactF.SetActive(false);
    }


    public void PlaySound(string name)
    {
        sounds[name].Play();
    }

    public void LoadThisScene(int number)
    {
        SceneManager.LoadScene(number);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadPreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    public int RoundNumber (float number)
    {
        int value = (int)number;
        float remainder = number - value;

        if (remainder >= 0.5f)
        {
            remainder = 1f;
        }
        else
        {
            remainder = 0;
        }

        value += (int)remainder;

        return value;
    }
}
