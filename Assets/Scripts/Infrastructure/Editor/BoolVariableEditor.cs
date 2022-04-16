using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoolVariable))]
[CanEditMultipleObjects]
public class BoolVariableDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        BoolVariable subject = (BoolVariable)target;
        EditorGUILayout.LabelField("Value: " + subject.Value);

        if (GUILayout.Button("Toggle"))
        {
            subject.SetValue(!subject.Value);
        }
    }
}
