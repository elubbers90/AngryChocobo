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
            case UpgradeType.Damage:
                return GetDamageCost(eggType);
            case UpgradeType.Speed:
                return GetSpeedCost(eggType);
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
                return (int)((SaveSystem.GetFloat("basicEggSpeed", 10f) -9.9f) * 750);
            case 1:
                return (int)((SaveSystem.GetFloat("lightningEggSpeed", 15f) - 14.9f) * 500);
            case 2:
                return (int)((SaveSystem.GetFloat("fireEggSpeed", 10f) - 9.9f) * 750);
            case 3:
                return (int)((SaveSystem.GetFloat("energyEggSpeed", 10f) - 9.9f) * 750);
            case 4:
                return (int)((SaveSystem.GetFloat("waterEggSpeed", 10f) - 9.9f) * 750);
        }
        return 0;
    }

}
