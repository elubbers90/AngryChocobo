using UnityEngine;
using System.Collections;

public class FireEgg : Egg {
    [HideInInspector]
    public int fireDamage;
    [HideInInspector]
    public float fireSpeed;
    [HideInInspector]
    public int fireDuration = 10;

    protected override void Start() {
        base.Start();
        fireSpeed = SaveSystem.GetFloat("fireEggFireSpeed", 1f);
        fireDamage = SaveSystem.GetInt("fireEggFireDamage", 1);
    }


    public override void SetMovingSpeed() {
        movingSpeed = SaveSystem.GetFloat("fireEggSpeed", 10f);
    }

    public override void SetDamage() {
        currentDamage = SaveSystem.GetInt("fireEggDamage", 3);
    }

}
