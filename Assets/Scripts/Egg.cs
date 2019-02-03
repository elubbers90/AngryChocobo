using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MovingObject { 
    public float movingSpeed;
    public float firingSpeed;
    public int baseDamage;
    public int eggAmount;

    [HideInInspector]
    public int currentDamage;
    [HideInInspector]
    public Animator animator;
    public GameObject specialDeathEffect;
    private GameObject specialDeathEffectInstance;

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

    private IEnumerator WaitandRemove() {
        float waitTime = 0.5f;
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
        if (specialDeathEffectInstance != null) {
            Destroy(specialDeathEffectInstance);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        rb2D.velocity = Vector2.zero;
        gameObject.layer = LayerMask.NameToLayer("NonBlockingLayer");
        //gameObject.tag = "ExplodingEgg";
        animator.SetTrigger("Explode");
        if (specialDeathEffect != null) {
            Vector3 size = specialDeathEffect.GetComponent<Renderer>().bounds.size;
            Vector3 position = transform.position;
            position.y += (size.y / 4);
            specialDeathEffectInstance = Instantiate(specialDeathEffect, position, Quaternion.identity) as GameObject;
            specialDeathEffectInstance.transform.SetParent(GameManager.instance.levelManager.levelHolder);
        }
        StartCoroutine(WaitandRemove());
    }
}
