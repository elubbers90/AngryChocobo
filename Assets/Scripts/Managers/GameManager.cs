using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [HideInInspector]
    public LevelManager levelManager;
    [HideInInspector]
    public UIManager uiManager;
    public GameObject playerReference;
    public GameObject candyReference;

    [HideInInspector]
    public int lives = 3;
    private GameObject player;
    private Player playerScript;
    [HideInInspector]
    public int level;
    [HideInInspector]
    public List<int> purchasedEggs;

    [HideInInspector]
    public int collectedCandy;
    [HideInInspector]
    public int totalCakes;
    [HideInInspector]
    public int totalCandy;

    [HideInInspector]
    public bool paused = false;

    private int currentVictoryCheck = 0;
    private bool gameEnded;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        
        levelManager = GetComponent<LevelManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        StartCoroutine(WaitAndInit());
    }

    public void InitGame() {
        Vector3 world = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        lives = 3;
        collectedCandy = 0;
        levelManager.SetupLevel(level);
        player = Instantiate(playerReference, new Vector3(2.5f - world.x, 0f, 0f), Quaternion.identity) as GameObject;
        playerScript = player.GetComponent<Player>();
        uiManager.ToggleGameOverlay(true);
        currentVictoryCheck = 0;
        gameEnded = false;
    }

    public void EnemyHit() {
        lives -= 1;
        uiManager.SetCurrentCakesText();
        if (lives <= 0) {
            GameOver();
        }
    }

    public void ActivateEgg(int type) {
        playerScript.ActivateEgg(type);
    }

    private IEnumerator WaitAndInit() {
        float waitTime = 0.5f;
        yield return new WaitForSeconds(waitTime);
        if (SaveSystem.IsLoaded()) {
            level = SaveSystem.GetInt("level", 1);
            totalCakes = SaveSystem.GetInt("cakes", 0);
            totalCandy = SaveSystem.GetInt("candy", 0);
            purchasedEggs = Utils.ToIntList(SaveSystem.GetString("purchasedEggs", "0"));
            StartCoroutine(uiManager.ToggleSplashScreen(false));
        } else {
            StartCoroutine(WaitAndInit());
        }
    }

    private IEnumerator WaitAndCheck() {
        currentVictoryCheck++;
        int victoryCheck = currentVictoryCheck;
        float waitTime = 0.5f;
        yield return new WaitForSeconds(waitTime);
        if (!gameEnded && victoryCheck == currentVictoryCheck && levelManager.CheckVictory()) {
            gameEnded = true;
            totalCakes += lives;
            totalCandy += collectedCandy;

            SaveSystem.SetInt("cakes", totalCakes);
            SaveSystem.SetInt("candy", totalCandy);

            ClearLevel();
            level++;
            SaveSystem.SetInt("level", level);
            SaveSystem.SaveToDisk();
            uiManager.ToggleVictory(true);
        }
    }

    public void GiveCandy(int startHp) {
        collectedCandy += (int) (Random.Range((int)Math.Floor((float)startHp / 2), startHp) * Mathf.Ceil(level / 2f));
    }


    public void CheckVictory() {
        StartCoroutine(WaitAndCheck());
    }

    public void GameOver() {
        gameEnded = true;
        currentVictoryCheck = 0;

        totalCandy += collectedCandy;
        SaveSystem.SetInt("candy", totalCandy);
        ClearLevel();
        uiManager.ToggleGameOver(true);
    }

    public void ClearLevel() {

        enabled = false;
        levelManager.RemoveManager();

        Destroy(player);
        uiManager.ToggleGameOverlay(false);
    }


    // Upgrades
    public void PayCandy(int cost) {
        totalCandy -= cost;
        SaveSystem.SetInt("candy", totalCandy);
        uiManager.SetCurrentCurrency();
    }

    public void PayCakes(int cost) {
        totalCakes -= cost;
        SaveSystem.SetInt("cakes", totalCakes);
        uiManager.SetCurrentCurrency();
    }

    public void PurchaseEgg(int eggType) {
        purchasedEggs.Add(eggType);
        SaveSystem.SetString("purchasedEggs", Utils.IntListToString(purchasedEggs));
    }

    public void IncreaseEggDamage(int eggType) {
        switch (eggType) {
            case 0:
                SaveSystem.SetInt("basicEggDamage", SaveSystem.GetInt("basicEggDamage", 2) + 1);
                break;
            case 1:
                SaveSystem.SetInt("lightningEggDamage", SaveSystem.GetInt("lightningEggDamage", 4) + 1);
                break;
            case 2:
                SaveSystem.SetInt("fireEggDamage", SaveSystem.GetInt("fireEggDamage", 3) + 1);
                break;
            case 3:
                SaveSystem.SetInt("energyEggDamage", SaveSystem.GetInt("energyEggDamage", 4) + 1);
                break;
            case 4:
                SaveSystem.SetInt("waterEggDamage", SaveSystem.GetInt("waterEggDamage", 3) + 1);
                break;
        }
    }

    public void IncreaseEggSpeed(int eggType) {
        switch (eggType) {
            case 0:
                SaveSystem.SetFloat("basicEggSpeed", SaveSystem.GetFloat("basicEggSpeed", 10f) + 0.1f);
                break;
            case 1:
                SaveSystem.SetFloat("lightningEggSpeed", SaveSystem.GetFloat("lightningEggSpeed", 15f) + 0.1f);
                break;
            case 2:
                SaveSystem.SetFloat("fireEggSpeed", SaveSystem.GetFloat("fireEggSpeed", 10f) + 0.1f);
                break;
            case 3:
                SaveSystem.SetFloat("energyEggSpeed", SaveSystem.GetFloat("energyEggSpeed", 10f) + 0.1f);
                break;
            case 4:
                SaveSystem.SetFloat("waterEggSpeed", SaveSystem.GetFloat("waterEggSpeed", 10f) + 0.1f);
                break;
        }
    }

    public void IncreaseEggAmount(int eggType) {
        switch (eggType) {
            case 1:
                SaveSystem.SetInt("lightningEggAmount", SaveSystem.GetInt("lightningEggAmount", 20) + 1);
                break;
            case 2:
                SaveSystem.SetInt("fireEggAmount", SaveSystem.GetInt("fireEggAmount", 15) + 1);
                break;
            case 3:
                SaveSystem.SetInt("energyEggAmount", SaveSystem.GetInt("energyEggAmount", 25) + 1);
                break;
            case 4:
                SaveSystem.SetInt("waterEggAmount", SaveSystem.GetInt("waterEggAmount", 15) + 1);
                break;
        }
    }

    public void UpgradeSpecial(SpecialUpgradeType type) {
        switch (type) {
            case SpecialUpgradeType.LightningBolt:
                SaveSystem.SetBool("lightningEggBolt", true);
                break;
            case SpecialUpgradeType.LightningBoltDamage:
                SaveSystem.SetInt("lightningEggBoltDamage", SaveSystem.GetInt("lightningEggBoltDamage", 5) + 1);
                break;
            case SpecialUpgradeType.FireDamage:
                SaveSystem.SetInt("fireEggFireDamage", SaveSystem.GetInt("fireEggFireDamage", 1) + 1);
                break;
            case SpecialUpgradeType.FireSpeed:
                SaveSystem.SetFloat("fireEggFireSpeed", SaveSystem.GetFloat("fireEggFireSpeed", 1f) - 0.01f);
                break;
            case SpecialUpgradeType.EnergyRadius:
                SaveSystem.SetFloat("energyEggRadius", SaveSystem.GetFloat("energyEggRadius", 1.5f) + 0.05f);
                break;
            case SpecialUpgradeType.EnergyDelay:
                SaveSystem.SetFloat("energyEggDelay", SaveSystem.GetFloat("energyEggDelay", 1.5f) - 0.01f);
                break;
            case SpecialUpgradeType.WaterSlow:
                SaveSystem.SetFloat("waterEggSlow", SaveSystem.GetFloat("waterEggSlow", 0.5f) - 0.01f);
                break;
            case SpecialUpgradeType.WaterRadius:
                SaveSystem.SetFloat("waterEggRadius", SaveSystem.GetFloat("waterEggRadius", 1f) + 0.05f);
                break;
        }
    }
}
