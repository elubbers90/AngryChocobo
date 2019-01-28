using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour {
    public TileBase[] centerTiles;
    private Tilemap background;
    public GameObject[] enemyReferences;

    [HideInInspector]
    public Transform levelHolder;

    private Vector3 world;

    private List<GameObject> enemies;
    private int enemyAmount;
    private int enemiesLeft;

    public void Start() {
        background = GameObject.Find("Background").GetComponent<Tilemap>();
        world = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
    }

    private IEnumerator WaitAndSpawn() {
        float waitTime = Random.Range(1.5f, 3f);
        yield return new WaitForSeconds(waitTime);
        SpawnEnemy();
    }

    private void SpawnEnemy() {
        if (enabled) {
            enemiesLeft--;
            GameObject toInstantiate = enemies[Random.Range(0, enemies.Count)];

            float x = world.x + 1f;
            float y = Random.Range((0 - world.y + 2f), (world.y - 2f));

            GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

            SpriteRenderer[] renderers = instance.GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer renderer in renderers){
                renderer.sortingOrder = 10000 - (int)(y * 100);
            }

            instance.transform.SetParent(levelHolder);

            if (enemiesLeft > 0) {
                StartCoroutine(WaitAndSpawn());
            }
        }
    }

    private void SpawnBackground() {
        background.ClearAllTiles();
        for (int x = (0 - (int)world.x - 1); x <= (0 + (int)world.x); x++){
            for (int y = (0 - (int)world.y); y <= (0 + (int)world.y); y++) {
                background.SetTile(new Vector3Int(x, y, 0), centerTiles[Random.Range(0, centerTiles.Length)]);
            }
        }
    }

    public void SetupLevel(int level) {
        SpawnBackground();

        enemies = new List<GameObject>();
        enemyAmount = Random.Range(15, 20);
        enemiesLeft = enemyAmount;


        int modded = level % 3;
        if (modded == 0) {
            enemies.Add(enemyReferences[modded]);
            enemies.Add(enemyReferences[modded+3]);
            enemies.Add(enemyReferences[modded + 6]);
            enemies.Add(enemyReferences[modded + 9]);
        } else if (modded == 1) {
            enemies.Add(enemyReferences[modded]);
            enemies.Add(enemyReferences[modded + 3]);
            enemies.Add(enemyReferences[modded + 6]);
        } else {
            enemies.Add(enemyReferences[modded]);
            enemies.Add(enemyReferences[modded + 3]);
            enemies.Add(enemyReferences[modded + 6]);
        }
        enabled = true;

        levelHolder = new GameObject("Level").transform;

        SpawnEnemy();
    }

    public bool CheckVictory() {
        return enemiesLeft <= 0 && (!enabled || levelHolder.childCount == 0);
    }

    public void RemoveManager() {
        enabled = false;
        Destroy(levelHolder.gameObject);
    }
}
