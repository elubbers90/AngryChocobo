using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum UpgradeType { Purchase, Damage, Speed, SpecialUnlock }

public class UpgradeButton : ScalingButton {
    public int eggType;
    public UpgradeType upgradeType;

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
            } else if (upgradeType == UpgradeType.Damage) {
                if (eggUnlocked) {
                    EnableButton();
                } else {
                    DisableButton(false);
                }
            } else if (upgradeType == UpgradeType.Speed) {
                if (eggUnlocked) {
                    if (!IsMaxSpeed()) {
                        EnableButton();
                    } else {
                        DisableButton(true);
                    }
                } else {
                    DisableButton(false);
                }
            }
        }
    }

    private void DisableButton(bool purchased) {
        GetComponent<Image>().color = new Color32(125, 125, 125, 255);
        if (purchased) {
            Transform checkmark = transform.Find("CheckMark");
            if (checkmark != null) {
                checkmark.gameObject.SetActive(true);
            }
        } else {
            Transform lockImage = transform.Find("Lock");
            if (lockImage != null) {
                lockImage.gameObject.SetActive(true);
            }
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
        }
    }
}
