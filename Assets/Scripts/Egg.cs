using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MovingObject { 
    public float movingSpeed;
    public float firingSpeed;
    public int baseDamage;

    [HideInInspector]
    public int currentDamage;

    protected override void Start() {
        base.Start();
        rb2D.velocity = transform.TransformDirection(Vector3.right * movingSpeed);

        currentDamage = baseDamage;
    }

    private void FixedUpdate() {
        if (transform.position.x > (wrld.x + half_x)) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }
}
