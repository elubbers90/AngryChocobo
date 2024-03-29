﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Player : MovingObject
{
    public GameObject[] eggTypes;

    private int selectedEggType = 0;

    protected override void Start() {
        base.Start();
        StartCoroutine(WaitAndSpawn());
    }

    private void FixedUpdate() {
        Vector2 start = transform.position;

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
        float vertical = 0;

        //Store the current vertical input in the float moveVertical.
        vertical = Input.GetAxis("Vertical");

        if (vertical != 0) {
            MoveTo(start + new Vector2(0, vertical * 0.25f));
        }

#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

        if (Input.touchCount > 0) {
            Touch myTouch = Input.touches[0];
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(myTouch.position);
            Vector3 end = new Vector3(start.x, worldPos.y, 0);
            float speed = 0.4f;
            if (end.y - start.y > speed){
                end.y = start.y + speed;
            } else if (end.y - start.y < -speed){
                end.y = start.y - speed;
            }
            MoveTo(end);
        }

#endif
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            selectedEggType = 1 - selectedEggType;
        }
    }

    private IEnumerator WaitAndSpawn() {
        yield return new WaitForSeconds(eggTypes[selectedEggType].GetComponent<Egg>().firingSpeed);
        SpawnEgg();
    }

    private void SpawnEgg() {
        GameObject instance = Instantiate(eggTypes[selectedEggType], transform.position, Quaternion.identity) as GameObject;

        instance.transform.SetParent(transform);

        StartCoroutine(WaitAndSpawn());
    }

    private void Restart() {
        SceneManager.LoadScene(0);
    }

}
