using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EggPurchaseButton : ScalingButton {
    public int eggType;

    protected override void Start() {
        if (SaveSystem.IsLoaded()) {
            if (GameManager.instance.purchasedEggs.Contains(eggType)) {
                DisableButton();
            } else {
                EnableButton();
            }
        }
    }

    private void DisableButton() {
        GetComponent<Image>().color = new Color32(125, 125, 125, 255);
        transform.Find("CheckMark").gameObject.SetActive(true);
        onClick.RemoveAllListeners();
    }

    private void EnableButton() {
        GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        transform.Find("CheckMark").gameObject.SetActive(false);
        onClick.AddListener(() => ButtonClicked());
    }

    void ButtonClicked() {
        GameManager.instance.PurchaseEgg(eggType);
        DisableButton();
    }
}
