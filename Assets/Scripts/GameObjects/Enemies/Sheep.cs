using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour {
    public GameObject horns;
    public Sprite horns1;
    public Sprite horns2;

    void Start() {
        SpriteRenderer renderer = horns.GetComponent<SpriteRenderer>();
        int choice = Random.Range(0, 3);
        if (choice == 0) {
            renderer.sprite = horns1;
        } else if(choice == 1) {
            renderer.sprite = horns2;
        }
    }
}