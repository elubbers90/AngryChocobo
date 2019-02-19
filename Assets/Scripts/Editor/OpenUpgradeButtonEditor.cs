using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OpenUpgradeButton))]
public class OpenUpgradeButtonEditor : UnityEditor.UI.ButtonEditor {
    public override void OnInspectorGUI() {
        OpenUpgradeButton targetButton = (OpenUpgradeButton)target;

        targetButton.eggType = EditorGUILayout.IntField("Egg Type", targetButton.eggType);
        targetButton.upgradeTree = (GameObject)EditorGUILayout.ObjectField("Upgrade Tree", targetButton.upgradeTree, typeof(GameObject), true);

        base.OnInspectorGUI();
    }
}