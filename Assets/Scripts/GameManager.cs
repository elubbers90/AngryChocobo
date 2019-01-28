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
    public GameObject cakeReference;

    private int lives = 3;
    private GameObject player;
    private Player playerScript;
    private List<GameObject> cakes;
    [HideInInspector]
    public int level;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        cakes = new List<GameObject>();
        levelManager = GetComponent<LevelManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        StartCoroutine(WaitAndInit());
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.RightControl)) {
            level = 1;
            SaveSystem.SetInt("level", level);
        }
    }

    public void InitGame() {
        Vector3 world = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        lives = 3;
        cakes.Add(Instantiate(cakeReference, new Vector3(1 - world.x, 0f, 0f), Quaternion.identity) as GameObject);
        cakes.Add(Instantiate(cakeReference, new Vector3(1 - world.x, -2f, 0f), Quaternion.identity) as GameObject);
        cakes.Add(Instantiate(cakeReference, new Vector3(1 - world.x, 2f, 0f), Quaternion.identity) as GameObject);
        levelManager.SetupLevel(level);
        player = Instantiate(playerReference, new Vector3(3f - world.x, 0f, 0f), Quaternion.identity) as GameObject;
        playerScript = player.GetComponent<Player>();
        uiManager.ShowEggType(playerScript.selectedEggType);
        uiManager.ToggleEggSwitcher(true);
    }

    public void EnemyHit() {
        lives -= 1;
        if (cakes.Count > 0) {
            Destroy(cakes[cakes.Count - 1]);
            cakes.RemoveAt(cakes.Count - 1);
        }
        if (lives <= 0) {
            GameOver();
        }
    }

    public void SwitchEggType() {
        playerScript.SwitchEggType();

    }

    private IEnumerator WaitAndInit() {
        float waitTime = 2f;
        yield return new WaitForSeconds(waitTime);
        level = SaveSystem.GetInt("level", 1);
        uiManager.ToggleMainMenu(true);
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

    private void ClearLevel() {
        enabled = false;
        levelManager.RemoveManager();

        Destroy(player);
        for(int i = 0; i < cakes.Count; i++) {
            Destroy(cakes[i]);
        }
        cakes.Clear();
        uiManager.ToggleEggSwitcher(false);
    }
}
