using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour {
    public GameObject[] enemyReferences;

    [HideInInspector]
    public Transform levelHolder;

    private Vector3 world;

    private List<GameObject> enemies;
    private int enemyAmount;

    private IEnumerator WaitAndSpawn() {
        float waitTime = Random.Range(1.5f, 3f);
        yield return new WaitForSeconds(waitTime);
        SpawnEnemy();
    }

    private void SpawnEnemy() {
        if (enabled) {
            enemyAmount--;
            GameObject toInstantiate = enemies[Random.Range(0, enemies.Count)];

            float x = world.x + 1f;
            float y = Random.Range((0 - world.y + 2f), (world.y - 2f));

            GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

            instance.transform.SetParent(levelHolder);

            if (enemyAmount > 0) {
                StartCoroutine(WaitAndSpawn());
            }
        }
    }

    public void SetupLevel(int level) {
        enemies = new List<GameObject>();
        
        int modded = level % 2;
        if (modded == 0) {
            enemyAmount = 5;
            enemies.Add(enemyReferences[modded]);
        } else {
            enemyAmount = 10;
            enemies.Add(enemyReferences[modded]);
        }

        enabled = true;

        world = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        levelHolder = new GameObject("Level").transform;

        SpawnEnemy();
    }

    public bool CheckVictory() {
        return enemyAmount <= 0 && (!enabled || levelHolder.childCount == 0);
    }

    public void RemoveManager() {
        enabled = false;
        Destroy(levelHolder.gameObject);
    }
}
