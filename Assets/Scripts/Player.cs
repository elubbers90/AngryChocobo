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
    public GameObject lightningEffect;
    public GameObject fireEffect;
    public GameObject energyEffect;

    [HideInInspector]
    public int selectedEggType = 0;
    private float currentEggSpeed;
    [HideInInspector]
    public Animator animator;

    private int lightningEggsAmount = 20;
    private int fireEggsAmount = 15;
    private int energyEggsAmount = 25;
    private int waterEggsAmount = 10; 
    private int eggsToShoot;

    protected override void Start() {
        Bounds bounds = GetComponent<Renderer>().bounds;
        size = bounds.size;
        base.Start();
        animator = GetComponent<Animator>();

        selectedEggType = 0;
        currentEggSpeed = 0.5f;
        animator.SetFloat("ShootingSpeed", 1 / currentEggSpeed);
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
            ActivateEgg((selectedEggType + 1) % 5);
        }
    }

    public void ActivateEgg(int type) {
        if (selectedEggType == 1) {
            lightningEffect.SetActive(false);
        } else if (selectedEggType == 2) {
            fireEffect.SetActive(false);
        } else if (selectedEggType == 3) {
            energyEffect.SetActive(false);
        }

        selectedEggType = type;

        switch (selectedEggType) {
            case 1:
                eggsToShoot = lightningEggsAmount;
                currentEggSpeed = 0.25f;
                lightningEffect.SetActive(true);
                break;
            case 2:
                eggsToShoot = fireEggsAmount;
                currentEggSpeed = 0.4f;
                fireEffect.SetActive(true);
                break;
            case 3:
                eggsToShoot = energyEggsAmount;
                currentEggSpeed = 0.2f;
                energyEffect.SetActive(true);
                break;
            case 4:
                eggsToShoot = waterEggsAmount;
                currentEggSpeed = 0.5f;
                break;
            default:
                currentEggSpeed = 0.5f;
                break;
        }
        animator.SetFloat("ShootingSpeed", 1 / currentEggSpeed);
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

        eggsToShoot--;
        if (selectedEggType != 0 && eggsToShoot <= 0) {
            ActivateEgg(0);
        }

        StartCoroutine(WaitAndSpawn());
    }

    private void Restart() {
        SceneManager.LoadScene(0);
    }

}
