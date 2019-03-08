using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearBoss : Boss{
    public GameObject projectileReference;

    public override IEnumerator DoAttack() {
        yield return new WaitForSeconds(0.2f);
        GameObject instance = Instantiate(projectileReference, new Vector3(transform.position.x, transform.position.y + 1.5f, 0f), Quaternion.identity) as GameObject;

        instance.transform.SetParent(GameManager.instance.levelManager.effectsHolder);
    }
}
