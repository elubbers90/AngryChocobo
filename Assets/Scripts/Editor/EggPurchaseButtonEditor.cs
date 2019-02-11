using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EggPurchaseButton))]
public class EggPurchaseButtonEditor : UnityEditor.UI.ButtonEditor {
    public override void OnInspectorGUI() {
        EggPurchaseButton targetButton = (EggPurchaseButton)target;

        targetButton.eggType = EditorGUILayout.IntField("Egg Type", targetButton.eggType);

        base.OnInspectorGUI();
    }
}