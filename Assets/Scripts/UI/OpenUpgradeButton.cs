using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenUpgradeButton : ScalingButton {
    public int eggType;
    public GameObject upgradeTree;

    private Animator treeAnimator;
    private bool started = false;

    protected override void Start() {
        started = true;
        treeAnimator = upgradeTree.GetComponent<Animator>();
        if (eggType != 0) {
            EnableButton();
        } else {
            Open();
        }
    }

    private void DisableButton() {
        GetComponent<Image>().color = new Color32(125, 125, 125, 255);
        interactable = false;
        onClick.RemoveAllListeners();
        upgradeTree.SetActive(true);
        treeAnimator.SetTrigger("MoveIn");
    }

    private void EnableButton() {
        GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        interactable = true;
        onClick.RemoveAllListeners();
        onClick.AddListener(() => Open());
        if (treeAnimator != null && treeAnimator.isActiveAndEnabled) { 
            treeAnimator.SetTrigger("MoveOut");
        }
        StartCoroutine(WaitAndRemove());
    }

    private IEnumerator WaitAndRemove() {
        yield return new WaitForSeconds(1f);
        upgradeTree.SetActive(false);
    }

    public void Close() {
        EnableButton();
    }

    public void Open() {
        if (started) {
            DisableButton();
            GameManager.instance.uiManager.upgradeManager.SetEggActive(this);
        }
    }
}
