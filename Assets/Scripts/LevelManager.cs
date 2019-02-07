using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour {
    public TileBase[] edgeTiles;
    public TileBase[] topBorderTiles;
    public TileBase[] topTiles;
    public TileBase[] centerTiles;
    public TileBase[] foregroundObjects;
    public TileBase[] treeTiles;

    private Tilemap background;
    private Tilemap foreground;
    private Tilemap treetops;
    public GameObject[] enemyReferences;

    public GameObject[] bunnyReferences;

    [HideInInspector]
    public Transform levelHolder;

    private Vector3 world;

    private List<GameObject> enemies;
    private int enemyAmount;
    private int enemiesLeft;
    private int bunnySpawn;

    public void Start() {
        background = GameObject.Find("Background").GetComponent<Tilemap>();
        foreground = GameObject.Find("Foreground").GetComponent<Tilemap>();
        treetops = GameObject.Find("TreeTops").GetComponent<Tilemap>();
        world = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
    }

    private IEnumerator WaitAndSpawn() {
        float waitTime = Random.Range(1.5f, 2f);
        yield return new WaitForSeconds(waitTime);
        SpawnEnemy();
    }

    private void SpawnEnemy() {
        if (enabled) {
            enemiesLeft--;
            GameObject toInstantiate;
            if (enemyAmount - bunnySpawn == enemiesLeft) {
                toInstantiate = bunnyReferences[Random.Range(0, bunnyReferences.Length)];
                bunnySpawn += Random.Range(10, 15);
            } else {
                toInstantiate = enemies[Random.Range(0, enemies.Count)];

            }

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

    private void SpawnBackground(int level) {
        background.ClearAllTiles();
        foreground.ClearAllTiles();

        int minBackgroundRange = (((int)Math.Floor((decimal)(level + 5) / 5)) % 4) * 5;
        Debug.Log("[minBackgroundRange " + minBackgroundRange);
        int chosenIndex = Random.Range(minBackgroundRange, minBackgroundRange + 5);

        int minForegroundRange = (((int)Math.Floor((decimal)(level + 5) / 5)) % 4) * 10;
        Debug.Log("[minForegroundRange " + minForegroundRange);
        int chosenForeground1 = Random.Range(minForegroundRange, minForegroundRange + 10);
        int chosenForeground2 = Random.Range(minForegroundRange, minForegroundRange + 10);
        while (chosenForeground2 == chosenForeground1) {
            chosenForeground2 = Random.Range(minForegroundRange, minForegroundRange + 10);
        }

        int left = (0 - (int)world.x - 1);
        int right = (int)world.x;
        int bottom = (0 - (int)world.y);
        int top = (int)world.y;

        for (int x = left; x <= right; x++){
            for (int y = bottom; y <= top; y++) {
                if (y == top) {
                    background.SetTile(new Vector3Int(x, y, 0), edgeTiles[chosenIndex]);
                } else if (y + 1 == top) {
                    background.SetTile(new Vector3Int(x, y, 0), topBorderTiles[chosenIndex]);
                } else if (y + 2 == top) {
                    background.SetTile(new Vector3Int(x, y, 0), topTiles[chosenIndex]);
                } else {
                    background.SetTile(new Vector3Int(x, y, 0), centerTiles[chosenIndex]);
                    if (y != bottom) {
                        if (Random.Range(0, 100) < 5) {
                            if (Random.Range(0, 2) == 0) {
                                foreground.SetTile(new Vector3Int(x, y, 0), foregroundObjects[chosenForeground1]);
                            } else {
                                foreground.SetTile(new Vector3Int(x, y, 0), foregroundObjects[chosenForeground2]);
                            }
                        }
                    }
                }
            }
        }


        treetops.SetTile(new Vector3Int(left, bottom, 0), treeTiles[minBackgroundRange]);
        treetops.SetTile(new Vector3Int(left + 1, bottom, 0), treeTiles[minBackgroundRange + 1]);
        treetops.SetTile(new Vector3Int(left + 2, bottom, 0), treeTiles[minBackgroundRange + 2]);
        treetops.SetTile(new Vector3Int(left + 3, bottom, 0), treeTiles[minBackgroundRange + 3]);
        treetops.SetTile(new Vector3Int(left + 4, bottom, 0), treeTiles[minBackgroundRange + 1]);
        treetops.SetTile(new Vector3Int(left + 5, bottom, 0), treeTiles[minBackgroundRange + 2]);
        treetops.SetTile(new Vector3Int(left + 6, bottom, 0), treeTiles[minBackgroundRange + 3]);
        treetops.SetTile(new Vector3Int(left + 7, bottom, 0), treeTiles[minBackgroundRange + 4]);

        treetops.SetTile(new Vector3Int(0, bottom, 0), treeTiles[minBackgroundRange]);
        treetops.SetTile(new Vector3Int(1, bottom, 0), treeTiles[minBackgroundRange + 1]);
        treetops.SetTile(new Vector3Int(2, bottom, 0), treeTiles[minBackgroundRange + 2]);
        treetops.SetTile(new Vector3Int(3, bottom, 0), treeTiles[minBackgroundRange + 3]);
        treetops.SetTile(new Vector3Int(4, bottom, 0), treeTiles[minBackgroundRange + 4]);

        treetops.SetTile(new Vector3Int(right - 5, bottom, 0), treeTiles[minBackgroundRange]);
        treetops.SetTile(new Vector3Int(right - 4, bottom, 0), treeTiles[minBackgroundRange + 1]);
        treetops.SetTile(new Vector3Int(right - 3, bottom, 0), treeTiles[minBackgroundRange + 2]);
        treetops.SetTile(new Vector3Int(right - 2, bottom, 0), treeTiles[minBackgroundRange + 3]);
        treetops.SetTile(new Vector3Int(right - 1, bottom, 0), treeTiles[minBackgroundRange + 4]);
    }

    public void SetupLevel(int level) {
        SpawnBackground(level);

        enemies = new List<GameObject>();
        enemyAmount = Random.Range(25, 35);

        enemiesLeft = enemyAmount;

        enemies.Add(GetEnemy(level, ((int)Math.Floor((decimal)level / 5)) % 3));
        enemies.Add(GetEnemy(level, 3 + ((int)Math.Floor((decimal)level / 7)) % 4));
        enemies.Add(GetEnemy(level, 6 + ((int)Math.Floor((decimal)level / 8)) % 5));

        bunnySpawn = Random.Range(5, 10);

        enabled = true;

        levelHolder = new GameObject("Level").transform;

        SpawnEnemy();
    }

    private GameObject GetEnemy(int level, int offset) {
        int result = (level + offset) % enemyReferences.Length;
        Debug.Log("Chosen enemy: " + result);
        return enemyReferences[result];
    }

    public bool CheckVictory() {
        return enemiesLeft <= 0 && (!enabled || levelHolder.childCount == 0);
    }

    public void RemoveManager() {
        enabled = false;
        Destroy(levelHolder.gameObject);
    }
}
