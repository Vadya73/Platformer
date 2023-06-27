using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEditor.UI;
using UnityEngine;

namespace Scripts.UI.Widgets.Editor
{
    [CustomEditor(typeof(CustomButton), true)]
    [CanEditMultipleObjects]
    public class CustomButtonEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_normalState"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_pressedState"));
            serializedObject.ApplyModifiedProperties();
            
            base.OnInspectorGUI();
        }
    }
}