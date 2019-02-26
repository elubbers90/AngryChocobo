using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour {
    private OpenUpgradeButton ActiveEgg;
    public GameObject BasicEgg;

    public void OpenBasicEgg() {
        BasicEgg.GetComponent<OpenUpgradeButton>().Open();
    }

    public void SetEggActive(OpenUpgradeButton button) {
        CloseCurrentEgg();
        ActiveEgg = button;
    }

    public void CloseCurrentEgg() {
        if (ActiveEgg) {
            ActiveEgg.Close();
            ActiveEgg = null;
        }
    }

    internal int GetCost(int eggType, UpgradeType upgradeType, SpecialUpgradeType specialUpgradeType) {
        switch (upgradeType) {
            case UpgradeType.Purchase:
                switch (GameManager.instance.purchasedEggs.Count) {
                    case 1:
                        return 1;
                    case 2:
                        return 10;
                    case 3:
                        return 25;
                    case 4:
                    default:
                        return 50;
                }
            case UpgradeType.Damage:
                return GetDamageCost(eggType);
            case UpgradeType.Speed:
                return GetSpeedCost(eggType);
            case UpgradeType.EggAmount:
                return GetAmountCost(eggType);
            case UpgradeType.SpecialUpgrade:
                return GetSpecialCost(specialUpgradeType);
        }
        return 0;
    }

    private int GetDamageCost(int eggType) {
        switch (eggType) {
            case 0:
                return (SaveSystem.GetInt("basicEggDamage", 2) - 1) * 50;
            case 1:
                return (SaveSystem.GetInt("lightningEggDamage", 4) - 3) * 40;
            case 2:
                return (SaveSystem.GetInt("fireEggDamage", 3) - 2) * 45;
            case 3:
                return (SaveSystem.GetInt("energyEggDamage", 4) - 3) * 40;
            case 4:
                return (SaveSystem.GetInt("waterEggDamage", 3) - 2) * 45;
        }
        return 0;
    }

    private int GetSpeedCost(int eggType) {
        switch (eggType) {
            case 0:
                return (int)(Mathf.Round((SaveSystem.GetFloat("basicEggSpeed", 10f) -9.9f) * 10)) * 75;
            case 1:
                return (int)(Mathf.Round((SaveSystem.GetFloat("lightningEggSpeed", 15f) - 14.9f) * 10)) * 50;
            case 2:
                return (int)(Mathf.Round((SaveSystem.GetFloat("fireEggSpeed", 10f) - 9.9f) * 10)) * 75;
            case 3:
                return (int)(Mathf.Round((SaveSystem.GetFloat("energyEggSpeed", 10f) - 9.9f) * 10)) * 75;
            case 4:
                return (int)(Mathf.Round((SaveSystem.GetFloat("waterEggSpeed", 10f) - 9.9f) * 10)) * 75;
        }
        return 0;
    }

    private int GetAmountCost(int eggType) {
        switch (eggType) {
            case 1:
                return (SaveSystem.GetInt("lightningEggAmount", 20) - 19) * 100;
            case 2:
                return (SaveSystem.GetInt("fireEggAmount", 15) - 14) * 125;
            case 3:
                return (SaveSystem.GetInt("energyEggAmount", 25) - 24) * 75;
            case 4:
                return (SaveSystem.GetInt("waterEggAmount", 15) - 14) * 125;
        }
        return 0;
    }

    private int GetSpecialCost(SpecialUpgradeType specialUpgradeType) {
        switch (specialUpgradeType) {
            case SpecialUpgradeType.LightningBolt:
                return 5000;
            case SpecialUpgradeType.LightningBoltDamage:
                return (SaveSystem.GetInt("lightningEggBoltDamage", 5) - 4) * 40;
            case SpecialUpgradeType.FireDamage:
                return SaveSystem.GetInt("fireEggFireDamage", 1) * 75;
            case SpecialUpgradeType.FireSpeed:
                return (int)(Mathf.Round((1.01f - SaveSystem.GetFloat("fireEggFireSpeed", 1f)) * 100)) * 35;
            case SpecialUpgradeType.EnergyRadius:
                return (int)(Mathf.Round((SaveSystem.GetFloat("energyEggRadius", 1.5f) - 1.45f) * 20)) * 75;
            case SpecialUpgradeType.EnergyDelay:
                return (int)(Mathf.Round((1.51f - SaveSystem.GetFloat("energyEggDelay", 1.5f)) * 100)) * 150;
            case SpecialUpgradeType.WaterSlow:
                return (int)(Mathf.Round((0.51f - SaveSystem.GetFloat("waterEggSlow", 0.5f)) * 100)) * 40;
            case SpecialUpgradeType.WaterRadius:
                return (int)(Mathf.Round((SaveSystem.GetFloat("waterEggRadius", 1f) - 0.95f) * 20)) * 75;
        }
        return 0;
    }

}
