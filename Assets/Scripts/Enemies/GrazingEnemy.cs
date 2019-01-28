using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrazingEnemy : Enemy {
    public float minGrazeTime;
    public float maxGrazeTime;

    protected override void Start() {
        base.Start();
    }

    private IEnumerator StartRandomGrazeTimer() {
        float waitTime = Random.Range(minGrazeTime, maxGrazeTime);
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(WaitGrazeAndMove());
    }

    private IEnumerator WaitGrazeAndMove() {
        int hpAtHit = currentHp;
        float waitTime = 5f;
        animator.SetTrigger("Graze");
        rb2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(waitTime);
        if (currentHp > 0 && currentHp == hpAtHit) {
            SetVelocity();
        }
    }

    public override void SetVelocity() { 
        rb2D.velocity = transform.TransformDirection(Vector3.left * speed);
        StartCoroutine(StartRandomGrazeTimer());
    }
}
