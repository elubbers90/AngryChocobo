using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {
    public float minSpeed;
    public float maxSpeed;
    public int baseHp;

    public GameObject fireEffect;
    public GameObject waterEffect;
    
    [HideInInspector]
    public int startHp;
    [HideInInspector]
    public int currentHp;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public bool forceStopMove = false;

    [HideInInspector]
    public int damageOverTime = 0;
    [HideInInspector]
    public int damageOverTimeTaken = 0;
    [HideInInspector]
    public int damageOverTimeDuration = 0;
    private float damageOverTimeSpeed = 1f;

    [HideInInspector]
    public float slowMultiplier = 1;
    [HideInInspector]
    public int slowDuration = 0;
    private int slowIndex = 0;
    [HideInInspector]
    public float speedBefore;
    [HideInInspector]
    public bool GotHitByWater = false;

    protected override void Start() {
        size = transform.Find("Body").gameObject.GetComponent<Renderer>().bounds.size;
        base.Start();
        currentHp = baseHp * (int)Mathf.Ceil(GameManager.instance.level / 2f);
        startHp = currentHp;
        speed = Random.Range(minSpeed, maxSpeed);
        SetVelocity();

        animator = GetComponent<Animator>();
    }

    public virtual void SetVelocity() {
        if (!forceStopMove) {
            rb2D.velocity = transform.TransformDirection(Vector3.left * speed);
        } else {
            rb2D.velocity = Vector2.zero;
        }
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
        if (currentHp - damageOverTimeTaken > 0 && currentHp == hpAtHit) {
            SetVelocity();
        }
    }

    private IEnumerator WaitandRemove() {
        float waitTime = 0.5f;
        yield return new WaitForSeconds(waitTime);
        SpawnCandy();
        RemoveEnemy();
    }

    private void SpawnCandy() {
        Vector3 position = transform.position;
        GameObject candy = Instantiate(GameManager.instance.candyReference, position, Quaternion.identity) as GameObject;
        candy.transform.SetParent(GameManager.instance.levelManager.effectsHolder);
    }

    public virtual void TakeDamage() {
        rb2D.velocity = Vector2.zero;
        animator.SetTrigger("TakeDamage");
        StartCoroutine(WaitandMove());
    }

    public virtual IEnumerator TakeDamageOverTime() {
        int damageAtStart = damageOverTimeTaken;
        float waitTime = damageOverTimeSpeed;
        yield return new WaitForSeconds(waitTime);
        if (damageOverTimeTaken == damageAtStart) {
            damageOverTimeTaken += damageOverTime;
            damageOverTimeDuration--;
            if (currentHp - damageOverTimeTaken <= 0) {
                Die();
            } else if (damageOverTimeDuration > 0) {
                StartCoroutine(TakeDamageOverTime());
            } else {
                fireEffect.SetActive(false);
            }
        }
    }

    public virtual IEnumerator SetSlow(float newSlow) {
        slowIndex++;
        int currentSlowIndex = slowIndex;
        if (slowMultiplier == 1f) {
            slowMultiplier = newSlow;
            speedBefore = speed;
            speed *= slowMultiplier;
        }
        if (rb2D.velocity != Vector2.zero) {
            SetVelocity();
        }
        yield return new WaitForSeconds(slowDuration);
        if (currentSlowIndex == slowIndex) {
            waterEffect.SetActive(false);
            slowMultiplier = 1f;
            speed = speedBefore;

            if (rb2D.velocity != Vector2.zero) {
                SetVelocity();
            }
        }

    }

    public virtual void Die() {
        rb2D.velocity = Vector2.zero;
        gameObject.layer = LayerMask.NameToLayer("NonBlockingLayer");
        gameObject.tag = "DeadEnemy";
        animator.SetTrigger("Die");
        GameManager.instance.GiveCandy(startHp);
        StartCoroutine(WaitandRemove());
    }

    public void WaterHit(int waterduration, float slowMultiplier) {
        slowDuration = waterduration;
        int sortingOrder = transform.Find("Body").gameObject.GetComponent<Renderer>().sortingOrder;
        waterEffect.GetComponent<Renderer>().sortingOrder = sortingOrder + 1;
        waterEffect.SetActive(true);

        StartCoroutine(SetSlow(slowMultiplier));
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag.Contains("Egg") && currentHp - damageOverTimeTaken > 0) {
            if (collision.gameObject.tag == "Egg") {
                Egg eggScript = collision.gameObject.GetComponent<Egg>();
                currentHp -= eggScript.currentDamage;
            } else if (collision.gameObject.tag == "LightningEgg") {
                LightningEgg eggScript = collision.gameObject.GetComponent<LightningEgg>();
                currentHp -= eggScript.currentDamage;
                if (eggScript.lightningHit) {
                    currentHp -= eggScript.lightningDamage;
                }
            } else if (collision.gameObject.tag == "FireEgg") {
                FireEgg eggScript = collision.gameObject.GetComponent<FireEgg>();
                currentHp -= eggScript.currentDamage;
                damageOverTime = eggScript.fireDamage;
                damageOverTimeDuration = eggScript.fireDuration;
                damageOverTimeSpeed = eggScript.fireSpeed;
                fireEffect.SetActive(true);
                StartCoroutine(TakeDamageOverTime());
            } else if (collision.gameObject.tag == "EnergyEgg") {
                EnergyEgg eggScript = collision.gameObject.GetComponent<EnergyEgg>();
                currentHp -= eggScript.currentDamage;
            } else if (collision.gameObject.tag == "WaterEgg") {
                GotHitByWater = true;
                WaterEgg eggScript = collision.gameObject.GetComponent<WaterEgg>();
                currentHp -= eggScript.currentDamage;

                WaterHit(eggScript.waterduration, eggScript.slowMultiplier);
            }



            if (currentHp - damageOverTimeTaken <= 0) {
                Die();
            } else {
                TakeDamage();
            }
        }
    }

    public virtual void RemoveEnemy() {
        Destroy(gameObject);
        GameManager.instance.CheckVictory();
    }
}
