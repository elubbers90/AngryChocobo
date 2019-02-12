using UnityEngine;
using System.Collections;

public class LightningEgg : Egg {
    public bool lightningHit;
    [HideInInspector]
    public int lightningDamage = 5;

    public GameObject specialDeathEffect;
    private GameObject specialDeathEffectInstance;

    protected override void Start() {
        base.Start();
    }

    public override void SetMovingSpeed() {
        movingSpeed = SaveSystem.GetFloat("lightningEggSpeed", 15f);
    }

    public override void SetDamage() {
        currentDamage = SaveSystem.GetInt("lightningEggDamage", 4);
    }

    public override IEnumerator WaitandRemove() {
        if (lightningHit) {
            Vector3 size = specialDeathEffect.GetComponent<Renderer>().bounds.size;
            Vector3 position = transform.position;
            position.y += (size.y / 4);
            specialDeathEffectInstance = Instantiate(specialDeathEffect, position, Quaternion.identity) as GameObject;
            specialDeathEffectInstance.transform.SetParent(GameManager.instance.levelManager.levelHolder);
        }

        yield return base.WaitandRemove();

        if (specialDeathEffectInstance != null) {
            Destroy(specialDeathEffectInstance);
        }
    }
}
