using UnityEngine;
using System.Collections;

public class Yarn : MovingObject {
    [HideInInspector]
    public Animator animator;

    protected override void Start() {
        base.Start();
        rb2D.velocity = transform.TransformDirection(Vector3.left * 5f);
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (transform.position.x < (0 - wrld.x - half_x)) {
            Destroy(gameObject);
        }
    }

    public virtual IEnumerator WaitandRemove() {
        float waitTime = 0.5f;
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            rb2D.velocity = Vector2.zero;
            gameObject.layer = LayerMask.NameToLayer("NonBlockingLayer");
            animator.SetTrigger("Explode");
            StartCoroutine(WaitandRemove());
        }
    }
}
