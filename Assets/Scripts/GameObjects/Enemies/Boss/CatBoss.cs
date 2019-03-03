using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBoss : Boss{
    public GameObject yarnReference;

    public int yarnAmount;

    public override IEnumerator DoAttack() {

        for (int i = 0; i < yarnAmount; i++) {
            float y = Random.Range((0 - wrld.y), (wrld.y));

            GameObject instance = Instantiate(yarnReference, new Vector3(wrld.x + 1f, y, 0f), Quaternion.identity) as GameObject;

            instance.transform.SetParent(GameManager.instance.levelManager.effectsHolder);

            yield return new WaitForSeconds(1f);
        }
    }
}
