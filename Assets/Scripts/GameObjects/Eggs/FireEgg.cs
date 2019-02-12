using UnityEngine;
using System.Collections;

public class FireEgg : Egg {
    [HideInInspector]
    public int fireDamage = 1;
    
    [HideInInspector]
    public int fireDuration = 10;

    public override void SetMovingSpeed() {
        movingSpeed = SaveSystem.GetFloat("fireEggSpeed", 10f);
    }

    public override void SetDamage() {
        currentDamage = SaveSystem.GetInt("fireEggDamage", 3);
    }

}
