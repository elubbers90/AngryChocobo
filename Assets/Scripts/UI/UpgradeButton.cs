using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum UpgradeType { Purchase, Damage, Speed, EggAmount, SpecialUnlock, SpecialUpgrade }
public enum SpecialUpgradeType { LightningBolt, LightningBoltDamage, FireDamage, FireSpeed }

public class UpgradeButton : ScalingButton {
    public int eggType;
    public UpgradeType upgradeType;
    public SpecialUpgradeType specialUpgradeType;

    protected override void Start() {
        SetState();
    }

    public void SetState() {
        if (SaveSystem.IsLoaded()) {
            bool eggUnlocked = GameManager.instance.purchasedEggs.Contains(eggType);
            if (upgradeType == UpgradeType.Purchase) {
                if (eggUnlocked) {
                    DisableButton(true);
                } else {
                    EnableButton();
                }
            } else if (eggUnlocked) {
                if (upgradeType == UpgradeType.SpecialUpgrade && specialUpgradeType == SpecialUpgradeType.LightningBoltDamage) {
                    if (SaveSystem.GetBool("lightningEggBolt", false)) {
                        EnableButton();
                    } else {
                        DisableButton(false, true);
                    }
                } else if (upgradeType == UpgradeType.Damage ||
                    (upgradeType == UpgradeType.Speed && !IsMaxSpeed()) ||
                    (upgradeType == UpgradeType.EggAmount && !IsMaxEggs()) ||
                    (upgradeType == UpgradeType.SpecialUpgrade && !IsMaxSpecialUpgrade())) {
                    EnableButton();
                } else {
                    DisableButton(true);
                }
            } else {
                DisableButton(false);
            }
        }
    }

    private void DisableButton(bool purchased, bool needNewUnlock = false) {
        GetComponent<Image>().color = new Color32(125, 125, 125, 255);
        Transform checkmark = transform.Find("CheckMark");
        Transform lockImage = transform.Find("Lock");
        bool showLock = false;
        bool showCheck = false;
        if (purchased && !needNewUnlock) {
            showCheck = true;
        } else {
            showLock = true;
        }
        if (checkmark != null) {
            checkmark.gameObject.SetActive(showCheck);
        }
        if (lockImage != null) {
            lockImage.gameObject.SetActive(showLock);
        }
        interactable = false;
        onClick.RemoveAllListeners();
    }

    private void EnableButton() {
        GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        Transform checkmark = transform.Find("CheckMark");
        if (checkmark != null) {
            checkmark.gameObject.SetActive(false);
        }
        Transform lockImage = transform.Find("Lock");
        if (lockImage != null) {
            lockImage.gameObject.SetActive(false);
        }
        interactable = true;
        onClick.RemoveAllListeners();
        onClick.AddListener(() => ButtonClicked());
    }

    private bool IsMaxSpeed() {
        switch (eggType) {
            case 0:
                return SaveSystem.GetFloat("basicEggSpeed", 10f) >= 25f;
            case 1:
                return SaveSystem.GetFloat("lightningEggSpeed", 15f) >= 30f;
            case 2:
                return SaveSystem.GetFloat("fireEggSpeed", 10f) >= 25f;
            case 3:
                return SaveSystem.GetFloat("energyEggSpeed", 10f) >= 25f;
            case 4:
                return SaveSystem.GetFloat("waterEggSpeed", 10f) >= 25f;
        }
        return false;
    }

    private bool IsMaxEggs() {
        switch (eggType) {
            case 1:
                return SaveSystem.GetInt("lightningEggAmount", 20) >= 250;
            case 2:
                return SaveSystem.GetInt("fireEggAmount", 15) >= 100;
            case 3:
                return SaveSystem.GetInt("energyEggAmount", 25) >= 175;
            case 4:
                return SaveSystem.GetInt("waterEggAmount", 15) >= 100;
        }
        return false;
    }

    private bool IsMaxSpecialUpgrade() {
        switch (specialUpgradeType) {
            case SpecialUpgradeType.LightningBolt:
                return SaveSystem.GetBool("lightningEggBolt", false);
            case SpecialUpgradeType.LightningBoltDamage:
                return false;
            case SpecialUpgradeType.FireDamage:
                return false;
            case SpecialUpgradeType.FireSpeed:
                return SaveSystem.GetFloat("fireEggFireSpeed", 1f) <= 0.1f;
        }
        return false;
    }

    void ButtonClicked() {
        if (upgradeType == UpgradeType.Purchase) {
            GameManager.instance.PurchaseEgg(eggType);
            DisableButton(true);
        } else if (upgradeType == UpgradeType.Damage) {
            GameManager.instance.IncreaseEggDamage(eggType);
        } else if (upgradeType == UpgradeType.Speed) {
            GameManager.instance.IncreaseEggSpeed(eggType);
            if (IsMaxSpeed()) {
                DisableButton(true);
            }
        } else if (upgradeType == UpgradeType.EggAmount) {
            GameManager.instance.IncreaseEggAmount(eggType);
            if (IsMaxEggs()) {
                DisableButton(true);
            }
        } else if (upgradeType == UpgradeType.SpecialUpgrade) {
            GameManager.instance.UpgradeSpecial(specialUpgradeType);
            if (IsMaxSpecialUpgrade()) {
                DisableButton(true);
            }
        }
    }
}
