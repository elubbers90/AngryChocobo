using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : Enemy {
    public GameObject specialEffect;
    public int eggType;

    public override void Die() {
        base.Die();
        specialEffect.SetActive(false);
    }

    public override void RemoveEnemy() {
        base.RemoveEnemy();

        GameManager.instance.ActivateEgg(eggType);
    }
}
