using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UpgradeButton))]
public class UpgradeButtonEditor : UnityEditor.UI.ButtonEditor {
    public override void OnInspectorGUI() {
        UpgradeButton targetButton = (UpgradeButton)target;

        targetButton.upgradeType = (UpgradeType) EditorGUILayout.EnumPopup("Upgrade Type", targetButton.upgradeType);

        if (targetButton.upgradeType == UpgradeType.SpecialUpgrade) {
            targetButton.specialUpgradeType = (SpecialUpgradeType)EditorGUILayout.EnumPopup("Special Upgrade Type", targetButton.specialUpgradeType);
        } else {
            targetButton.eggType = EditorGUILayout.IntField("Egg Type", targetButton.eggType);

        }

        base.OnInspectorGUI();
    }
}