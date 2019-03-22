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
    public GameObject waterEffect;

    public float eggYOffset;
    public float eggXOffset;

    [HideInInspector]
    public int selectedEggType = 0;
    private float currentEggSpeed;
    [HideInInspector]
    public Animator animator;
    private bool takingDamage = false;

    private int eggsToShoot;
    private int currentDamageTrigger;

    protected override void Start() {
        Bounds bounds = transform.Find("Body").GetComponent<Renderer>().bounds;
        size = bounds.size;
        base.Start();
        animator = GetComponent<Animator>();

        currentDamageTrigger = 0;
        selectedEggType = 0;
        currentEggSpeed = 0.45f - ((SaveSystem.GetFloat("basicEggSpeed", 10f) - 10f) / 50f);
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
        } else if (selectedEggType == 4) {
            waterEffect.SetActive(false);
        }

        selectedEggType = type;

        switch (selectedEggType) {
            case 1:
                eggsToShoot = SaveSystem.GetInt("lightningEggAmount", 20);
                currentEggSpeed = 0.25f - ((SaveSystem.GetFloat("lightningEggSpeed", 10f) - 10f) / 100f);
                lightningEffect.SetActive(true);
                break;
            case 2:
                eggsToShoot = SaveSystem.GetInt("fireEggAmount", 15);
                currentEggSpeed = 0.45f - ((SaveSystem.GetFloat("fireEggSpeed", 10f) - 10f) / 50f);
                fireEffect.SetActive(true);
                break;
            case 3:
                eggsToShoot = SaveSystem.GetInt("energyEggAmount", 25);
                currentEggSpeed = 0.2f - ((SaveSystem.GetFloat("energyEggSpeed", 10f) - 10f) / 150f);
                energyEffect.SetActive(true);
                break;
            case 4:
                eggsToShoot = SaveSystem.GetInt("waterEggAmount", 15);
                currentEggSpeed = 0.45f - ((SaveSystem.GetFloat("waterEggSpeed", 10f) - 10f) / 50f);
                waterEffect.SetActive(true);
                break;
            default:
                currentEggSpeed = 0.45f - ((SaveSystem.GetFloat("basicEggSpeed", 10f) - 10f) / 50f);
                break;
        }
        animator.SetFloat("ShootingSpeed", 1 / currentEggSpeed);
    }

    private IEnumerator WaitAndSpawn() {
        yield return new WaitForSeconds(currentEggSpeed);
        if (takingDamage) {
            StartCoroutine(WaitAndSpawn());
        } else {
            SpawnEgg();
        }
    }

    private void SpawnEgg() {
        GameObject instance = Instantiate(eggTypes[selectedEggType], transform.position, Quaternion.identity) as GameObject;

        instance.transform.SetParent(transform);
        instance.transform.position = new Vector3(instance.transform.position.x + eggXOffset, instance.transform.position.y - eggYOffset, 0f);
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

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Yarn") {
            StartCoroutine(TakeDamage());
        } else if (collision.gameObject.tag == "Honey") {
            StartCoroutine(TakeDamage());
        }
    }

    private IEnumerator TakeDamage() {
        currentDamageTrigger++;
        int damageTrigger = currentDamageTrigger;
        takingDamage = true;
        animator.SetFloat("ShootingSpeed", 0);
        animator.SetTrigger("TakeDamage");
        GameManager.instance.EnemyHit();
        yield return new WaitForSeconds(0.5f);
        if (currentDamageTrigger == damageTrigger) {
            takingDamage = false;
            animator.SetFloat("ShootingSpeed", 1 / currentEggSpeed);
        }
    }
}
