using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum UpgradeType { Purchase, Damage, Speed, EggAmount, SpecialUpgrade }
public enum SpecialUpgradeType { LightningBolt, LightningBoltDamage, FireDamage, FireSpeed, EnergyRadius, EnergyDelay, WaterRadius, WaterSlow}

public class UpgradeButton : ScalingButton {
    public int eggType;
    public UpgradeType upgradeType;
    public SpecialUpgradeType specialUpgradeType;

    private int cost;

    private Transform checkmark;
    private Transform lockImage;
    private Transform candy;
    private Transform candyAmount;

    protected override void Start() {
        checkmark = transform.Find("CheckMark");
        lockImage = transform.Find("Lock");
        candy = transform.Find("Candy");
        candyAmount = transform.Find("CandyAmount");
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
        if (upgradeType == UpgradeType.Purchase && purchased) {
            gameObject.SetActive(false);
        } else {
            GetComponent<Image>().color = new Color32(125, 125, 125, 255);
            bool showLock = false;
            bool showCheck = false;
            if (purchased && !needNewUnlock) {
                showCheck = true;
            } else {
                showLock = true;
            }
            if (lockImage != null) {
                lockImage.gameObject.SetActive(showLock);
            }
            if (checkmark != null) {
                checkmark.gameObject.SetActive(showCheck);
            }
            if (candy != null) {
                candy.gameObject.SetActive(false);
            }
            if (candyAmount != null) {
                candyAmount.gameObject.SetActive(false);
            }

            interactable = false;
            onClick.RemoveAllListeners();
        }
    }

    private void EnableButton() {
        gameObject.SetActive(true);
        cost = GameManager.instance.uiManager.upgradeManager.GetCost(eggType, upgradeType, specialUpgradeType);
        GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        if (checkmark != null) {
            checkmark.gameObject.SetActive(false);
        }
        if (lockImage != null) {
            lockImage.gameObject.SetActive(false);
        }
        if (candy != null) {
            candy.gameObject.SetActive(true);
        }
        interactable = true;
        if (candyAmount != null) {
            bool notAffordable = upgradeType == UpgradeType.Purchase ? cost > GameManager.instance.totalCakes : cost > GameManager.instance.totalCandy;
            if (notAffordable) {
                candyAmount.GetComponent<Text>().color = new Color32(255, 0, 0, 255);
                interactable = false;
            } else {
                candyAmount.GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            }
            candyAmount.GetComponent<Text>().text = "x " + cost;
            candyAmount.gameObject.SetActive(true);
        }
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
            case SpecialUpgradeType.EnergyRadius:
                return SaveSystem.GetFloat("energyEggRadius", 1.5f) >= 10f;
            case SpecialUpgradeType.EnergyDelay:
                return SaveSystem.GetFloat("energyEggDelay", 1.5f) <= 0f;
            case SpecialUpgradeType.WaterSlow:
                return SaveSystem.GetFloat("waterEggSlow", 0.5f) <= 0.1f;
            case SpecialUpgradeType.WaterRadius:
                return SaveSystem.GetFloat("waterEggRadius", 1f) >= 10f;
        }
        return false;
    }

    void ButtonClicked() {
        bool affordable = upgradeType == UpgradeType.Purchase ? cost <= GameManager.instance.totalCakes : cost <= GameManager.instance.totalCandy;
        if (affordable) {
            if (upgradeType == UpgradeType.Purchase) {
                GameManager.instance.PayCakes(cost);
                GameManager.instance.PurchaseEgg(eggType);
            } else {
                GameManager.instance.PayCandy(cost);
                if (upgradeType == UpgradeType.Damage) {
                    GameManager.instance.IncreaseEggDamage(eggType);
                } else if (upgradeType == UpgradeType.Speed) {
                    GameManager.instance.IncreaseEggSpeed(eggType);
                } else if (upgradeType == UpgradeType.EggAmount) {
                    GameManager.instance.IncreaseEggAmount(eggType);
                } else if (upgradeType == UpgradeType.SpecialUpgrade) {
                    GameManager.instance.UpgradeSpecial(specialUpgradeType);
                }
            }
            GameManager.instance.uiManager.UpdateUpgradeScreenButtons();
        }
    }
}
