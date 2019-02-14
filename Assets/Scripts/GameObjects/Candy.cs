using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour {
    public Sprite Candy1;
    public Sprite Candy2;
    public Sprite Candy3;

    // Start is called before the first frame update
    void Start() {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        int choice = Random.Range(0, 3);
        if (choice == 0) {
            renderer.sprite = Candy1;
        } else if (choice == 1) {
            renderer.sprite = Candy2;
        } else if (choice == 2) {
            renderer.sprite = Candy3;
        }

        StartCoroutine(WaitAndDestroy());
    }

    private IEnumerator WaitAndDestroy() {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
