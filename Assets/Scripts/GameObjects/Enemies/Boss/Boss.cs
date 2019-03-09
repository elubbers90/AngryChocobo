using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovingDirection { Left, Up, Down }

public class Boss : Enemy {
    public GameObject faceIdle;
    public GameObject faceAngry;
    public GameObject faceHappy;
    public GameObject faceHurt;
    public GameObject faceAttack;

    public float attackSpeed;

    private GameObject currentFace;

    private float baseSpeed;
    private int currentStage;
    private float stopRange;

    private bool attacking = false;

    private MovingDirection currentDirection = MovingDirection.Left;

    protected override void Start() {
        base.Start();
        currentStage = 0;
        currentFace = faceHappy;

        stopRange = Random.Range(wrld.x / 4f, wrld.x / 1.25f);

        int sortingOrder = transform.Find("Body").gameObject.GetComponent<Renderer>().sortingOrder;
        faceIdle.GetComponent<Renderer>().sortingOrder = sortingOrder;
        faceAngry.GetComponent<Renderer>().sortingOrder = sortingOrder;
        faceHurt.GetComponent<Renderer>().sortingOrder = sortingOrder;
        faceAttack.GetComponent<Renderer>().sortingOrder = sortingOrder;

        StartCoroutine(WaitAndAttack());
    }

    public override void SetVelocity() {
        if (!forceStopMove) {
            if (currentDirection == MovingDirection.Left) {
                rb2D.velocity = transform.TransformDirection(Vector3.left * speed);
            } else if (currentDirection == MovingDirection.Up) {
                rb2D.velocity = transform.TransformDirection(Vector3.up * speed / 2);
            } else if (currentDirection == MovingDirection.Down) {
                rb2D.velocity = transform.TransformDirection(Vector3.down * speed / 2);
            }
        } else {
            rb2D.velocity = Vector2.zero;
        }
    }

    public override void Update() {
        if (currentDirection == MovingDirection.Left) {
            if (transform.position.x < (0 + stopRange)) {
                int chosenDirection = Random.Range(0, 1);
                if (chosenDirection == 0) {
                    currentDirection = MovingDirection.Down;
                } else {
                    currentDirection = MovingDirection.Up;
                }
                animator.SetFloat("WalkingSpeed", 0.5f);
                SetVelocity();
            }
        } else if (currentDirection == MovingDirection.Up) {
            if (transform.position.y > (wrld.y - (size.y * 2 ))) {
                currentDirection = MovingDirection.Down;
                SetVelocity();
            }
        } else if (currentDirection == MovingDirection.Down) {
            if (transform.position.y < (0 - wrld.y)) {
                currentDirection = MovingDirection.Up;
                SetVelocity();
            }
        }
    }

    private void SetBossFace() {
        if (attacking) {
            currentFace.SetActive(false);
            faceAttack.SetActive(true);
        } else {
            float newDamagePercentage = (float)(currentHp - damageOverTimeTaken) / (float)startHp;

            if (newDamagePercentage < 0.5f) {
                if (currentStage < 2) {
                    currentStage = 2;
                    currentFace.SetActive(false);
                    currentFace = faceAngry;
                    currentFace.SetActive(true);
                }
            } else if (newDamagePercentage < 0.75f) {
                if (currentStage == 0) {
                    currentStage = 1;
                    currentFace.SetActive(false);
                    currentFace = faceIdle;
                    currentFace.SetActive(true);
                }
            } else {

            }
        }
    }

    private IEnumerator WaitandResetFace() {
        int hpAtHit = currentHp;
        float waitTime = 0.5f;
        yield return new WaitForSeconds(waitTime);
        if (currentHp - damageOverTimeTaken > 0 && currentHp == hpAtHit) {
            currentFace.SetActive(true);
            faceHurt.SetActive(false);
        }
    }

    public override void TakeDamage() {
        SetBossFace();
        base.TakeDamage();

        currentFace.SetActive(false);
        faceHurt.SetActive(true);
        StartCoroutine(WaitandResetFace());
    }
     
    public override IEnumerator TakeDamageOverTime() {
        SetBossFace();
        yield return base.TakeDamageOverTime();
    }

    public override void Die() {
        currentFace.SetActive(false);
        faceHurt.SetActive(true);
        GameManager.instance.levelManager.SetBossDead();
        base.Die();
    }

    public IEnumerator WaitAndAttack() {
        float slowAttack = 2f - ((slowMultiplier - 0.1f) / (0.9f));

        yield return new WaitForSeconds(attackSpeed * slowAttack);
        if (currentHp > 0) {
            rb2D.velocity = Vector2.zero;
            attacking = true;
            SetBossFace();
            animator.SetTrigger("Attack");
            StartCoroutine(DoAttack());
            yield return new WaitForSeconds(0.5f);
            attacking = false;
            faceAttack.SetActive(false);
            if (currentHp > 0) {
                SetVelocity();
                currentFace.SetActive(true);
                StartCoroutine(WaitAndAttack());
            }
        }
    }

    public virtual IEnumerator DoAttack() {
        yield return null;
    }
}
