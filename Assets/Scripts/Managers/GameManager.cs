using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [HideInInspector]
    public LevelManager levelManager;
    [HideInInspector]
    public UIManager uiManager;
    public GameObject playerReference;

    [HideInInspector]
    public int lives = 3;
    private GameObject player;
    private Player playerScript;
    [HideInInspector]
    public int level;
    [HideInInspector]
    public List<int> purchasedEggs;

    [HideInInspector]
    public bool paused = false;

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
        levelManager.SetupLevel(level);
        player = Instantiate(playerReference, new Vector3(2.5f - world.x, 0f, 0f), Quaternion.identity) as GameObject;
        playerScript = player.GetComponent<Player>();
        uiManager.ToggleGameOverlay(true);
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
            purchasedEggs = Utils.ToIntList(SaveSystem.GetString("purchasedEggs", "0"));
            StartCoroutine(uiManager.ToggleSplashScreen(false));
        } else {
            StartCoroutine(WaitAndInit());
        }
    }

    private IEnumerator WaitAndCheck() {
        float waitTime = 0.5f;
        yield return new WaitForSeconds(waitTime);
        if (levelManager.CheckVictory()) {
            ClearLevel();
            level++;
            SaveSystem.SetInt("level", level);
            SaveSystem.SaveToDisk();
            uiManager.ToggleVictory(true);
        }
    }

    public void CheckVictory() {
        StartCoroutine(WaitAndCheck());
    }

    public void GameOver() {
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
    public void PurchaseEgg(int eggType) {
        purchasedEggs.Add(eggType);
        SaveSystem.SetString("purchasedEggs", Utils.IntListToString(purchasedEggs));
        SaveSystem.SaveToDisk();
    }
}
