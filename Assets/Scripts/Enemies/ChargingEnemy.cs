using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemy : Enemy {
    public float minChargeTime;
    public float maxChargeTime;
    private float baseSpeed;

    protected override void Start() {
        base.Start();
        baseSpeed = speed;
        StartCoroutine(StartRandomChargeTimer());
    }

    private IEnumerator StartRandomChargeTimer() {
        float waitTime = Random.Range(minChargeTime, maxChargeTime);
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(WaitChargeAndMove());
    }

    private IEnumerator WaitChargeAndMove() {
        forceStopMove = true;
        rb2D.velocity = Vector2.zero;
        float waitTime = 1.2f;
        animator.SetTrigger("Charge");
        yield return new WaitForSeconds(waitTime);
        speed = baseSpeed * 2;
        speedBefore = speed;
        speed *= slowMultiplier;
        if (currentHp > 0) {
            forceStopMove = false;
            SetVelocity();
        }
    }
}
