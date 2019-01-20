using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public LevelManager levelManager;
    public GameObject playerReference;
    public GameObject cakeReference;

    [HideInInspector]
    public int level = 1;
    private int lives = 3;
    private GameObject player;
    private List<GameObject> cakes;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        cakes = new List<GameObject>();
        levelManager = GetComponent<LevelManager>();
        InitGame();
    }

    //this is called only once, and the paramter tell it to be called only after the scene was loaded
    //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization() {
        //register the callback to be called everytime the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //This is called each time a scene is loaded.
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
        instance.InitGame();
    }

    private void Update() {
    }

    void InitGame() {
        lives = 3;
        cakes.Add(Instantiate(cakeReference, new Vector3(-10f, 0f, 0f), Quaternion.identity) as GameObject);
        cakes.Add(Instantiate(cakeReference, new Vector3(-10f, -2f, 0f), Quaternion.identity) as GameObject);
        cakes.Add(Instantiate(cakeReference, new Vector3(-10f, 2f, 0f), Quaternion.identity) as GameObject);
        levelManager.SetupLevel(level);
        player = Instantiate(playerReference, new Vector3(-8.75f, 0f, 0f), Quaternion.identity) as GameObject;
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

    private IEnumerator WaitAndInit() {
        float waitTime = 2f;
        yield return new WaitForSeconds(waitTime);
        InitGame();
    }

    private IEnumerator WaitAndCheck() {
        float waitTime = 0.5f;
        yield return new WaitForSeconds(waitTime);
        if (levelManager.CheckVictory()) {
            ClearLevel();
            level++;
            StartCoroutine(WaitAndInit());
        }
    }

    public void CheckVictory() {
        StartCoroutine(WaitAndCheck());
    }

    public void GameOver() {
        ClearLevel();
    }

    private void ClearLevel() {
        enabled = false;
        levelManager.RemoveManager();

        Destroy(player);
        for(int i = 0; i < cakes.Count; i++) {
            Destroy(cakes[i]);
        }
        cakes.Clear();
    }
}
