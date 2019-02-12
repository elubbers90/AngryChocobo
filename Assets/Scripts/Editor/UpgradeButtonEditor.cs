using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UpgradeButton))]
public class UpgradeButtonEditor : UnityEditor.UI.ButtonEditor {
    public override void OnInspectorGUI() {
        UpgradeButton targetButton = (UpgradeButton)target;

        targetButton.eggType = EditorGUILayout.IntField("Egg Type", targetButton.eggType);

        targetButton.upgradeType = (UpgradeType) EditorGUILayout.EnumPopup("Upgrade Type", targetButton.upgradeType);

        base.OnInspectorGUI();
    }
}