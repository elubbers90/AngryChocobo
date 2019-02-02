using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEnemy : Enemy {
    public GameObject mouthObject1;
    public GameObject mouthObject2;
    public GameObject headObject;
    public GameObject eyeObject1;
    public GameObject eyeObject2;

    public Sprite head2;
    public Sprite mouth1;
    public Sprite mouth2;
    public Sprite mouth3;

    public bool hideMouth1;

    private float baseSpeed;
    private int currentStage;

    protected override void Start() {
        base.Start();
        baseSpeed = speed;
        eyeObject2.SetActive(false);

        currentStage = 0;
    }

    public override void TakeDamage() {
        float newDamagePercentage = (float)currentHp / (float)baseHp;

        if (newDamagePercentage < 0.5f) {
            if (currentStage < 3) {
                currentStage = 3;
                speed = baseSpeed * 4;
                if (hideMouth1) {
                    mouthObject1.SetActive(true);
                }
                mouthObject1.GetComponent<SpriteRenderer>().sprite = mouth3;
                headObject.GetComponent<SpriteRenderer>().sprite = head2;
                eyeObject2.SetActive(true);
                eyeObject1.SetActive(false);
                animator.SetFloat("WalkingSpeed", 1.2f);
            }
        } else if (newDamagePercentage < 0.75f) {
            if (currentStage < 2) {
                currentStage = 2;
                speed = baseSpeed * 3;
                if (hideMouth1) {
                    mouthObject1.SetActive(true);
                }
                mouthObject1.GetComponent<SpriteRenderer>().sprite = mouth2;
                animator.SetFloat("WalkingSpeed", 1.1f);
            }
        } else {
            if (currentStage == 0) {
                currentStage = 1;
                speed = baseSpeed * 2;
                mouthObject2.SetActive(false);
                if (hideMouth1) {
                    mouthObject1.SetActive(false);
                } else {
                    mouthObject1.GetComponent<SpriteRenderer>().sprite = mouth1;
                }
                animator.SetFloat("WalkingSpeed", 1.025f);
            }
        }
        base.TakeDamage();
    }
}
