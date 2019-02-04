using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MovingObject {
    [HideInInspector]
    public float movingSpeed = 10;
    [HideInInspector]
    public int baseDamage = 1;

    [HideInInspector]
    public int currentDamage;
    [HideInInspector]
    public Animator animator;

    protected override void Start() {
        base.Start();
        rb2D.velocity = transform.TransformDirection(Vector3.right * movingSpeed);

        currentDamage = baseDamage;

        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        if (transform.position.x > (wrld.x + half_x)) {
            Destroy(gameObject);
        }
    }

    public virtual IEnumerator WaitandRemove() {
        float waitTime = 0.5f;
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        rb2D.velocity = Vector2.zero;
        gameObject.layer = LayerMask.NameToLayer("NonBlockingLayer");
        animator.SetTrigger("Explode");
        StartCoroutine(WaitandRemove());
    }
}
