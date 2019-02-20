using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EnergyEgg : Egg {
    [HideInInspector]
    public float teleportRange;
    [HideInInspector]
    public float teleportDelay;

    public GameObject DisappearAnimation;
    public GameObject AppearAnimation;

    private Vector3 world;

    private float chosenY;

    protected override void Start() {
        teleportRange = SaveSystem.GetFloat("energyEggRadius", 1.5f);
        teleportDelay = SaveSystem.GetFloat("energyEggDelay", 1.5f);
        base.Start();

        world = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));

        SelectEnemy();
    }

    public override void SetMovingSpeed() {
        movingSpeed = SaveSystem.GetFloat("energyEggSpeed", 10f);
    }

    public override void SetDamage() {
        currentDamage = SaveSystem.GetInt("energyEggDamage", 4);
    }

    private void SelectEnemy() {
        float enemy = 999f;
        Transform levelHolder = GameManager.instance.levelManager.levelHolder;
        float currentY = transform.position.y;
        foreach (Transform child in levelHolder) {
            if(child.gameObject.tag == "Enemy") {
                if (transform.position.x < child.position.x - 1f) {
                    Vector3 size = child.Find("Body").gameObject.GetComponent<Renderer>().bounds.size;
                    float childCenter = child.position.y + (size.y / 2);
                    if ((currentY >= childCenter && currentY - teleportRange <= childCenter) ||
                        (currentY <= childCenter && currentY + teleportRange >= childCenter)) {
                        if (Math.Abs(currentY - childCenter) < enemy) {
                            enemy = childCenter;
                        }
                    }
                }
            }
        }

        if (enemy != 999) {
            chosenY = enemy;
            StartCoroutine(Disappear());
        } else {
            StartCoroutine(WaitAndTryAgain());
        }
    }
    
    private IEnumerator WaitAndTryAgain() {
        float waitTime = 1f;
        yield return new WaitForSeconds(waitTime);
        SelectEnemy();
    }

    private IEnumerator Disappear() {
        yield return new WaitForSeconds(0.05f);

        DisappearAnimation.SetActive(true);
        float waitTime = 0.17f;
        yield return new WaitForSeconds(waitTime);
        rb2D.velocity = Vector2.zero;
        transform.position = new Vector3(transform.position.x, world.y * 2, transform.position.z);
        DisappearAnimation.SetActive(false);
        yield return new WaitForSeconds(teleportDelay);
        Appear();
    }

    private void Appear() {
        AppearAnimation.SetActive(true);
        transform.position = new Vector3(transform.position.x, chosenY, transform.position.z);
        movingSpeed = movingSpeed + 10;
        rb2D.velocity = transform.TransformDirection(Vector3.right * movingSpeed);
    }
}
