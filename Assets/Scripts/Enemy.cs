﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {
    public float minSpeed;
    public float maxSpeed;
    public int baseHp;

    private int currentHp;
    private float speed;
    private Animator animator;

    protected override void Start() {
        size = transform.Find("Body").gameObject.GetComponent<Renderer>().bounds.size;
        Vector3 scale = transform.localScale;
        size.x = size.x * scale.x * 1.5f;
        size.y = size.y * scale.y * 1.5f;
        base.Start();
        speed = Random.Range(minSpeed, maxSpeed);
        SetVelocity();
        currentHp = baseHp * (int) Mathf.Ceil(GameManager.instance.level / 2f);

        animator = GetComponent<Animator>();
    }

    private void SetVelocity() {
        rb2D.velocity = transform.TransformDirection(Vector3.left * speed);
    }

    private void Update() {
        if (transform.position.x < (0 - wrld.x - half_x)) {
            GameManager.instance.EnemyHit();
            RemoveEnemy();
        }
    }

    private IEnumerator WaitandMove() {
        int hpAtHit = currentHp;
        float waitTime = 0.5f;
        yield return new WaitForSeconds(waitTime);
        if (currentHp > 0 && currentHp == hpAtHit) {
            SetVelocity();
        }
    }

    private IEnumerator WaitandRemove() {
        float waitTime = 0.5f;
        yield return new WaitForSeconds(waitTime);
        RemoveEnemy();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Egg" && currentHp > 0) {
            Egg eggScript = collision.gameObject.GetComponent<Egg>();
            currentHp -= eggScript.currentDamage;
            if(currentHp <= 0) {
                rb2D.velocity = Vector2.zero;
                gameObject.layer = LayerMask.NameToLayer("NonBlockingLayer");
                gameObject.tag = "DeadEnemy";
                animator.SetTrigger("Die");
                StartCoroutine(WaitandRemove());
            } else {
                rb2D.velocity = Vector2.zero;
                animator.SetTrigger("TakeDamage");
                StartCoroutine(WaitandMove());
            }
        }
    }

    void RemoveEnemy() {
        Destroy(gameObject);
        GameManager.instance.CheckVictory();
    }
}
