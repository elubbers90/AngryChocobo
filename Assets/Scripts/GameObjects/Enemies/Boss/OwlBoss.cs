using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlBoss : Boss{
    public int minionsAmount;
    public float minionsDelay;
    
    public override IEnumerator DoAttack() {
        for (int i = 0; i < minionsAmount; i++) {
            GameManager.instance.levelManager.SpawnExtraMinion();

            yield return new WaitForSeconds(minionsDelay);
        }
    }
}
