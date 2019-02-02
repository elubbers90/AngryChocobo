using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;

public class Player : MovingObject
{
    public GameObject[] eggTypes;

    [HideInInspector]
    public int selectedEggType = 1;
    private float currentEggSpeed;
    [HideInInspector]
    public Animator animator;

    protected override void Start() {
        Bounds bounds = GetComponent<Renderer>().bounds;
        size = bounds.size;
        base.Start();
        animator = GetComponent<Animator>();

        SwitchEggType();
        SpawnEgg();
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
            float speed = 0.2f;
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
            SwitchEggType();
        }
    }

    public void SwitchEggType() {
        selectedEggType = 1 - selectedEggType;
        currentEggSpeed = eggTypes[selectedEggType].GetComponent<Egg>().firingSpeed;
        animator.SetFloat("ShootingSpeed", 1 / currentEggSpeed);
        GameManager.instance.uiManager.ShowEggType(selectedEggType);
    }

    private IEnumerator WaitAndSpawn() {
        yield return new WaitForSeconds(currentEggSpeed);
        SpawnEgg();
    }

    private void SpawnEgg() {
        GameObject instance = Instantiate(eggTypes[selectedEggType], transform.position, Quaternion.identity) as GameObject;

        instance.transform.SetParent(transform);
        instance.transform.position = new Vector3(instance.transform.position.x + 0.7f, instance.transform.position.y - 0.1f, 0f);
        instance.transform.localScale = new Vector3(2f, 2f, 1f);

        StartCoroutine(WaitAndSpawn());
    }

    private void Restart() {
        SceneManager.LoadScene(0);
    }

}
