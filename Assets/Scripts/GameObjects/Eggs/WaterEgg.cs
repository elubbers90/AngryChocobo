using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterEgg : Egg {
    [HideInInspector]
    public float slowMultiplier;

    [HideInInspector]
    public int waterduration = 5;

    [HideInInspector]
    public float waterSplashRadius;

    public GameObject specialDeathEffect;
    private List<GameObject> specialDeathEffectInstances;


    protected override void Start() {
        specialDeathEffectInstances = new List<GameObject>();
        waterSplashRadius = SaveSystem.GetFloat("waterEggRadius", 1f);
        slowMultiplier = SaveSystem.GetFloat("waterEggSlow", 0.5f);
        base.Start();
    }

    public override void SetMovingSpeed() {
        movingSpeed = SaveSystem.GetFloat("waterEggSpeed", 10f);
    }

    public override void SetDamage() {
        currentDamage = SaveSystem.GetInt("waterEggDamage", 3);
    }

    private void HitSurroundingEnemies() {
        Transform levelHolder = GameManager.instance.levelManager.levelHolder;
        float currentY = transform.position.y;
        float currentX = transform.position.x;
        Vector3 effectSize = specialDeathEffect.GetComponent<Renderer>().bounds.size;
        foreach (Transform child in levelHolder) {
            if (child.gameObject.tag == "Enemy") {
                Vector3 size = child.Find("Body").gameObject.GetComponent<Renderer>().bounds.size;
                float childCenterY = child.position.y + (size.y / 2);
                float childCenterX = child.position.x + (size.x / 2);
                if ((currentY >= childCenterY && currentY - waterSplashRadius <= childCenterY) ||
                    (currentY <= childCenterY && currentY + waterSplashRadius >= childCenterY)) {
                    if ((currentX >= childCenterX && currentX - waterSplashRadius <= childCenterX) ||
                        (currentX <= childCenterX && currentX + waterSplashRadius >= childCenterX)) {
                        Enemy enemyScript = child.gameObject.GetComponent<Enemy>();
                        if (enemyScript.GotHitByWater) {
                            enemyScript.GotHitByWater = false;
                        } else {
                            Vector3 position = child.position;
                            position.y = childCenterY + (effectSize.y / 4);
                            GameObject specialDeathEffectInstance = Instantiate(specialDeathEffect, position, Quaternion.identity) as GameObject;
                            specialDeathEffectInstance.transform.SetParent(GameManager.instance.levelManager.effectsHolder);
                            specialDeathEffectInstances.Add(specialDeathEffectInstance);
                            enemyScript.WaterHit(waterduration, slowMultiplier);
                        }
                    }
                }
            }
        }
    }

    public override IEnumerator WaitandRemove() {
        Vector3 size = specialDeathEffect.GetComponent<Renderer>().bounds.size;
        Vector3 position = transform.position;
        position.y += (size.y / 4);
        position.x += (size.x / 8);
        GameObject specialDeathEffectInstance = Instantiate(specialDeathEffect, position, Quaternion.identity) as GameObject;
        specialDeathEffectInstance.transform.SetParent(GameManager.instance.levelManager.effectsHolder);
        specialDeathEffectInstances.Add(specialDeathEffectInstance);

        yield return new WaitForSeconds(0.05f);

        HitSurroundingEnemies();

        yield return base.WaitandRemove();

        foreach (GameObject effect in specialDeathEffectInstances) {
            Destroy(effect);
        }
    }
}
