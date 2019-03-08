using UnityEngine;
using System.Collections;

public class Projectile : MovingObject {
    [HideInInspector]
    public Animator animator;

    public float speed;

    private Vector3 playerPos;
    private Vector3 direction;
    private bool exploded = false;

    protected override void Start() {
        base.Start();
        playerPos = GameManager.instance.GetPlayerPosition();
        direction = playerPos - transform.position;
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (!exploded) {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, playerPos + direction, step);
        }
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
            exploded = true;
            gameObject.layer = LayerMask.NameToLayer("NonBlockingLayer");
            animator.SetTrigger("Explode");
            StartCoroutine(WaitandRemove());
        }
    }
}
