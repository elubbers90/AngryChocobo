using UnityEngine;
using System.Collections;

public class WaterEgg : Egg {
    [HideInInspector]
    public float slowMultiplier = 0.5f;

    [HideInInspector]
    public int waterduration = 5;

    public GameObject specialDeathEffect;
    private GameObject specialDeathEffectInstance;

    public override IEnumerator WaitandRemove() {
        Vector3 size = specialDeathEffect.GetComponent<Renderer>().bounds.size;
        Vector3 position = transform.position;
        position.y += (size.y / 4);
        position.x += (size.x / 8);
        specialDeathEffectInstance = Instantiate(specialDeathEffect, position, Quaternion.identity) as GameObject;
        specialDeathEffectInstance.transform.SetParent(GameManager.instance.levelManager.levelHolder);

        yield return base.WaitandRemove();

        Destroy(specialDeathEffectInstance);
    }
}
