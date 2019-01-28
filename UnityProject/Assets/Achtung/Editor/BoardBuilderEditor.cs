using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoardBuilder))]
public class BoardBuilderEditor : Editor
{
    private BoardBuilder builder;
    private void OnEnable()
    {
        builder = (BoardBuilder)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        if (GUILayout.Button("Clear"))
        {
            builder.Clear();
        }

        if (GUILayout.Button("Save"))
        {
            builder.Save();
        }
    }
}
