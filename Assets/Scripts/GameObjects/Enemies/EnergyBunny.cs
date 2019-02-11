using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBunny : Bunny {
    private Vector3 world;

    public GameObject DisappearAnimation;
    public GameObject AppearAnimation;

    protected override void Start() {
        base.Start();

        world = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        StartCoroutine(WaitAndBlink());
    }

    private IEnumerator WaitAndBlink() {
        int waitTime = Random.Range(1, 3);
        yield return new WaitForSeconds(waitTime);
        if (currentHp - damageOverTimeTaken > 0) {
            StartCoroutine(Disappear());
        }
    }

    private IEnumerator Disappear() {
        AppearAnimation.SetActive(false);
        DisappearAnimation.SetActive(true);
        float waitTime = 0.17f;
        yield return new WaitForSeconds(waitTime);
        rb2D.velocity = Vector2.zero;
        transform.position = new Vector3(transform.position.x, world.y*2, transform.position.z);
        DisappearAnimation.SetActive(false);
        if (currentHp - damageOverTimeTaken > 0) {
            StartCoroutine(Appear());
        }
    }

    private IEnumerator Appear() {
        float waitTime = 0.5f;
        yield return new WaitForSeconds(waitTime);
        if (currentHp - damageOverTimeTaken > 0) {
            AppearAnimation.SetActive(true);
            float y = Random.Range((0 - world.y + 2f), (world.y - 2f));
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            SetVelocity();
            StartCoroutine(WaitAndBlink());
        }
    }
}
