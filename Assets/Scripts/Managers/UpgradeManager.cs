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
}
