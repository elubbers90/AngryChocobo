using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {
    public float minSpeed;
    public float maxSpeed;
    public int baseHp;

    private int currentHp;
    private float speed;

    protected override void Start() {
        base.Start();
        speed = Random.Range(minSpeed, maxSpeed);
        rb2D.velocity = transform.TransformDirection(Vector3.left * speed);

        currentHp = baseHp * (int) Mathf.Ceil(GameManager.instance.level / 2);
    }

    private void Update() {
        if (transform.position.x < (0 - wrld.x - half_x)) {
            GameManager.instance.EnemyHit();
            RemoveEnemy();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Egg") {
            Egg eggScript = collision.gameObject.GetComponent<Egg>();
            currentHp -= eggScript.currentDamage;
            if(currentHp <= 0) {
                RemoveEnemy();
            }
        }
    }

    void RemoveEnemy() {
        Destroy(gameObject);
        GameManager.instance.CheckVictory();
    }
}
